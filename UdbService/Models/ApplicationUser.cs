using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UdbService.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Please Enter Your Full Name")]
        [Display(Name = "Full Name")]
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }
        [Required(ErrorMessage ="Please Enter Your City")]
        [Display(Name ="City")]
        [StringLength(30, MinimumLength =3)]
        public string City { get; set; }
        [Required(ErrorMessage = "Please Enter Your Full Address ' between 15 - 300 Charecter'")]
        [Display(Name = "Service Name")]
        [StringLength(300, MinimumLength = 15)]
        public string Address { get; set; }
    }
}
