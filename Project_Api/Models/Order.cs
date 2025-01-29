using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Api.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

      /*  [Required]*/
        public DateTime OrderDate { get; set; }
       /* [Required]*/
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        /*[Required]*/
        public string ShippingAddress { get; set; }

        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
