using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.API.Services.MappingConfigs
{
    public class SampleProfile : Profile
    {
        public SampleProfile()
        {
            /*
            CreateMap<Entities.MonthlyPurchase, MonthlyPurchase>()
                .ForMember(dest => dest.Creator, opt => opt.Ignore())
                .ForMember(dest => dest.Modifier, opt => opt.Ignore());

            CreateMap<MonthlyPurchase, Entities.MonthlyPurchase>()
                .ForMember(dest => dest.Branch, opt => opt.Ignore())
                .ForMember(dest => dest.Vehicle, opt => opt.Ignore())
                .ForMember(dest => dest.Creator, opt => opt.Ignore())
                .ForMember(dest => dest.Modifier, opt => opt.Ignore());

            CreateMap<Entities.DailyPurchase, MonthlyPurchaseDetail>()
                .ForMember(dest => dest.DailyPurchaseId, opt => opt.MapFrom(o => o.Id))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(o => (o.Amount + (o.AdjustmentAmount))))
                .AfterMap((src, dest) =>
                {
                    if (src.DailyPurchaseDetail != null && src.DailyPurchaseDetail.Any() == true)
                    {
                        string destinations =
                            string.Join<string>(", ", src.DailyPurchaseDetail
                                .OrderBy(o => o.Id)
                                .Select(o => o.ShipmentRoute.Destination.Name)
                                .Distinct());

                        dest.Destinations = destinations;

                        string suppliers =
                            string.Join<string>(", ", src.DailyPurchaseDetail
                                .OrderBy(o => o.ShipmentRoute.Supplier.Name)
                                .Select(o => o.ShipmentRoute.Supplier.Name)
                                .Distinct());

                        dest.Suppliers = suppliers;
                    }
                });
            */
        }
    }
}
