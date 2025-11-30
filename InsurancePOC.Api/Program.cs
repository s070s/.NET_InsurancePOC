using Microsoft.EntityFrameworkCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Configure OpenAPI/Swagger for API documentation
builder.Services.AddOpenApi();

// Configure Entity Framework with SQL Server
// Connection string is stored in appsettings.json
builder.Services.AddDbContext<InsurancePOC.Api.Data.AppDbContext>(
options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register AutoMapper for DTO <-> Entity mappings
builder.Services.AddAutoMapper(typeof(Program));

// Register FluentValidation validators from the Shared project
builder.Services.AddValidatorsFromAssemblyContaining<CreateClientDtoValidator>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
