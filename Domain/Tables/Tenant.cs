using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{    
    public class Tenant
    {    
        [TableColumnAttr]
        public string TenantId { get; set; }
    }
}