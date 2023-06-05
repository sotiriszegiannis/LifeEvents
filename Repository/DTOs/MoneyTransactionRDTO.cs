using Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
{    
    public class MoneyTransactionRDTO
    {        
        public int Id { get; set; }
        public float Amount { get; set; }
        public MoneyTransactionTypeEnum Type { get; set; }
    }
}