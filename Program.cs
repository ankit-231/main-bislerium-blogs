using bislerium_blogs.Data;
using bislerium_blogs.Models;
using bislerium_blogs.Services.Implementations;
using bislerium_blogs.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.ConfigureKestrel((context, options) =>
//{
//    options.ListenAnyIP(8081); // Change port if needed
//});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// identity authentication
//builder.Services.AddIdentity<CustomUser, IdentityRole>()
//    .AddEntityFrameworkStores<DataContext>()
//    .AddDefaultTokenProviders();
//builder.Services.AddAuthentication();

// database connection

//builder.Services.AddDbContext<DataContext>(options =>
//{
//    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//    //options.UseMySQL("server=localhost;database=library;user=user;password=password");
//    options.UseMySQL(builder.Configuration.GetConnectionString("BisleriumBlogsDB"));
//});

builder.Services.AddDbContext<DataContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("BisleriumBlogsDB")));
//builder.Services.AddAuthorization(options =>
//    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"))


//    );

//builder.Services.AddIdentity<CustomUser, IdentityRole>()
//       .AddEntityFrameworkStores<DataContext>()
//    .AddDefaultTokenProviders();

// RoleManager for managing roles
//builder.Services.AddScoped<RoleManager<IdentityRole>>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBlogService, BlogService>();

//builder.Services.AddScoped<IGmailEmailProvider, GmailEmailProvider>();


builder.Services.AddIdentityApiEndpoints<CustomUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DataContext>();

//builder.Services.AddScoped<IAuthService, AuthService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
.AllowAnyMethod()
.AllowAnyHeader()
.WithExposedHeaders("Authorization")
.WithHeaders(HeaderNames.ContentType)
);

app.MapIdentityApi<CustomUser>();

app.UseHttpsRedirection();

//app.UseWebSockets();



app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new List<string> { "admin", "user" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}


app.Run();
