using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FluentValidation;
using InsurancePOC.Api.Data;
using InsurancePOC.Api.Models;
using InsurancePOC.Shared.Dtos;

namespace InsurancePOC.Api.Controllers
{
    public static class ClientEndpoints
    {
        public static void MapClientEndpoints(this WebApplication app)
        {
            var clientGroup = app.MapGroup("/api/client")
                .WithTags("Client");

            clientGroup.MapGet("/", async (AppDbContext context, IMapper mapper) =>
            {
                var clients = await context.Clients.ToListAsync();
                return mapper.Map<List<ClientDto>>(clients);
            })
            .WithName("GetAllClients")
            .WithSummary("Retrieves all clients from the database")
            .Produces<List<ClientDto>>(StatusCodes.Status200OK);

            clientGroup.MapGet("/{id}", async Task<Results<Ok<ClientDto>, NotFound>> (int id, AppDbContext context, IMapper mapper) =>
            {
                var client = await context.Clients.FindAsync(id);
                if (client == null) return TypedResults.NotFound();
                return TypedResults.Ok(mapper.Map<ClientDto>(client));
            })
            .WithName("GetClientById")
            .WithSummary("Retrieves a specific client by ID")
            .Produces<ClientDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            clientGroup.MapPost("/", async Task<Results<CreatedAtRoute<ClientDto>, ValidationProblem>>
                (CreateClientDto dto, AppDbContext context, IMapper mapper, IValidator<CreateClientDto> validator) =>
            {
                var validationResult = await validator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    return TypedResults.ValidationProblem(validationResult.ToDictionary());
                }

                var client = mapper.Map<Client>(dto);
                context.Clients.Add(client);
                await context.SaveChangesAsync();
                var result = mapper.Map<ClientDto>(client);
                return TypedResults.CreatedAtRoute(result, "GetClientById", new { id = client.Id });
            })
            .WithName("CreateClient")
            .WithSummary("Creates a new client record")
            .Produces<ClientDto>(StatusCodes.Status201Created)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest);

            clientGroup.MapPut("/{id}", async Task<Results<NoContent, NotFound, ValidationProblem>>
                (int id, UpdateClientDto dto, AppDbContext context, IMapper mapper, IValidator<UpdateClientDto> validator) =>
            {
                var validationResult = await validator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    return TypedResults.ValidationProblem(validationResult.ToDictionary());
                }

                var client = await context.Clients.FindAsync(id);
                if (client == null) return TypedResults.NotFound();

                mapper.Map(dto, client);
                await context.SaveChangesAsync();
                return TypedResults.NoContent();
            })
            .WithName("UpdateClient")
            .WithSummary("Updates an existing client's information")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest);

            clientGroup.MapDelete("/{id}", async Task<Results<NoContent, NotFound>> (int id, AppDbContext context) =>
            {
                var client = await context.Clients.FindAsync(id);
                if (client == null) return TypedResults.NotFound();

                context.Clients.Remove(client);
                await context.SaveChangesAsync();
                return TypedResults.NoContent();
            })
            .WithName("DeleteClient")
            .WithSummary("Deletes a client record")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
