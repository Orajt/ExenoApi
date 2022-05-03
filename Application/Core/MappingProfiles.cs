using AutoMapper;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
             CreateMap<Domain.Product, Domain.Product>();
        }
    }
}