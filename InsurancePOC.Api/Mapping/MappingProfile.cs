using AutoMapper;
using InsurancePOC.Api.Models;
using InsurancePOC.Shared.Dtos;


namespace InsurancePOC.Api.Mapping;

/// <summary>
/// AutoMapper configuration profile defining all DTO <-> Entity mappings.
/// Automatically discovered and registered by AutoMapper at startup.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Client mappings: handle conversion between domain models and DTOs
        CreateMap<Client, ClientDto>();
        CreateMap<CreateClientDto, Client>();
        CreateMap<UpdateClientDto, Client>();

        // Policy mappings: handle conversion between domain models and DTOs
        CreateMap<Policy, PolicyDto>();
        CreateMap<CreatePolicyDto, Policy>();
        CreateMap<UpdatePolicyDto, Policy>();
    }
}