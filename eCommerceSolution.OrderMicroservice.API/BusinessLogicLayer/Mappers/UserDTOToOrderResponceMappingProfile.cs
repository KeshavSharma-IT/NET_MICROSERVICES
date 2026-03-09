using AutoMapper;
using eCommerce.OrderMicroservice.BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.Mappers
{
    public class UserDTOToOrderResponceMappingProfile  :Profile
    {
        public UserDTOToOrderResponceMappingProfile()
        {
            CreateMap<UserDTO, OrderResponse>()
                .ForMember(dest => dest.UserPersonName, opt => opt.MapFrom(src => src.PersonName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
