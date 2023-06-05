using Domain;

namespace WebApp
{
    public class MoneyTransactionDTO
    {
        public float Amount { get; set; }
        public MoneyTransactionTypeEnum Type { get; set; }
    }
}
