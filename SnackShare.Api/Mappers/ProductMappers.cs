using AutoMapper;
using SnackShare.Api.Data.Entities.Products;
using SnackShare.Api.Models.Products;

namespace SnackShare.Api.Mappers
{
    internal static class ProductMappers
    {
        internal static IMapper Mapper { get; }

        static ProductMappers()
        {
            Mapper = new MapperConfiguration(c => c.AddProfile<ProductMapProfile>()).CreateMapper();
        }

        public static ProductModel ToModel(this Product product) => Mapper.Map<ProductModel>(product);
        public static ProductStockModel ToModel(this ProductStock stock) => Mapper.Map<ProductStockModel>(stock);

    }

    public class ProductMapProfile : Profile
    {
        public ProductMapProfile()
        {
            CreateMap<Product, ProductModel>();
            CreateMap<ProductStock, ProductStockModel>();
        }
    }
}
