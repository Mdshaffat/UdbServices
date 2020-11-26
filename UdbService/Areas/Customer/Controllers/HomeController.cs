using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UdbService.Data;
using UdbService.Extension;
using UdbService.Models;
using UdbService.Models.ViewModels;
using UdbService.Utility;

namespace UdbService.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private HomeVM homeVM;
        private DetailsVM detailsVM;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}


        //get Home View
        public async Task<IActionResult> Index(int id, string searchString)
        {
            var category = from c in _context.Category select c;
            var service = from s in _context.Service select s;
            //var services = await _context.Service
            //    .ToListAsync();
            //foreach (var s in services.Where(i=>i.Id == i.Id)) {
            //    ViewBag.ExtraPrice = (int)s.Price + (int)s.Price * 0.4;
            //}

            if (id > 0)
            {
                service  = _context.Service.Where(c => c.CategoryId == id);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                service = service.Where(s => s.Name.Contains(searchString));
            }


            homeVM = new HomeVM()
            {
                //SerByCat = await serbycat.ToListAsync(),
                CategoryList = await category.ToListAsync(),
                ServiceList = await service.ToListAsync(),
                

            };
            return View(homeVM);
        }

        //get Item Details


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var services = from s in _context.Service select s;

            var service = await _context.Service
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            var execptSer = _context.Service.Where(e => e.Id == id);
            var servicelist =await _context.Service.Where(c => c.CategoryId == service.CategoryId).ToListAsync();

            ViewBag.ExtraPrice = (int)service.Price + (int)service.Price * 0.4 ;
            detailsVM = new DetailsVM()
            {
                IndService = service,
                ServiceList = servicelist.Except(execptSer)

            };
            return View(detailsVM);
        }


        public IActionResult AddToCart(int serviceId)
        {
            List<int> sessionList = new List<int>();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SD.SessionCart)))
            {
                sessionList.Add(serviceId);
                HttpContext.Session.SetObject(SD.SessionCart, sessionList);
            }
            else
            {
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                if (!sessionList.Contains(serviceId))
                {
                    sessionList.Add(serviceId);
                    HttpContext.Session.SetObject(SD.SessionCart, sessionList);
                }
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
