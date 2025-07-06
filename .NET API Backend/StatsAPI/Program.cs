using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// Register controllers (API endpoints)
builder.Services.AddControllers();

// Enable API explorer for Swagger/OpenAPI documentation
builder.Services.AddEndpointsApiExplorer();

// Register Swagger generator to create API docs and UI
builder.Services.AddSwaggerGen();

// Configure Entity Framework Core with SQLite database connection
builder.Services.AddDbContext<StatsDbContext>(options =>
    options.UseSqlite("Data Source=stats.db"));

// Build the application
var app = builder.Build();

// Configure middleware pipeline

// If in development environment, enable Swagger UI and JSON endpoint
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable authorization middleware (can add auth later if needed)
app.UseAuthorization();

// Map controller endpoints
app.MapControllers();

// Run the app (start listening for HTTP requests)
app.Run();
