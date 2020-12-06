using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index()
        {
            var Order = from o in _context.Order select o;
            var allOrder = await Order.ToListAsync();
           
            return View(allOrder);
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

        public IActionResult Approve(int? id)
        {
            var orderFromDb = _context.Order.FirstOrDefault(o => o.Id == id);
            if (orderFromDb == null)
            {
                return NotFound();
            }
            orderFromDb.Status = SD.StatusApproved;
            
            _context.SaveChanges();
            return RedirectToAction("Index", "Order");
        }

        public IActionResult Reject(int? id)
        {
            var orderFromDb = _context.Order.FirstOrDefault(o => o.Id == id);
            if (orderFromDb == null)
            {
                return NotFound();
            }
            orderFromDb.Status = SD.StatusRejected;
            _context.SaveChanges();
            return RedirectToAction("Index", "Order");
        }

        public IActionResult PendingIndex()
        {
            var AllOrder = from o in _context.Order select o;
            var AllpendingOrder = AllOrder.Where(o => o.Status == SD.StatusSubmitted);

            return View(AllpendingOrder);
        }
        public IActionResult ApprovedIndex()
        {
            var AllOrder = from o in _context.Order select o;
            var AllApprovedOrder = AllOrder.Where(o => o.Status == SD.StatusApproved);

            return View(AllApprovedOrder);
        }
        public IActionResult RejectIndex()
        {
            var AllOrder = from o in _context.Order select o;
            var AllApprovedOrder = AllOrder.Where(o => o.Status == SD.StatusRejected);

            return View(AllApprovedOrder);
        }
    }
}
