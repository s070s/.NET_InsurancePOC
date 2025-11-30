using AutoMapper;
using InsurancePOC.Api.Data;
using InsurancePOC.Api.Models;
using InsurancePOC.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace InsurancePOC.Api.Controllers;

/// <summary>
/// API controller for managing insurance policies.
/// Provides CRUD operations for policy records.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PolicyController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    
    public PolicyController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all policies from the database.
    /// </summary>
    /// <returns>List of all policies as DTOs</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PolicyDto>>> GetAll()
    {
        var policies = await _context.Policies.ToListAsync();
        return Ok(_mapper.Map<List<PolicyDto>>(policies));
    }

    /// <summary>
    /// Retrieves a specific policy by ID.
    /// </summary>
    /// <param name="id">The policy's unique identifier</param>
    /// <returns>Policy DTO if found, 404 if not found</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PolicyDto>> GetById(int id)
    {
        var policy = await _context.Policies.FindAsync(id);
        if(policy == null) return NotFound();
        return Ok(_mapper.Map<PolicyDto>(policy));
    }

    /// <summary>
    /// Creates a new insurance policy.
    /// </summary>
    /// <param name="dto">Policy creation data including client reference</param>
    /// <returns>201 Created with the new policy's location and data</returns>
    [HttpPost]
    public async Task<ActionResult<PolicyDto>> Create(CreatePolicyDto dto)
    {
        // TODO: Add validation to ensure ClientId exists before creating policy
        var policy = _mapper.Map<Policy>(dto);
        _context.Policies.Add(policy);
        await _context.SaveChangesAsync();
        var result = _mapper.Map<PolicyDto>(policy);
        return CreatedAtAction(nameof(GetById), new { id = policy.Id }, result);
    }

    /// <summary>
    /// Updates an existing policy's information.
    /// </summary>
    /// <param name="id">The policy's unique identifier</param>
    /// <param name="dto">Updated policy data</param>
    /// <returns>204 No Content if successful, 404 if policy not found</returns>
    [HttpPut("{id}")]
     public async Task<ActionResult> Update(int id, UpdatePolicyDto dto)
    {
        var policy = await _context.Policies.FindAsync(id);
        if (policy == null) return NotFound();

        // AutoMapper updates the existing entity with DTO values
        _mapper.Map(dto, policy);

        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    /// <summary>
    /// Deletes a policy record.
    /// </summary>
    /// <param name="id">The policy's unique identifier</param>
    /// <returns>204 No Content if successful, 404 if policy not found</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var policy = await _context.Policies.FindAsync(id);
        if (policy == null) return NotFound();

        _context.Policies.Remove(policy);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}