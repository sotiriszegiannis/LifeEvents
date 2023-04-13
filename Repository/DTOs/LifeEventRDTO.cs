using Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Repository
{
    public class LifeEventRDTO
    {        
        public int? Id { get; set; }        
        public DateTime From { get; set; }        
        public DateTime To { get; set; }
        public int DurationInMinutes { get; set; }
        public string Name { get; set; }        
        public string Description { get; set; }        
        public List<TagRDTO> Tags { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
        public string Location { get; set; }
    }
}
