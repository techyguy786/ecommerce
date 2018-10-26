using System;
using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
    public class FileDetail
    {
        public Guid Id { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string Extension { get; set; }

        public int ProductId { get; set; }


        public virtual Product Product { get; set; }
    }
}