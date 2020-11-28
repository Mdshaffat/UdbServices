﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdbService.Models.ViewModels
{
    public class CartVM
    {
        public IList<Service> ServiceList { get; set; }
        public Order Order { get; set; }
        public ApplicationUser User { get; set; }
    }
}
