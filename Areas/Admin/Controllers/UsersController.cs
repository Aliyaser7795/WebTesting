using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Spice.Areas.Admin.Controllers
{
    [Authorize(Roles=SD.ManagerUser)]// تصريح للمدير 

    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext db;
        public UsersController(ApplicationDbContext db)
        {
            this.db = db;
        }




        public async Task<IActionResult> Index()
        {
            var clamisIdentity = (ClaimsIdentity)User.Identity;
            var claim = clamisIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string UserId = claim.Value;
            return View(await db.ApplicationUsers.Where(m => m.Id != UserId).ToListAsync());//حيجيب جميع المستخدمين ماعدا المستخدم الحالي
        }

        public async Task<IActionResult> LockUnLock(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user =await db.ApplicationUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            else 
            {
                user.LockoutEnd = DateTime.Now;
            }
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        }
    }


