using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
{
    public class TagsRepository : BaseRepository<Tag>
    {
        IMapper Mapper { get; set; }
        public TagsRepository(IDbContextFactory<AppDbContext> dbContextFactory,ITenantResolver tenantResolver,IMapper mapper) : base(dbContextFactory, tenantResolver) { 
            Mapper = mapper;
        }
        public async Task<List<TagRDTO>> GetAll()
        {
            return Mapper.Map<List<Tag>, List<TagRDTO>>(await base.GetAll());
        }
    }
}
