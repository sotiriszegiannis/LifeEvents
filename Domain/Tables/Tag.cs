using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{    
    public class Tag:Tenant,ITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [TableColumnAttr]
        public int Id { get; set; }        
        [TableColumnAttr]
        public string Name { get; set; }        
        public List<LifeEvent> LifeEvents { get; set; }        
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        [TableColumnAttr]
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    }
}