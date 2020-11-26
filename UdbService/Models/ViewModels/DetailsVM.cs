using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdbService.Models.ViewModels
{
    public class DetailsVM
    {
        public Category IndCategory { get; set; }
        public Service IndService { get; set; }
        public IEnumerable<Service> ServiceList { get; set; }

    }
}
