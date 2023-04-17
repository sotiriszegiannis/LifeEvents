using Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
{    
    public class UserRDTO
    {        
        public int Id { get; set; }     
        public string Name { get; set; }
        public string TimeZone { get; set; }
    }
}