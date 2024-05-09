using bislerium_blogs.Data;
using bislerium_blogs.Models;
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
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<CustomUser>()
    .AddEntityFrameworkStores<DataContext>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
policy.WithOrigins("http://localhost:3000", "https://localhost:3001")
.AllowAnyMethod()
.AllowAnyHeader()
.WithExposedHeaders("Authorization")
.WithHeaders(HeaderNames.ContentType)
);

app.MapIdentityApi<CustomUser>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
