using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdbService.Models.ViewModels
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
