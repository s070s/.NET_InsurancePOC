using AutoMapper;
using InsurancePOC.Api.Data;
using InsurancePOC.Api.Models;
using InsurancePOC.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace InsurancePOC.Api.Controllers;

/// <summary>
/// API controller for managing client records.
/// Provides CRUD operations for insurance clients.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    
    public ClientController(AppDbContext context,IMapper mapper)
    {
        _context=context;
        _mapper=mapper;
    }
    
    /// <summary>
    /// Retrieves all clients from the database.
    /// </summary>
    /// <returns>List of all clients as DTOs</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientDto>>> GetAll()
    {
        var clients = await _context.Clients.ToListAsync();
        return Ok(_mapper.Map<List<ClientDto>>(clients));
    }

    /// <summary>
    /// Retrieves a specific client by ID.
    /// </summary>
    /// <param name="id">The client's unique identifier</param>
    /// <returns>Client DTO if found, 404 if not found</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ClientDto>> GetById(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if(client == null) return NotFound();
        return Ok(_mapper.Map<ClientDto>(client));
    }

    /// <summary>
    /// Creates a new client record.
    /// </summary>
    /// <param name="dto">Client creation data</param>
    /// <returns>201 Created with the new client's location and data</returns>
    [HttpPost]
    public async Task<ActionResult<ClientDto>> Create(CreateClientDto dto)
    {
        var client = _mapper.Map<Client>(dto);
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        var result = _mapper.Map<ClientDto>(client);
        // Returns 201 Created with Location header pointing to the new resource
        return CreatedAtAction(nameof(GetById), new { id = client.Id }, result);
    }
    /// <summary>
    /// Updates an existing client's information.
    /// </summary>
    /// <param name="id">The client's unique identifier</param>
    /// <param name="dto">Updated client data</param>
    /// <returns>204 No Content if successful, 404 if client not found</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateClientDto dto)
    {
        var client = await _context.Clients.FindAsync(id);
        if(client == null) return NotFound();
        // AutoMapper updates the existing entity with DTO values
        _mapper.Map(dto,client);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    /// <summary>
    /// Deletes a client record.
    /// </summary>
    /// <param name="id">The client's unique identifier</param>
    /// <returns>204 No Content if successful, 404 if client not found</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if(client == null) return NotFound();
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}