using Domain;
using Microsoft.EntityFrameworkCore;
using Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
{
    public class UsersRepository : BaseRepository<User>
    {
        public UsersRepository(IDbContextFactory<AppDbContext> dbContextFactory, ITenantResolver tenantResolver) : base(dbContextFactory, tenantResolver) { }
        public async Task<int> Update(UserRDTO userRDTO)
        {
            return await base.Update(userRDTO.Id, p =>
            {
                p.TimeZone = userRDTO.TimeZone;
            });
        }
        public async Task<UserRDTO> Get()
        {
            var tenantId = TenantResolver.GetCurrentTenantId();
            var user = await base.Get(p => p.TenantId == tenantId);
            if (user != null)
                return new UserRDTO()
                {
                    Id = user.Id,
                    TimeZone = user.TimeZone,
                    Name = user.Name,
                };
            else
                return null;
        }
    }
}
