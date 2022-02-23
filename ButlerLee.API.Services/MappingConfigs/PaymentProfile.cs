using AutoMapper;
using ButlerLee.API.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static AutoMapper.Internal.ExpressionFactory;

namespace ButlerLee.API.Services.MappingConfigs
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Entities.Payment, Models.Payment>().ReverseMap();

            CreateMap<Entities.Payment, KakaoPaymentResponse>()
                .ForMember(dest => dest.Tid, opt => opt.MapFrom(src => src.PaymentKey))
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.OrderName)) 
                .ForMember(dest => dest.PaymentMethodType, opt => opt.MapFrom(src => src.Method))
                .ForPath(dest => dest.Amount.Total, opt => opt.MapFrom(src => src.TotalAmount))
                .ForPath(dest => dest.Amount.TaxFree, opt => opt.MapFrom(src => src.TaxFreeAmount))
                .ForPath(dest => dest.Amount.Discount, opt => opt.MapFrom(src => src.DiscountAmount))
                .ForPath(dest => dest.CardInfo.PurchaseCorp, opt => opt.MapFrom(src => src.CardCompany))
                .ForPath(dest => dest.CardInfo.InstallMonth, opt => opt.MapFrom(src => src.CardInstallmentPlanMonths))
                .ForPath(dest => dest.CardInfo.ApprovedId, opt => opt.MapFrom(src => src.CardApproveNo))
                .ForPath(dest => dest.CardInfo.Bin, opt => opt.MapFrom(src => src.CardNumber))
                .ForPath(dest => dest.CardInfo.CardType, opt => opt.MapFrom(src => src.CardType))
                .ForPath(dest => dest.CanceledAmount.Total, opt => opt.MapFrom(src => src.CanceledAmount))
                .ForMember(dest => dest.CanceledAt, opt => opt.MapFrom(src => src.CancelDate))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.RequestDate))
                .ForMember(dest => dest.ApprovedAt, opt => opt.MapFrom(src => src.ApproveDate))
                .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => src.UpdateDate));


            CreateMap<KakaoPaymentResponse, Entities.Payment>()
               .IncludeMembers(u => u.CardInfo, u => u.Amount, u => u.CanceledAmount)
               .ForMember(dest => dest.PaymentKey, opt => opt.MapFrom(src => src.Tid))
               .ForMember(dest => dest.OrderName, opt => opt.MapFrom(src => src.ItemName))
               .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.PaymentMethodType))
               .ForMember(dest => dest.CancelDate, opt => opt.MapFrom(src => src.CanceledAt))
               .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(src => src.CreatedAt))
               .ForMember(dest => dest.ApproveDate, opt => opt.MapFrom(src => src.ApprovedAt))
               .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateTime))
               .ForMember(dest => dest.CanceledAmount, opt => opt.MapFrom(src => src.CanceledAmount.Total));

            CreateMap<CardInfo, Entities.Payment>()
                .ForMember(dest => dest.CardInstallmentPlanMonths, opt => opt.MapFrom(src => Convert.ToInt32(src.InstallMonth)))
                .ForMember(dest => dest.IsInsterestFree, opt => opt.MapFrom(src => src.InterestFreeInstall.ToUpper() == "Y" ? true : false))
                .ForMember(dest => dest.CardCompany, opt => opt.MapFrom(src => src.PurchaseCorp))
                .ForMember(dest => dest.CardInstallmentPlanMonths, opt => opt.MapFrom(src => src.InstallMonth))
                .ForMember(dest => dest.CardApproveNo, opt => opt.MapFrom(src => src.ApprovedId))
                .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.Bin ))
                .ForMember(dest => dest.CardType, opt => opt.MapFrom(src => src.CardType ))
                .ReverseMap();

            CreateMap<CanceledAmount, Entities.Payment>()
                .ForMember(dest => dest.CanceledAmount, opt => opt.MapFrom(src => src.Total))
                .ReverseMap();

            CreateMap<Amount, Entities.Payment>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Total))
                .ForMember(dest => dest.TaxFreeAmount, opt => opt.MapFrom(src => src.TaxFree))
                .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.Discount))
                .ReverseMap();

            CreateMap<Entities.Payment, TossPaymentResponse>()
                .ForMember(dest => dest.PaymentKey, opt => opt.MapFrom(src => src.PaymentKey))
                .ForMember(dest => dest.OrderName, opt => opt.MapFrom(src => src.OrderName))
                .ForMember(dest => dest.EasyPay, opt => opt.MapFrom(src => src.EasyPay))
                .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.TaxFreeAmount, opt => opt.MapFrom(src => src.TaxFreeAmount))
                .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount))
                .ForPath(dest => dest.Card.Company, opt => opt.MapFrom(src => src.CardCompany))
                .ForPath(dest => dest.Card.Number, opt => opt.MapFrom(src => src.CardNumber))
                .ForPath(dest => dest.Card.InstallmentPlanMonths, opt => opt.MapFrom(src => src.CardInstallmentPlanMonths))
                .ForPath(dest => dest.Card.IsInterestFree, opt => opt.MapFrom(src => src.IsInsterestFree))
                .ForPath(dest => dest.Card.ApproveNo, opt => opt.MapFrom(src => src.CardApproveNo))
                .ForPath(dest => dest.Card.CardType, opt => opt.MapFrom(src => src.CardType))
                .ForPath(dest => dest.Card.OwnerType, opt => opt.MapFrom(src => src.CardOwnerType))
                .ForPath(dest => dest.Card.ReceiptUrl, opt => opt.MapFrom(src => src.CardReceiptUrl))
                .ForPath(dest => dest.Card.AcquireStatus, opt => opt.MapFrom(src => src.CardAcquireStatus))
                .ForPath(dest => dest.CancelData.CancelAmount, opt => opt.MapFrom(src => src.CanceledAmount))
                .ForPath(dest => dest.CancelData.CanceledAt, opt => opt.MapFrom(src => src.CancelDate))
                .ForMember(dest => dest.RequestedAt, opt => opt.MapFrom(src => src.RequestDate))
                .ForMember(dest => dest.ApprovedAt, opt => opt.MapFrom(src => src.ApproveDate))
                .ReverseMap();


            #region Models.Payment
            CreateMap<Models.Payment, KakaoPaymentResponse>()
              .ForMember(dest => dest.Tid, opt => opt.MapFrom(src => src.PaymentKey))
              .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.OrderName))
              .ForMember(dest => dest.PaymentMethodType, opt => opt.MapFrom(src => src.Method))
              .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
              .ForPath(dest => dest.Amount.Total, opt => opt.MapFrom(src => src.TotalAmount))
              .ForPath(dest => dest.Amount.TaxFree, opt => opt.MapFrom(src => src.TaxFreeAmount))
              .ForPath(dest => dest.Amount.Discount, opt => opt.MapFrom(src => src.DiscountAmount))
              .ForPath(dest => dest.CardInfo.PurchaseCorp, opt => opt.MapFrom(src => src.CardCompany))
              .ForPath(dest => dest.CardInfo.InstallMonth, opt => opt.MapFrom(src => src.CardInstallmentPlanMonths))
              .ForPath(dest => dest.CardInfo.ApprovedId, opt => opt.MapFrom(src => src.CardApproveNo))
              .ForPath(dest => dest.CardInfo.Bin, opt => opt.MapFrom(src => src.CardNumber))
              .ForPath(dest => dest.CardInfo.CardType, opt => opt.MapFrom(src => src.CardType))
              .ForPath(dest => dest.CanceledAmount.Total, opt => opt.MapFrom(src => src.CanceledAmount))
              .ForMember(dest => dest.CanceledAt, opt => opt.MapFrom(src => src.CancelDate))
              .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.RequestDate))
              .ForMember(dest => dest.ApprovedAt, opt => opt.MapFrom(src => src.ApproveDate))
              .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => src.UpdateDate));

            CreateMap<KakaoPaymentResponse, Models.Payment>()
               .ForMember(dest => dest.PaymentKey, opt => opt.MapFrom(src => src.Tid))
               .ForMember(dest => dest.OrderName, opt => opt.MapFrom(src => src.ItemName))
               .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.PaymentMethodType))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
               .ForMember(dest => dest.CancelDate, opt => opt.MapFrom(src => src.CanceledAt))
               .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(src => src.CreatedAt))
               .ForMember(dest => dest.ApproveDate, opt => opt.MapFrom(src => src.ApprovedAt))
               .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateTime))
               .ForMember(dest => dest.CanceledAmount, opt => opt.MapFrom(src => src.CanceledAmount.Total))
               .IncludeMembers(u => u.CardInfo, u => u.Amount, u => u.CanceledAmount);

            CreateMap<CardInfo, Models.Payment>()
                .ForMember(dest => dest.CardInstallmentPlanMonths, opt => opt.MapFrom(src => Convert.ToInt32(src.InstallMonth)))
                .ForMember(dest => dest.IsInsterestFree, opt => opt.MapFrom(src => src.InterestFreeInstall.ToUpper() == "Y" ? true : false))
                .ForMember(dest => dest.CardCompany, opt => opt.MapFrom(src => src.PurchaseCorp))
                .ForMember(dest => dest.CardInstallmentPlanMonths, opt => opt.MapFrom(src => src.InstallMonth))
                .ForMember(dest => dest.CardApproveNo, opt => opt.MapFrom(src => src.ApprovedId))
                .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.Bin))
                .ForMember(dest => dest.CardType, opt => opt.MapFrom(src => src.CardType))
                .ReverseMap();

            CreateMap<CanceledAmount, Models.Payment>()
                .ForMember(dest => dest.CanceledAmount, opt => opt.MapFrom(src => src.Total))
                .ReverseMap();

            CreateMap<Amount, Models.Payment>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Total))
                 .ForMember(dest => dest.TaxFreeAmount, opt => opt.MapFrom(src => src.TaxFree))
                .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.Discount))
                .ReverseMap();


            CreateMap<Models.Payment, TossPaymentResponse>()
                .ForMember(dest => dest.PaymentKey, opt => opt.MapFrom(src => src.PaymentKey))
                .ForMember(dest => dest.OrderName, opt => opt.MapFrom(src => src.OrderName))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.EasyPay, opt => opt.MapFrom(src => src.EasyPay))
                .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.TaxFreeAmount, opt => opt.MapFrom(src => src.TaxFreeAmount))
                .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount))
                .ForPath(dest => dest.Card.Company, opt => opt.MapFrom(src => src.CardCompany))
                .ForPath(dest => dest.Card.Number, opt => opt.MapFrom(src => src.CardNumber))
                .ForPath(dest => dest.Card.InstallmentPlanMonths, opt => opt.MapFrom(src => src.CardInstallmentPlanMonths))
                .ForPath(dest => dest.Card.IsInterestFree, opt => opt.MapFrom(src => src.IsInsterestFree))
                .ForPath(dest => dest.Card.ApproveNo, opt => opt.MapFrom(src => src.CardApproveNo))
                .ForPath(dest => dest.Card.CardType, opt => opt.MapFrom(src => src.CardType))
                .ForPath(dest => dest.Card.OwnerType, opt => opt.MapFrom(src => src.CardOwnerType))
                .ForPath(dest => dest.Card.ReceiptUrl, opt => opt.MapFrom(src => src.CardReceiptUrl))
                .ForPath(dest => dest.Card.AcquireStatus, opt => opt.MapFrom(src => src.CardAcquireStatus))
                .ForPath(dest => dest.CancelData.CancelAmount, opt => opt.MapFrom(src => src.CanceledAmount))
                .ForPath(dest => dest.CancelData.CanceledAt, opt => opt.MapFrom(src => src.CancelDate))
                .ForMember(dest => dest.RequestedAt, opt => opt.MapFrom(src => src.RequestDate))
                .ForMember(dest => dest.ApprovedAt, opt => opt.MapFrom(src => src.ApproveDate))
                .ReverseMap();
            #endregion
           
        }

        
    }
}
