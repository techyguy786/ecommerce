using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
    public class CategoryType
    {
        public int CategoryTypeId { get; set; }

        [Display(Name = "Category Type Name")]
        [Required]
        public string CategoryTypeName { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}