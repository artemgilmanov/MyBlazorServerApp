using AutoMapper;
using MyBlazorServerApp.Domain;
using MyBlazorServerApp.Resources;

namespace MyBlazorServerApp.MappingProfiles;

public class DurationProfile : Profile
{
  public DurationProfile()
  {
    CreateMap<DurationEntity, Duration>();

    CreateMap<Duration, DurationEntity>()
      .ForMember(dest => dest.Id, opt => opt.Ignore())
      .ForMember(dest => dest.Created, opt => opt.Ignore());
  }
}
