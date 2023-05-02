using AutoMapper;
using Domain;
using Helper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    public class LifeEventsRepository : BaseRepository<LifeEvent>
    {
        
        IMapper Mapper { get; set; }
        public LifeEventsRepository(IDbContextFactory<AppDbContext> dbContextFactory, ITenantResolver tenantResolver, IMapper mapper) : base(dbContextFactory, tenantResolver) { 
            Mapper = mapper;
            base.RelationsEagerLoading = true;
        }
        protected override IQueryable<LifeEvent> LoadRelations(IQueryable<LifeEvent> dbSet)
        {
            return dbSet
                    .Include(p => p.Tags);
        }
        [Obsolete("Cannot save directly a LifeEvent entity", true)]
        public Task<int> Save(LifeEvent entity)
        {
            return base.Save(entity);
        }
        public async Task<List<LifeEventRDTO>> GetAll()
        {
            return Mapper.Map<List<LifeEvent>, List<LifeEventRDTO>>(await base.GetAll()!);
        }
        public async Task<List<(string title,int id)>> GetAllTitles()
        {
            var result = await base.GetAll(p => new ListItem<int, object>(p.Id, p.Title, null!));
            return result.Select(p => (p.Text, p.Key)).ToList();
                
        }
        public async Task<LifeEventRDTO> Get(int id)
        {
            return Mapper.Map<LifeEvent, LifeEventRDTO>(await base.Get(id));
        }
        public async Task<int> Save(LifeEventRDTO lifeEventDTO)
        {
            LifeEvent lifeEvent = new LifeEvent();
            if (lifeEvent.Id == 0)
            {
                lifeEvent = new LifeEvent();
                lifeEvent.From = !lifeEventDTO.From.HasValue ? DateTime.UtcNow.AddMinutes(-lifeEventDTO.DurationInMinutes) : lifeEventDTO.From.Value;
                lifeEvent.To = !lifeEventDTO.To.HasValue ? lifeEvent.From.AddMinutes(lifeEventDTO.DurationInMinutes) : lifeEventDTO.To.Value;
                lifeEvent.Title = lifeEventDTO.Title;
                lifeEvent.Description = lifeEventDTO.Description;
                lifeEvent.Location = lifeEventDTO.Location;
                lifeEvent.DateCreated = DateTime.UtcNow;
                lifeEvent.DateUpdated = DateTime.UtcNow;
                lifeEvent.Tags = lifeEventDTO.Tags?.Select(p => new Tag()
                {
                    Id = p.Id,
                    Name = p.Name,
                }).ToList();
                return await base.Save(lifeEvent);
            }
            return 0;
        }
        public async Task<bool> Delete(int id)
        {
            return await base.Delete(id);
        }
        protected override void Map(LifeEvent fromEntity, LifeEvent toEntity, DbContext dbContext)
        {
            List<Tag> dbTags = null;
            toEntity.User = (dbContext as AppDbContext)?.Users.FirstOrDefault()!;            
            if (fromEntity.Tags == null)
                fromEntity.Tags = new List<Tag>();
            else
            {
                dbTags = (dbContext as AppDbContext)?.Tags.ToList();
            }            
            toEntity.Tags = dbTags?.Where(p => fromEntity.Tags.Any(x => x.Name == p.Name)).ToList();
            if (toEntity.Tags == null)
                toEntity.Tags = new List<Tag>();
            fromEntity.Tags?.ToList().ForEach(p =>
            {               
                var existingTag = dbTags.FirstOrDefault(x => x.Name == p.Name);
                if (existingTag == null)
                    toEntity.Tags.Add(new Tag()
                    {
                        Name = p.Name,
                        TenantId = TenantResolver.GetCurrentTenantId()
                    });
            });
            toEntity.Tags?.Where(p => fromEntity.Tags?.First(x => x.Name == p.Name) == null).ToList().ForEach(p =>
            {
                toEntity.Tags.Remove(p);
            });
            base.Map(fromEntity, toEntity, dbContext);
        }        
    }
}
