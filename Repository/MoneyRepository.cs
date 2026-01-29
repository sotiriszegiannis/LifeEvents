using AutoMapper;
using Domain;
using Helper;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class MoneyRepository : BaseRepository<MoneyTransaction>
    {
        private IMapper Mapper { get; set; }

        public MoneyRepository(IDbContextFactory<AppDbContext> dbContextFactory, ITenantResolver tenantResolver, IMapper mapper) : base(dbContextFactory, tenantResolver)
        {
            Mapper = mapper;
        }

        public async Task<List<MoneyTransactionRDTO>> GetAll(DateTime from, DateTime to, UserRDTO user)
        {
            using (DbContext db = await GetDbContext())
            {
                var utcFrom = from.FromIanaTimeZone(user.TimeZone);
                var utcTo = to.FromIanaTimeZone(user.TimeZone);
                var lEvents = (db as AppDbContext).LifeEvents
                                .Where(p => p.User.Id == user.Id && p.From >= utcFrom && p.To <= utcTo && p.MoneyTransaction!=null)
                                .Select(p => new { p.MoneyTransaction, p })
                                .ToList();
                return lEvents.Select(p => new MoneyTransactionRDTO()
                {
                    Id = p.MoneyTransaction.Id,
                    Amount = p.MoneyTransaction.Amount,
                    Type = p.MoneyTransaction.Type,
                    Date= p.p.DateCreated
                }).ToList();
            }
        }
        public async Task<List<MoneyTransactionGroupRDTO>> GetPerMonth(DateTime from, DateTime to, UserRDTO user,MoneyTransactionTypeEnum moneyTransactionType)
        {
            using (DbContext db = await GetDbContext())
            {
                var utcFrom = from.FromIanaTimeZone(user.TimeZone);
                var utcTo = to.FromIanaTimeZone(user.TimeZone);
                var lEvents = (db as AppDbContext)!.LifeEvents
                                .Where(p => p.User.Id == user.Id && p.From >= utcFrom && p.To <= utcTo && p.MoneyTransaction != null && p.MoneyTransaction.Type == moneyTransactionType)
                                .GroupBy(p => new { p.DateCreated.Year, p.DateCreated.Month })
                                .Select(p => new { 
                                    Sum = p.Sum(x => x.MoneyTransaction!.Amount), 
                                    Date = new DateTime(p.Key.Year, p.Key.Month, 1, 0, 0, 0, DateTimeKind.Utc) 
                                })
                                .ToList();
                return lEvents.Select(p => new MoneyTransactionGroupRDTO()
                {
                    Amount = p.Sum,
                    Type = moneyTransactionType,
                    Month = p.Date
                }).ToList();
            }
        }
    }
}