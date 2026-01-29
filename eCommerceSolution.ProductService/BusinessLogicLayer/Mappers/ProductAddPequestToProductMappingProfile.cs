using AutoMapper;
using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.DataAccessLayer.Entities;


namespace eCommerce.BusinessLogicLayer.Mappers
{
    public class ProductAddPequestToProductMappingProfile :Profile
    {
        public ProductAddPequestToProductMappingProfile()
        {
            CreateMap<ProductAddRequest, Product>()
                .ForMember(dest => dest.ProductID, opt => opt.Ignore())
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(product => product.ProductName))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(product => product.Category))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(product => product.UnitPrice))
                .ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(product => product.QuantityInStock));
        }
    }
}
