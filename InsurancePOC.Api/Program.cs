using Microsoft.EntityFrameworkCore;
using FluentValidation;
using AutoMapper;
using InsurancePOC.Api.Data;
using InsurancePOC.Api.Models;
using InsurancePOC.Shared.Dtos;
using InsurancePOC.Shared.Validation;
using Microsoft.AspNetCore.Http.HttpResults;
using InsurancePOC.Api.Controllers;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI / Swagger (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateClientDtoValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Home endpoint
app.MapGet("/", () => Results.Ok("Hello from InsurancePOC API"))
   .WithName("Home")
   .WithSummary("Health / home endpoint")
   .Produces<string>(StatusCodes.Status200OK);

// Map endpoints moved to controllers
app.MapClientEndpoints();
app.MapPolicyEndpoints();

app.Run();
