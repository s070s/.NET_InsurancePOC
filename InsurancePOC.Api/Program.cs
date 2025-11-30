using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using InsurancePOC.Api.Data;
using InsurancePOC.Shared.Dtos;
using InsurancePOC.Shared.Validation;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

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
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
