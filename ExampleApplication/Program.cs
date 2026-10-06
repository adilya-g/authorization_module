using System.Globalization;
using ExampleApplication.Database;
using ExampleApplication.Repository;
using ExampleApplication.Service;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using OpenSearch.Net;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Sinks.OpenSearch;
using Serilog;
using Serilog.Sinks.OpenSearch;

var builder = WebApplication.CreateBuilder(args);

var openSearchUri = builder.Configuration["OpenSearch:NodeUri"];
var osUsername = builder.Configuration["OpenSearch:Username"];
var osPassword = builder.Configuration["OpenSearch:Password"];
var indexFormat = builder.Configuration["OpenSearch:IndexFormat"];

var openSearchOptions = new OpenSearchSinkOptions(new Uri(openSearchUri!))
{
    IndexFormat = indexFormat,
    ModifyConnectionSettings = c => c
        .BasicAuthentication(osUsername!, osPassword!)
        .ServerCertificateValidationCallback((o, cert, chain, errors) => true)
};

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.OpenSearch(openSearchOptions)
    .CreateLogger();

builder.Host.UseSerilog();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.OpenSearch(openSearchOptions)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserManageService>();
builder.Services.AddScoped<AuthorizationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();