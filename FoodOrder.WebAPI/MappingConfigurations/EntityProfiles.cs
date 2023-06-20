using AutoMapper;
using FoodOrder.Persistance;
using FoodOrder.DTO;
using System.Collections.Generic;

namespace FoodOrder.WebAPI.MappingConfigurations
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Orders, OrderDto>();
        }
    }

    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            CreateMap<Products, ProductDto>();
        }
    }

    public class OrderDtoProfile : Profile
    {
        public OrderDtoProfile()
        {
            CreateMap<OrderDto, Orders>();
        }
    }

    public class ProductDtoProfile : Profile
    {
        public ProductDtoProfile()
        {
            CreateMap<ProductDto, Products>();
        }
    }
}
