using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Api.Dtos;
using Project_Api.Interfaces;

namespace Project_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
            
        }
        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDto paymentDto)
        {
            await _paymentService.ProcessPaymentAsync(paymentDto);
            _logger.LogInformation("Payment processed for customer ID {CustomerId}", paymentDto.OrderId);
            return Ok();
        }

        [HttpGet("success")]
        public IActionResult PaymentSuccess()
        {
            _logger.LogInformation("Payment successful.");
            return Ok("Payment successful.");
        }

        [HttpGet("failure")]
        public IActionResult PaymentFailure()
        {
            _logger.LogError("Payment failed.");
            return BadRequest("Payment failed.");
        }

    }
}
