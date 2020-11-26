using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UdbService.Data;
using UdbService.Extension;
using UdbService.Models;
using UdbService.Models.ViewModels;
using UdbService.Utility;

namespace UdbService.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public CartVM CCartVM { get; set; }

        public CartController(ApplicationDbContext context)
        {
            _context = context;
            CCartVM = new CartVM()
            {
                //OrderHeader = new Models.OrderHeader(),
                ServiceList = new List<Service>()
            };
        }
        


        public IActionResult Index()
        {
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                foreach (int serviceId in sessionList)
                {
                    CCartVM.ServiceList.Add(_context.Service
                        .Include(s => s.Category).
                        FirstOrDefault(u => u.Id == serviceId)
                        );
                }
            }
            return View(CCartVM);
        }
    }
}
