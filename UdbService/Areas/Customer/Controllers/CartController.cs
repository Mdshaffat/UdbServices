using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        //private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public CartVM CCartVM { get; set; }
        public CartVM CartVM { get; set; }

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _userManager = usermanager;
            //_signInManager = signInManager;

            CCartVM = new CartVM()
            {
                Order = new Models.Order(),
                ServiceList = new List<Service>()
                
            };
        }


       // UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>;
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

        public IActionResult Remove(int serviceId)
        {
            List<int> sessionList = new List<int>();
            sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
            sessionList.Remove(serviceId);
            HttpContext.Session.SetObject(SD.SessionCart, sessionList);

            return RedirectToAction(nameof(Index));
        }


        //Order Summary

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Summary()
        {
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                //Order order = new Order()
                //{
                //    Name = user.Name,
                //    Email = user.Email,
                //    Phone = user.PhoneNumber,
                //    City = user.City,
                //    Address = user.Address

                //};
                
                var info =await GetCurrentUser();

                

                CartVM = new CartVM()
                {
                    Order = new Models.Order()
                    {
                        UserId = info.Id,
                        Name = info.Name,
                        Email = info.Email,
                        Phone = info.PhoneNumber,
                        City = info.City,
                        Address = info.Address
                    },
                    ServiceList = new List<Service>(),
                    
                };
                

                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                foreach (int serviceId in sessionList)
                {
                    CartVM.ServiceList.Add(_context.Service.FirstOrDefault(u => u.Id == serviceId));
                }
            }
            return View(CartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                CCartVM.ServiceList = new List<Service>();

                foreach (int serviceId in sessionList)
                {
                    CCartVM.ServiceList.Add(_context.Service.Find(serviceId));
                }
            }

            if (!ModelState.IsValid)
            {
                return View(CCartVM);
            }
            else
            {
                CCartVM.Order.OrderDate = DateTime.Now;
                CCartVM.Order.Status = SD.StatusSubmitted;
                CCartVM.Order.ServiceCount = CCartVM.ServiceList.Count;
                _context.Order.Add(CCartVM.Order);
                _context.SaveChanges();

                foreach (var item in CCartVM.ServiceList)
                {
                    OrderDetails orderDetails = new OrderDetails
                    {
                        ServiceId = item.Id,
                        OrderId = CCartVM.Order.Id,
                        ServiceName = item.Name,
                        Price = item.Price
                    };

                    _context.OrderDetails.Add(orderDetails);

                }
                _context.SaveChanges();
                HttpContext.Session.SetObject(SD.SessionCart, new List<int>());
                return RedirectToAction("OrderConfirmation", "Cart", new { id = CCartVM.Order.Id });
            }
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }

        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        
        //private async Task<ApplicationUser> GetCurrentUserinfo()
        //{

        //    var info = await GetCurrentUser();

        //    var userInfo = from u in _context.ApplicationUser
        //                   where u.Id == info.Id
        //                   select u;

        //    return userInfo.;
            
        //}

    }
}
