using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [Display(Name = "Discount Percentage")]
        public decimal DiscountPercentage { get; set; }

        [Display(Name = "Final Price")]
        public decimal FinalPrice { get; set; }

        public int Quantity { get; set; }

        [Required]
        public string Details { get; set; }

        public DateTime ArrivalTime { get; set; }

        public Gender Gender { get; set; }

        [Required]
        [Display(Name = "Brand")]
        public string BrandName { get; set; }

        public bool Featured { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }


        public virtual Category Category { get; set; }

        public virtual ICollection<FileDetail> FileDetails { get; set; }
    }

    public enum Gender
    {
        Male = 1,
        Female
    }
}