 using Domain;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Security.Claims;

namespace WebApp
{
    public class TenantResolver : ITenantResolver
    {
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        private string TenantId;                
        public TenantResolver(AuthenticationStateProvider authenticationStateProvider)
        {
            AuthenticationStateProvider = authenticationStateProvider;
        }
        public string GetCurrentTenantId()
        {
            if(TenantId == null)
            {
                var authState = AuthenticationStateProvider.GetAuthenticationStateAsync().Result;
                var user = authState.User;
                if (user.Identity is not null && user.Identity.IsAuthenticated)
                {                    
                    var claims = user.Claims;
                    TenantId = user.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
                }
            }
            
            return TenantId!;
        }

        public void SetCurrentTenantId(string tenantId)
        {
            TenantId = tenantId;
        }
    }
}
