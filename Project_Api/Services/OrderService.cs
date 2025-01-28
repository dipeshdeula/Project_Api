using AutoMapper;
using Project_Api.Dtos;
using Project_Api.Interfaces;
using Project_Api.Models;

namespace Project_Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;

        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
           var orders = await _orderRepository.GetAllOrdersAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
           var order = await _orderRepository.GetOrderByIdAsync(id);
            return _mapper.Map<OrderDto>(order);
        }
        public async Task AddOrderAsync(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            await _orderRepository.AddOrderAsync(order);
        }
        public async Task UpdateOrderAsync(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            await _orderRepository.UpdateOrderAsync(order);
        }
        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.DeleteOrderAsync(id);
        }

        public async Task PlaceOrderAsync(int customerId)
        { 
            var cart = await _cartRepository.GetCartByCustomerIdAsync(customerId);
            if(cart == null || !cart.CartItems.Any())
            {
                throw new Exception("Cart is empty");
            }

            var order = new Order
            {
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow,
                ShippingAddress = "Default Address",
                TotalAmount = cart.CartItems.Sum(item => item.Price * item.Quantity),
                OrderItems = cart.CartItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
                
            };

            await _orderRepository.AddOrderAsync(order);

            //clear the cart after placing the order
            cart.CartItems.Clear();
            await _cartRepository.UpdateCartAsync(cart);
        }

    }
}
