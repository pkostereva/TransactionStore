using AutoMapper;
using System;
using TransactionStore.API.Models.InputModels;
using TransactionStore.API.Models.OutputModels;
using TransactionStore.DB.Models;

namespace TransactionStore.API.Configuration
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            CreateMap<AccountInputModel, Account>();              
            CreateMap<TransactionEntityInputModel, TransactionEntity>()
                .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(src => src.TimeStamp == "" ? null : (DateTime?)Convert.ToDateTime(src.TimeStamp)));
            CreateMap<TransactionEntity, TransactionEntityOutputModel>()
               .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(src => src.TimeStamp.ToString(@"dd.MM.yyyy")));
            CreateMap<AccountBalance, AccountBalanceOutputModel>()
               .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(src => src.TimeStamp.HasValue ? src.TimeStamp.Value.ToString("yyyy.MM.dd HH:mm:ss.ffffff") : string.Empty));
            CreateMap<TransferInputModel, TransferEntities>()
                .ForPath(dest => dest.Recipient.Account, opt => opt.MapFrom(src => src.RecipientAccount))
                .ForPath(dest => dest.Sender.Account, opt => opt.MapFrom(src => src.SenderAccount))
                .ForPath(dest => dest.Recipient.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForPath(dest => dest.Sender.Amount, opt => opt.MapFrom(src => -src.Amount))
                .ForPath(dest => dest.Recipient.TimeStamp, opt => opt.MapFrom(src => src.RecipientTimeStamp == "" ? null : (DateTime?)Convert.ToDateTime(src.RecipientTimeStamp)))
                .ForPath(dest => dest.Sender.TimeStamp, opt => opt.MapFrom(src => src.SenderTimeStamp == "" ? null : (DateTime?)Convert.ToDateTime(src.SenderTimeStamp)));
        }
    }
}
