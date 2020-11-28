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
        [StringLength(25, MinimumLength = 3, ErrorMessage = "String Length Must be 3-25")]
        public string Name { get; set; }
        
        [Display(Name ="City")]
        [StringLength(30, MinimumLength =3)]
        public string City { get; set; }

        [StringLength(300, MinimumLength = 15, ErrorMessage ="String Length Must be 15-300")]
        public string Address { get; set; }

    }
}
