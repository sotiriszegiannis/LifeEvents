using AutoMapper;
using Domain;
namespace Repository
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagRDTO>().ReverseMap();
        }
    }
}
