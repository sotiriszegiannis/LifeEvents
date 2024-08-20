using AutoMapper;
using Domain;
namespace Repository
{
    public class MoneyTransactionProfile : Profile
    {
        public MoneyTransactionProfile()
        {
            CreateMap<MoneyTransaction, MoneyTransactionRDTO>().ReverseMap();
        }
    }
}
