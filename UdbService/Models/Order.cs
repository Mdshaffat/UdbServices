using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UdbService.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        [StringLength(50, MinimumLength =3, ErrorMessage ="String Length Must be 3-50 charecter")]
        [Display(Name="Name")]
        public string Name { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(300, MinimumLength = 10, ErrorMessage = "String Length Must be 10-300 charecter")]
        [Display(Name = "City")]
        public string City { get; set; }
        [Required]
        [StringLength(300, MinimumLength = 10, ErrorMessage = "String Length Must be 10-300 charecter")]
        [Display(Name = "Address")]
        public string Address { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public int ServiceCount { get; set; }


    }
}
