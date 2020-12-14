using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdbService.Data;
using UdbService.Models;

namespace UdbService.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ChartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChartController(ApplicationDbContext context)
        {
            _context = context;
        }
        //public async Task<List<OrderDetails>> Index()
        public async Task<IActionResult> Index()
        {
            var Data = _context.OrderDetails;
            //var OrderList = from service in Data
            //                group service by service.ServiceName into newGroup
            //                orderby newGroup.Count()
            //                select newGroup;
            //var query = Data.GroupBy(service => service.ServiceName).OrderByDescending(group => group.Count()).Take(5).ToListAsync();
            //Series cities = seriesList.Single(); cities.Name = "Cities"; foreach (var record in query) { cities.Points.AddXY(record.Key, record.Count()); } }
            return View(await Data.ToListAsync());
        }

    }

}
