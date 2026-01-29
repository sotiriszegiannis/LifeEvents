using AutoMapper;
using Domain;
using Helper;
using Microsoft.EntityFrameworkCore;
using WebApp;

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
        public async Task<List<MoneyTransactionPerMonthRDTO>> GetPerMonth(DateTime from, DateTime to, UserRDTO user,MoneyTransactionTypeEnum moneyTransactionType)
        {
            using (DbContext db = await GetDbContext())
            {
                var utcFrom = from.FromIanaTimeZone(user.TimeZone);
                var utcTo = to.FromIanaTimeZone(user.TimeZone);
                var lEvents = (db as AppDbContext)!.LifeEvents
                                .Where(p => p.User.Id == user.Id && p.From >= utcFrom && p.To <= utcTo && p.MoneyTransaction != null && p.MoneyTransaction.Type == moneyTransactionType)
                                .GroupBy(p => new { p.DateCreated.Year, p.DateCreated.Month })
                                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                                .Select(p => new { 
                                    Sum = p.Sum(x => x.MoneyTransaction!.Amount), 
                                    Date = new DateTime(p.Key.Year, p.Key.Month, 1, 0, 0, 0, DateTimeKind.Utc) 
                                })
                                .ToList();
                return lEvents.Select(p => new MoneyTransactionPerMonthRDTO()
                {
                    Amount = p.Sum,
                    Type = moneyTransactionType,
                    Month = p.Date
                }).ToList();
            }
        }

        public async Task<List<TagTransactionPerMonthRDTO>> Get10MostTagsPerMonth(DateTime from, DateTime to, UserRDTO user)
        {
            using (DbContext db = await GetDbContext())
            {

                var utcFrom = from.FromIanaTimeZone(user.TimeZone);
                var utcTo = to.FromIanaTimeZone(user.TimeZone);
                var qRes = (db as AppDbContext).LifeEvents
                                .Include(p => p.MoneyTransaction)
                                .Include(p => p.Tags)
                                .Where(p => p.From >= utcFrom && p.To <= utcTo)
                                .ToList();
                var lEvents = qRes
                                .SelectMany(p=>p.Tags!.Select(t => new { p.MoneyTransaction, p.DateCreated, Tag = t }))
                                .GroupBy(p => new { p.Tag, p.DateCreated.Year, p.DateCreated.Month })
                                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                                .Select(p => new {
                                    Income = p
                                                .Where(x => x.MoneyTransaction != null && 
                                                      x.MoneyTransaction.Type == MoneyTransactionTypeEnum.Income)
                                                .Sum(x => x.MoneyTransaction!.Amount),
                                    Expense = p
                                                .Where(x => x.MoneyTransaction != null && 
                                                      x.MoneyTransaction.Type == MoneyTransactionTypeEnum.Expense)
                                                .Sum(x => x.MoneyTransaction!.Amount),
                                    Month = new DateTime(p.Key.Year, p.Key.Month, 1),
                                    Name = p.Key.Tag.Name,
                                    Id = p.Key.Tag.Id
                                })
                                .OrderByDescending(p => p.Expense + p.Income)
                                .Take(10)
                                .ToList();
                return lEvents.Select(p => new TagTransactionPerMonthRDTO()
                {
                    ExpenseAmount = (int)p.Expense,
                    IncomeAmount = (int)p.Income,
                    Month = p.Month,
                    Id = p.Id,
                    Name = p.Name
                }).ToList();
            }
        }
    }
}