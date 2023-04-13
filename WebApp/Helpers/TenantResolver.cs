using Domain;
using System.Diagnostics;

namespace WebApp
{
    public class TenantResolver : ITenantResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            Debug.WriteLine("*********************************TenantResolver instantiated through DI*****************");
        }

        public string GetCurrentTenantId()
        {
            var tenantId = _httpContextAccessor?.HttpContext.User.Identity.Name;
            //if (claim is null)
            //    throw new UnauthorizedAccessException("Authentication failed");

            //var tenant = _tenantRegistry.GetTenants().FirstOrDefault(t => t.Name == claim.Value);
            //if (tenant is null)
            //    throw new UnauthorizedAccessException($"Tenant '{claim.Value}' is not registered.");

            return tenantId!;
        }
    }
}
