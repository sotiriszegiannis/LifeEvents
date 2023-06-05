using AutoMapper;
using Domain;
using Helper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    public class MoneyRepository : BaseRepository<MoneyTransaction>
    {
                
        IMapper Mapper { get; set; }
        public MoneyRepository(IDbContextFactory<AppDbContext> dbContextFactory, ITenantResolver tenantResolver, IMapper mapper) : base(dbContextFactory, tenantResolver) { 
            Mapper = mapper;            
        }                
    }
}
