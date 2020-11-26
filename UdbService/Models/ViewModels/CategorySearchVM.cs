using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdbService.Models.ViewModels
{
    public class CategorySearchVM
    {
        public List<Category> category { get; set; }
        public List<Service> service { get; set; }
        public SelectList CategoryList { get; set; }
        public int CategoryName { get; set; }
        public string SearchString { get; set; }
    }
}
