using AutoMapper;
using MyBlazorServerApp.Domain;
using MyBlazorServerApp.Resources;

namespace MyBlazorServerApp.MappingProfiles;

/// <summary>
/// AutoMapper profile for mapping between DurationEntity and Duration resource objects.
/// This profile defines the bidirectional mapping configuration for duration-related entities and DTOs.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DurationProfile"/> class.
/// Configures mappings between domain entities and API resources for duration calculations.
/// </remarks>
public class DurationProfile : Profile
{
  /// <summary>
  /// Initializes a new instance of the <see cref="DurationProfile"/> class.
  /// Configures the mapping rules between DurationEntity and Duration types.
  /// </summary>
  public DurationProfile()
  {
    CreateMap<DurationEntity, Duration>();

    CreateMap<Duration, DurationEntity>()
      .ForMember(dest => dest.Id, opt => opt.Ignore())
      .ForMember(dest => dest.Created, opt => opt.Ignore());
  }
}
