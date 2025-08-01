using BooksCrudApi.Services;
using BooksCrudApi.Repositories;
using BooksCrudApi.Data;
using Microsoft.EntityFrameworkCore;
using Azure.Monitor.OpenTelemetry.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add OpenTelemetry and configure it to use Azure Monitor.
builder.Services.AddOpenTelemetry().UseAzureMonitor();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework with In-Memory Database
DatabaseConfigurationService.ConfigureDatabase(builder.Services, builder.Configuration);

// Register repositories
builder.Services.AddScoped<IBookRepository, BookRepository>();

// Register the BookService
builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Enable Swagger in both Development and Production
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
