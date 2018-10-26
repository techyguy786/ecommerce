using System;
using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [Required]
        public string CartId { get; set; }

        public int Count { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}