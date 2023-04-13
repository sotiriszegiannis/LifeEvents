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
                lifeEvent.From = lifeEventDTO.From==default(DateTime)?DateTime.UtcNow.AddMinutes(-lifeEventDTO.DurationInMinutes):lifeEventDTO.From;
                lifeEvent.To = lifeEventDTO.To==default(DateTime)?lifeEvent.From.AddMinutes(lifeEventDTO.DurationInMinutes):lifeEventDTO.From;
                lifeEvent.Name = lifeEventDTO.Name;
                lifeEvent.Description = lifeEventDTO.Description;
                lifeEvent.Location = lifeEventDTO.Location;
                lifeEvent.DateCreated = DateTime.UtcNow;
                lifeEvent.DateUpdated = DateTime.UtcNow;                
                //lifeEvent.Tags = lifeEventDTO.Tags;
                return await base.Save(lifeEvent);
            }
            return 0;
        }
        public override void Map(LifeEvent fromEntity, LifeEvent toEntity)
        {
            toEntity.User = (GetDbContext().Result as AppDbContext).Users.FirstOrDefault(x => x.TenantId == TenantResolver.GetCurrentTenantId());
            base.Map(fromEntity, toEntity);
        }
    }
}
