using AutoMapper;
using Domain;
using WebApp;

namespace Repository
{
    public class TagSummaryProfile : Profile
    {
        public TagSummaryProfile()
        {
            CreateMap<TagSummaryRDTO, TagSummary>().ReverseMap();
        }
    }
}
