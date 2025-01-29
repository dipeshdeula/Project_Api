namespace Project_Api.Dtos
{
    public class PaymentDto
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
