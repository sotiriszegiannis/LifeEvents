using Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Repository
{
    public class LifeEventRDTO
    {        
        public int? Id { get; set; }        
        public DateTime? From { get; set; }        
        public DateTime? To { get; set; }
        public int DurationInMinutes { get; set; }
        public string Title { get; set; }        
        public string Description { get; set; }        
        public List<TagRDTO> Tags { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Location { get; set; }
        public MoneyTransactionRDTO MoneyTransaction { get; set; }
    }
}
