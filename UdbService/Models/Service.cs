using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UdbService.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Service Name")]
        [Display(Name = "Service Name")]
        [StringLength(60, MinimumLength = 3)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Service Price")]
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name ="Image")]
        public string ImageName { get; set; }
        [NotMapped]
        [DisplayName("Upload Image")]
        public IFormFile ImageFile { get; set; }
        [StringLength(500)]
        public string Details { get; set; }
        [Required(ErrorMessage = "Please Chose a Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
