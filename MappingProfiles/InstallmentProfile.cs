using AutoMapper;
using MyBlazorServerApp.Domain;
using MyBlazorServerApp.Resources;

namespace MyBlazorServerApp.MappingProfiles;

/// <summary>
/// AutoMapper profile for mapping between InstallmentEntity and Installment resource objects.
/// This profile defines the bidirectional mapping configuration for duration-related entities and DTOs.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="InstallmentProfile"/> class.
/// Configures mappings between domain entities and API resources for duration calculations.
/// </remarks>
public class InstallmentProfile : Profile
{
  public InstallmentProfile()
  {
    CreateMap<InstallmentEntity, Installment>();

    CreateMap<Installment, InstallmentEntity>()
      .ForMember(dest => dest.Id, opt => opt.Ignore())
      .ForMember(dest => dest.Created, opt => opt.Ignore());
  }
}