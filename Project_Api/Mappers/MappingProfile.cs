using AutoMapper;
using Project_Api.Dtos;
using Project_Api.Models;

namespace Project_Api.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Admin, AdminDto>().ReverseMap();
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Cart, CartDto>().ReverseMap();
            CreateMap<CartItem,CartItemDto>().ReverseMap();
            CreateMap<PaymentDto, Payment>();

        }
    }
}
