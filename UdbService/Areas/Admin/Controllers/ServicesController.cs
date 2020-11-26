using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UdbService.Data;
using UdbService.Models;
using UdbService.Models.ViewModels;

namespace UdbService.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
       // private string[] _permittedExtensions = { ".jpeg", ".png", ".jpg", ".webp", ".gif" };


        public ServicesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Admin/Services
        public async Task<IActionResult> Index(string searchString)
            //, int categoryid)
        {
            var services = from s in _context.Service select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                services = services.Where(se => se.Name.Contains(searchString));
            }

            var applicationDbContext = services.Include(s => s.Category);
            //var category = from s in _context.Category select s;
            //IQueryable<int> categoryQuery = from m in _context.Category
            //                                orderby m.Id
            //                                select m.Id;
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            //if (categoryid != null)
            //{
            //    category = category.Where(x => x.Id == categoryid);
            //}




            //var serviceVM = new CategorySearchVM
            //{
            //    CategoryList = new SelectList(await categoryQuery.Distinct().ToListAsync()),
            //    service = await applicationDbContext.ToListAsync()
            //};
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Service
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Admin/Services/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: Admin/Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,ImageFile,Details,CategoryId")] Service service)
        {
            if (ModelState.IsValid)
            {
                //Save image to wwwroot/image

                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(service.ImageFile.FileName);
                string extension = Path.GetExtension(service.ImageFile.FileName);
                //string[] _permittedExtensions = { ".jpeg", ".png", ".jpg", ".webp" };
                service.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/services/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await service.ImageFile.CopyToAsync(fileStream);
                }
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));



            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", service.CategoryId);
            return View(service);
        }

        // GET: Admin/Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Service.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", service.CategoryId);
            return View(service);
        }

        // POST: Admin/Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,ImageFile,Details,CategoryId")] Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    //var files = HttpContext.Request.Form.Files;
                    var serviceFromDb = await _context.Service.FindAsync(id);

                    string fileName = Path.GetFileNameWithoutExtension(service.ImageFile.FileName);
                    string extension = Path.GetExtension(service.ImageFile.FileName);
                    service.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Images/services/", fileName);
                    string imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/services/", serviceFromDb.ImageName);
                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await service.ImageFile.CopyToAsync(fileStream);
                    }

                    // service.ImageName = fileName;
                    _context.Service.Remove(serviceFromDb);
                    _context.Update(service);
                    // _context.Add(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", service.CategoryId);
            return View(service);
        }

        // GET: Admin/Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Service
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Admin/Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Service.FindAsync(id);
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/services/", service.ImageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
            _context.Service.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Service.Any(e => e.Id == id);
        }
    }
}
