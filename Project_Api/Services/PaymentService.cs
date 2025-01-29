using AutoMapper;
using Project_Api.Dtos;
using Project_Api.Interfaces;
using Project_Api.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project_Api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper, ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task ProcessPaymentAsync(PaymentDto paymentDto)
        {
            // Map to Payment model
            var payment = _mapper.Map<Payment>(paymentDto);
            payment.PaymentDate = DateTime.Now;

            // Call eSewa API
            var esewaResponse = await CallEsewaApi(paymentDto);
            if (esewaResponse.IsSuccessStatusCode)
            {
                // Save payment details to database
                await _paymentRepository.AddPaymentAsync(payment);
                _logger.LogInformation("Payment processed for Order ID {OrderID}", paymentDto.OrderId);
            }
            else
            {
                var responseContent = await esewaResponse.Content.ReadAsStringAsync();
                _logger.LogError("Error processing payment for Order ID {OrderId}. Response: {Response}", paymentDto.OrderId, responseContent);
                throw new Exception($"Error processing payment. Response: {responseContent}");
            }
        }

        private async Task<HttpResponseMessage> CallEsewaApi(PaymentDto paymentDto)
        {
            var client = new HttpClient();
            var esewaRequest = new
            {
                amt = paymentDto.Amount,
                psc = 0,
                pdc = 0,
                txAmt = 0,
                tAmt = paymentDto.Amount,
                pid = paymentDto.TransactionId,
                scd = "EPAYTEST",
                su = "https://localhost:7078/api/payment/success",
                fu = "https://localhost:7078/api/payment/failure"
            };

            var content = new StringContent(JsonSerializer.Serialize(esewaRequest), Encoding.UTF8, "application/json");
            _logger.LogInformation("Sending request to eSewa: {Request}", JsonSerializer.Serialize(esewaRequest));
            var response = await client.PostAsync("https://esewa.com.np/epay/main", content);
            _logger.LogInformation("Received response from eSewa: {Response}", await response.Content.ReadAsStringAsync());
            return response;
        }
    }
}
