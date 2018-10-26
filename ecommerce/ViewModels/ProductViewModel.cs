using ecommerce.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ecommerce.ViewModels
{
    public class ProductViewModel
    {
        public bool Scheduled { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        //public DateTime? Date { get; set; }
        [DataType(DataType.Date)]
        public string Date { get; set; }

        public int CategoryId { get; set; }

        public Product Product { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        [Display(Name = "Category Type")]
        public int CategoryTypeId { get; set; }
        public IEnumerable<CategoryType> CategoryTypes { get; set; }
    }
}