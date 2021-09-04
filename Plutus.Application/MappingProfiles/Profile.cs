using AutoMapper;
using Plutus.Application.Categories.ViewModels;
using Plutus.Application.Transactions.Commands;
//using Plutus.Application.Transactions.Indexes;
using Plutus.Application.Transactions.ViewModels.cs;
using Plutus.Domain;
using Plutus.Domain.Enums;

namespace Plutus.Application.MappingProfiles
{
    public class Profile: AutoMapper.Profile
    {

        private class TransactionTypeConverter : IValueConverter<bool, TransactionType>
        {
            public TransactionType Convert(bool sourceMember, ResolutionContext context)
            {
                return sourceMember switch
                {
                    true => TransactionType.Income,
                    _ => TransactionType.Expense
                };
            }
        }

        public Profile()
        {
            CreateMap<Transaction, CreateTransaction.Response>();
            CreateMap<Transaction, UpdateTransaction.Response>();
            CreateMap<Transaction, TransactionViewModel>().ForMember(dest => dest.DateTime, exp => exp.MapFrom(src =>  src.DateTime.Value));
            CreateMap<Category, CategoryViewModel>();

            //CreateMap<TransactionIndex.Index, TransactionViewModel>()
            //        //.ForPath(dest => dest.Category.Name, exp => exp.MapFrom(src => src.Category))
            //        .ForMember(dest => dest.TransactionType, exp => exp.ConvertUsing(new TransactionTypeConverter(), src => src.IsCredit));
            //        ;
        }
    }
}