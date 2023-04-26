using Domain;
using Microsoft.EntityFrameworkCore;
using Repository;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace Repository
{
    public class LifeEventsRepository : BaseRepository<LifeEvent>
    {
        public LifeEventsRepository(IDbContextFactory<AppDbContext> dbContextFactory, ITenantResolver tenantResolver) : base(dbContextFactory, tenantResolver) { }
        [Obsolete("Cannot save directly a LifeEvent entity", true)]
        public override Task<int> Save(LifeEvent entity)
        {
            return base.Save(entity);
        }
        public async Task<int> Save(LifeEventRDTO lifeEventDTO)
        {
            LifeEvent lifeEvent = new LifeEvent();
            if (lifeEvent.Id == 0)
            {
                lifeEvent = new LifeEvent();
                lifeEvent.From = !lifeEventDTO.From.HasValue ? DateTime.UtcNow.AddMinutes(-lifeEventDTO.DurationInMinutes) : lifeEventDTO.From.Value;
                lifeEvent.To = !lifeEventDTO.To.HasValue ? lifeEvent.From.AddMinutes(lifeEventDTO.DurationInMinutes) : lifeEventDTO.To.Value;
                lifeEvent.Name = lifeEventDTO.Name;
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
        public override void Map(LifeEvent fromEntity, LifeEvent toEntity, DbContext dbContext)
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
