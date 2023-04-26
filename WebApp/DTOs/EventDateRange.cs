using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp
{
    public class EventDateRange
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}