using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Api.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
       /* [Required]*/
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public ICollection<CartItem> CartItems { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
