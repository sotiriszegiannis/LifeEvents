using Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace WebApp
{
    public class TagTransactionPerMonthRDTO
    {
        public float IncomeAmount { get; set; }
        public float ExpenseAmount { get; set; }
        public DateTime Month { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
