using AutoMapper;
using Domain;
namespace Repository
{
    public class LifeEventProfile : Profile
    {
        public LifeEventProfile()
        {
            CreateMap<LifeEvent, LifeEventRDTO>().ReverseMap();
        }
    }
}
