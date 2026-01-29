using Domain;

namespace Repository
{
    public class MoneyTransactionPerMonthRDTO
    {
        public float Amount { get; set; }
        public MoneyTransactionTypeEnum Type { get; set; }
        public DateTime Month { get; set; }
    }
}