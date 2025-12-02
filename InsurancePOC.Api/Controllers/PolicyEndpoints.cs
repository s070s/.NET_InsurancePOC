using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FluentValidation;
using InsurancePOC.Api.Data;
using InsurancePOC.Api.Models;
using InsurancePOC.Shared.Dtos;


namespace InsurancePOC.Api.Controllers
{
    public static class PolicyEndpoints
    {
        public static void MapPolicyEndpoints(this WebApplication app)
        {
            var policyGroup = app.MapGroup("/api/policy")
                .WithTags("Policy");

            policyGroup.MapGet("/", async (AppDbContext context, IMapper mapper) =>
            {
                var policies = await context.Policies.ToListAsync();
                return mapper.Map<List<PolicyDto>>(policies);
            })
            .WithName("GetAllPolicies")
            .WithSummary("Retrieves all policies from the database")
            .Produces<List<PolicyDto>>(StatusCodes.Status200OK);

            policyGroup.MapGet("/{id}", async Task<Results<Ok<PolicyDto>, NotFound>> (int id, AppDbContext context, IMapper mapper) =>
            {
                var policy = await context.Policies.FindAsync(id);
                if (policy == null) return TypedResults.NotFound();
                return TypedResults.Ok(mapper.Map<PolicyDto>(policy));
            })
            .WithName("GetPolicyById")
            .WithSummary("Retrieves a specific policy by ID")
            .Produces<PolicyDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            policyGroup.MapPost("/", async Task<Results<CreatedAtRoute<PolicyDto>, ValidationProblem>>
                (CreatePolicyDto dto, AppDbContext context, IMapper mapper, IValidator<CreatePolicyDto> validator) =>
            {
                var validationResult = await validator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    return TypedResults.ValidationProblem(validationResult.ToDictionary());
                }

                var policy = mapper.Map<Policy>(dto);
                context.Policies.Add(policy);
                await context.SaveChangesAsync();
                var result = mapper.Map<PolicyDto>(policy);
                return TypedResults.CreatedAtRoute(result, "GetPolicyById", new { id = policy.Id });
            })
            .WithName("CreatePolicy")
            .WithSummary("Creates a new insurance policy")
            .Produces<PolicyDto>(StatusCodes.Status201Created)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest);

            policyGroup.MapPut("/{id}", async Task<Results<NoContent, NotFound, ValidationProblem>>
                (int id, UpdatePolicyDto dto, AppDbContext context, IMapper mapper, IValidator<UpdatePolicyDto> validator) =>
            {
                var validationResult = await validator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    return TypedResults.ValidationProblem(validationResult.ToDictionary());
                }

                var policy = await context.Policies.FindAsync(id);
                if (policy == null) return TypedResults.NotFound();

                mapper.Map(dto, policy);
                await context.SaveChangesAsync();
                return TypedResults.NoContent();
            })
            .WithName("UpdatePolicy")
            .WithSummary("Updates an existing policy's information")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest);

            policyGroup.MapDelete("/{id}", async Task<Results<NoContent, NotFound>> (int id, AppDbContext context) =>
            {
                var policy = await context.Policies.FindAsync(id);
                if (policy == null) return TypedResults.NotFound();

                context.Policies.Remove(policy);
                await context.SaveChangesAsync();
                return TypedResults.NoContent();
            })
            .WithName("DeletePolicy")
            .WithSummary("Deletes a policy record")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
