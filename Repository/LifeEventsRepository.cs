using Domain;
using Microsoft.EntityFrameworkCore;
using Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
{
    public class LifeEventsRepository : BaseRepository<LifeEvent>
    {
        public LifeEventsRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
