using AutoMapper;
using ButlerLee.API.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.API.Services
{
    public class PagedListProfile : Profile
    {
        public PagedListProfile()
        {
            CreateMap(typeof(PagedList<>), typeof(PagedList<>)).ConvertUsing(typeof(PagedListConverter<,>));
        }
    }

    public class PagedListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
    {
        public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
        {
            var collection = context.Mapper.Map<IEnumerable<TDestination>>(source);

            return new PagedList<TDestination>(collection, source.TotalCount, source.CurrentPage, source.PageSize);
        }
    }
}
