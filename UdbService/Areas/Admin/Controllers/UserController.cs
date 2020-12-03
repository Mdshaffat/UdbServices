using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdbService.Data;
using UdbService.Models;

namespace UdbService.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            var AllUser = await _context.ApplicationUser.ToListAsync();
            return View(AllUser);
        }

        public IActionResult Lock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userFromDb = _context.ApplicationUser.FirstOrDefault(u => u.Id == id);
            userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult UnLock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userFromDb = _context.ApplicationUser.FirstOrDefault(u => u.Id == id);
            userFromDb.LockoutEnd = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
