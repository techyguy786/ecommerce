using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
    public class Contact
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Phone]
        [Required]
        [Display(Name = "Mobile")]
        public string Phone1 { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required]
        [Display(Name = "Telephone")]
        public string Phone2 { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }
    }
}