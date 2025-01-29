using Project_Api.Dtos;

namespace Project_Api.Interfaces
{
    public interface IPaymentService
    {
        Task ProcessPaymentAsync(PaymentDto paymentDto);
    }
}
