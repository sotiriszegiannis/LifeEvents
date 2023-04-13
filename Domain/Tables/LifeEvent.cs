using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{    
    public class LifeEvent:ITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [TableColumnAttr]
        public int Id { get; set; }
        [TableColumnAttr]
        public DateTime From { get; set; }
        [TableColumnAttr]
        public DateTime To { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int DurationInMinutes { get
            {
                if(From==default || To== default)
                    return 0;
                return (To - From).Minutes;
            }
        }
        [TableColumnAttr]
        public string Name { get; set; }
        [TableColumnAttr]
        public string Description { get; set; }
        public User User { get; set; }
        public List<Tag> Tags { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        [TableColumnAttr]
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    }
}