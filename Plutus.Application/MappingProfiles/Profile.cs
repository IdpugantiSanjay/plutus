using Plutus.Application.Categories.ViewModels;
using Plutus.Application.Transactions.Commands;
using Plutus.Application.Transactions.ViewModels.cs;
using Plutus.Domain;

namespace Plutus.Application.MappingProfiles
{
    public class Profile: AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<Transaction, CreateTransaction.Response>();
            CreateMap<Transaction, TransactionViewModel>().ForMember(dest => dest.DateTime, exp => exp.MapFrom(src =>  src.DateTime.Value));
            CreateMap<Category, CategoryViewModel>();
        }
    }
}