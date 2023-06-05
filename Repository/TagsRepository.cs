using AutoMapper;
using Domain;
using Helper;
using Microsoft.EntityFrameworkCore;
using Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp;

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
        public async Task<TagSummaryRDTO> GetSummary(int id,DateTime from,DateTime to,UserRDTO user)
        {
            using (DbContext db = await GetDbContext())
            {
                var utcFrom = from.FromIanaTimeZone(user.TimeZone);
                var utcTo = to.FromIanaTimeZone(user.TimeZone);
                var lEvents = (db as AppDbContext).LifeEvents
                                .Where(p => p.From >= utcFrom && p.To <= utcTo && p.Tags.Any(x => x.Id == id))
                                .Select(p => new { p.MoneyTransaction, p })
                                .ToList();
                 return new TagSummaryRDTO()
                {
                    Expenses = (int)lEvents.Where(p => p.MoneyTransaction != null && p.MoneyTransaction.Type == MoneyTransactionTypeEnum.Expense).Sum(p => p.MoneyTransaction!.Amount),
                    Income= (int)lEvents.Where(p => p.MoneyTransaction != null && p.MoneyTransaction.Type == MoneyTransactionTypeEnum.Income).Sum(p => p.MoneyTransaction!.Amount),
                    Minutes= (int)lEvents.Sum(x=> x.p.From.GetTotalMinutes(x.p.To)),
                    Occurences=lEvents.Count(),
                    From=from,
                    To=to
                };
            }
        }
    }
}
