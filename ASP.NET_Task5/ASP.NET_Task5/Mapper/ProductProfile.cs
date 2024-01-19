using ASP.NET_Task5.Models;
using ASP.NET_Task5.Models.ViewModels;
using AutoMapper;

namespace ASP.NET_Task5.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductViewModel, Product>()
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore()) 
            .ForMember(dest => dest.Tags, opt => opt.Ignore()) 
            .ReverseMap();

            CreateMap<EditProductViewModel, Product>()
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
            .ForMember(dest => dest.Tags, opt => opt.Ignore())
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => (IFormFile)null)) 
            .ReverseMap();

            CreateMap<string, IFormFile>().ConvertUsing(new NullConverter<string, IFormFile>());
        }
    }

    public class NullConverter<TSource, TDestination> : ITypeConverter<TSource, TDestination>
    where TSource : class
    where TDestination : class
    {
        public TDestination Convert(TSource source, TDestination destination, ResolutionContext context)
        {
            return null;
        }
    }
}
