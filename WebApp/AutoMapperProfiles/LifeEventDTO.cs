using AutoMapper;
using Domain;
using Repository;
using WebApp.DTOs.RepositoryDTOs;

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
