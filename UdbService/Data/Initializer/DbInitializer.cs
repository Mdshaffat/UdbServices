using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using UdbService.Data;
using UdbService.Models;

namespace Uplift.DataAccess.Data.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }catch(Exception ex)
            {
                
            }

            if (_db.Roles.Any(r => r.Name == "Admin")) return;

            _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
            

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "shaffat@gmail.com",
                Email = "shaffat@gmail.com",
                EmailConfirmed = true,
                Name= "shaffat",
                PhoneNumber = "01772994901"
                

            }, "Admin@123").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUser.Where(u => u.Email == "shaffat@gmail.com").FirstOrDefault();
            _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();

        }
    }
}
