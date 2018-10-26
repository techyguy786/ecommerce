using System;
using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
    public class WishList
    {
        [Key]
        public int WishListId { get; set; }

        [Required]
        public string wishId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}