using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Api.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }

    }
}
