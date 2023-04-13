using AutoMapper;
using Domain;
using Repository;

namespace WebApp
{
    public class LifeEventProfile : Profile
    {
        public LifeEventProfile()
        {
            CreateMap<LifeEvent, LifeEventDTO>().ReverseMap();
        }
    }
}
