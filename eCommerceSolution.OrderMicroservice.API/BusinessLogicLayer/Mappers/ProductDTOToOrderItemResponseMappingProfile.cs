using AutoMapper;
using eCommerce.OrderMicroservice.BusinessLogicLayer.DTO;
using eCommerce.OrderMicroservice.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.Mappers
{
    public class ProductDTOToOrderItemResponseMappingProfile   :Profile
    {
        public ProductDTOToOrderItemResponseMappingProfile()
        {
            CreateMap<ProductDTO, OrderItemResponse>()
             .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
             .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
        }
    }
}
