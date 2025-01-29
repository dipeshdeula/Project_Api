using Project_Api.Models;

namespace Project_Api.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddPaymentAsync(Payment payment);
    }
}
