using Domain;
using Microsoft.EntityFrameworkCore;
using Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
{
    public class UsersRepository : BaseRepository<User>
    {
        public UsersRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
