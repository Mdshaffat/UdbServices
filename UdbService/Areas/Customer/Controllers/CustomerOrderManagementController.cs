using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdbService.Data;
using UdbService.Models;
using UdbService.Models.ViewModels;
using UdbService.Utility;

namespace UdbService.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CustomerOrderManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;

        public CustomerOrderManagementController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }
        public async Task<IActionResult> Index()
        {
            var info = await GetCurrentUser();

            var Order = from o in _context.Order select o;
            var userOrder = await Order.Where(o=>o.UserId == info.Id).ToListAsync();

            return View(userOrder);
        }


        public async Task<IActionResult> Details(int id)
        {
            var info = await GetCurrentUser();
            //var CurrentOrder = _context.Order.Where(o => o.UserId == info.Id).FirstOrDefault(o => o.Id == id);
            //if(info.Id != CurrentOrder.UserId ) {
            //    ViewData["NotFound"] = "Sorry! Order Not Found!!!" ;
            //}
            OrderViewModel orderVM = new OrderViewModel()
            {
                
                Order = _context.Order.Where(o => o.UserId == info.Id).FirstOrDefault(o => o.Id == id),
                OrderDetails = _context.OrderDetails.Where(o => o.OrderId == id).ToList()


            };
            if(orderVM.Order == null)
            {
                return RedirectToAction("Index", "CustomerOrderManagement");
            }
            else
            {
                return View(orderVM);
            }
            
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
            return RedirectToAction("Index", "CustomerOrderManagement");
        }
        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await _usermanager.GetUserAsync(HttpContext.User);
        }


    }
}
