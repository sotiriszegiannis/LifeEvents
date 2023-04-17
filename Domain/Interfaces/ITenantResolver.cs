using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface ITenantResolver
    {
        string GetCurrentTenantId();
        void SetCurrentTenantId(string tenantId);
    }
}
