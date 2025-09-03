using AutoMapper;
using MyBlazorServerApp.Domain;
using MyBlazorServerApp.Resources;

namespace MyBlazorServerApp.MappingProfiles;

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