using bislerium_blogs.Data;
using bislerium_blogs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapIdentityApi<CustomUser>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
