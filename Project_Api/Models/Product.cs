using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Project_Api.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }


        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string? ProductImage { get; set; }


    }
}
