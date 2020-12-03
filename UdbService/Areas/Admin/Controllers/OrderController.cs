using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdbService.Data;
using UdbService.Models.ViewModels;
using UdbService.Utility;

namespace UdbService.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller

    { 
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            var AllOrder = _context.Order.ToList();
           
            return View(AllOrder);
        }


        public IActionResult Details(int id)
        {
            OrderViewModel orderVM = new OrderViewModel()
            {
                Order = _context.Order.Find(id),
                OrderDetails = _context.OrderDetails.Where(o=>o.Id == id).ToList()

            };
            return View(orderVM);
        }

        public IActionResult Approve(int id)
        {
            var orderFromDb = _context.Order.FirstOrDefault(o => o.Id == id);
            if (orderFromDb == null)
            {
                return NotFound();
            }
            orderFromDb.Status = SD.StatusApproved;
            _context.SaveChanges();
            return View(nameof(Index));
        }

        public IActionResult Reject(int id, string status)
        {
            var orderFromDb = _context.Order.FirstOrDefault(o => o.Id == id);
            if (orderFromDb == null)
            {
                return NotFound();
            }
            orderFromDb.Status = SD.StatusRejected;
            _context.SaveChanges();
            return View(nameof(Index));
        }
       


    }
}
