using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StudentManagementSystem.Data;
using StudentManagementSystem.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register ApplicationDbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register controllers for API and MVC
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Register Swagger for API documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Student Management System API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger and Swagger UI in development mode
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Management System API v1");
    });
}

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Serve static files (e.g., CSS, JS, images)
app.UseStaticFiles();

// Enable routing
app.UseRouting();

// Enable authorization (if needed)
app.UseAuthorization();

// Map API controllers
app.MapControllers();

// Map MVC controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Run the application
app.Run();