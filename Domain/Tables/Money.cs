using Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{    
    public class MoneyTransaction:Tenant,ITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [TableColumnAttr]
        public int Id { get; set; }        
        [TableColumnAttr]
        public float Amount { get; set; }
        [TableColumnAttr]
        public MoneyTransactionTypeEnum Type { get; set; }
        public int LifeEventId { get; set; }
        public LifeEvent LifeEvent { get; set; }
    }
}