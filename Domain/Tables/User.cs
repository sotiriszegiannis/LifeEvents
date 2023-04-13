using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    [Table("Users")]
    public class User : Tenant,ITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [TableColumnAttr]
        public int Id { get; set; }
        [TableColumnAttr]
        public string Name { get; set; }        

    }
}