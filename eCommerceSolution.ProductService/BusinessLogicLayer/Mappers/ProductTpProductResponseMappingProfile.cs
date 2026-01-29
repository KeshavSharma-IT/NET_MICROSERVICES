using AutoMapper;
using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.BusinessLogicLayer.Mappers
{
    internal class ProductTpProductResponseMappingProfile :Profile
    {
        public ProductTpProductResponseMappingProfile()
        {
            //source type- product and destination type is ProductResponce
            CreateMap<Product, ProductResponce>()
         .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.QuantityInStock));
        }
    }
}
