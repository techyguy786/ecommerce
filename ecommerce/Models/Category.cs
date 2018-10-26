using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Display(Name = "Category Name")]
        [Required]
        public string CategoryName { get; set; }

        [Display(Name = "Category Type")]
        public int CategoryTypeId { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual CategoryType Type { get; set; }
    }
}