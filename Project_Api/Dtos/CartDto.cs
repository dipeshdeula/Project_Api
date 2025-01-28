namespace Project_Api.Dtos
{
    public class CartDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }
}
