using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Utility;
using System.IO;

using System.Threading.Tasks;

namespace Spice.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]// تصريح للمدير 
    [Area("Admin")]
    public class CouponsController : Controller
    {
        private readonly ApplicationDbContext db;
        public CouponsController(ApplicationDbContext db)
        {
            this.db = db;
        }
        //Create
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var coupons = await db.Coupons.ToListAsync();
            return View(coupons);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Coupon coupon)
        {

            if (ModelState.IsValid)
            {

                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] picture = null;
                    var fs = files[0].OpenReadStream();//  يعمل فتح عشان يقرا الملف الذي حملته 
                    var ms = new MemoryStream();
                    fs.CopyTo(ms);
                    picture = ms.ToArray();// حيحولها الى بايت  
                    coupon.Picture = picture;
                }

                db.Coupons.Add(coupon);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);

        }

        // Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var coupon = await db.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Coupon coupon)
        {

            if (ModelState.IsValid)
            {

                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] picture = null;
                    var fs = files[0].OpenReadStream();//  يعمل فتح عشان يقرا الملف الذي حملته 
                    var ms = new MemoryStream();
                    fs.CopyTo(ms);
                    picture = ms.ToArray();// حيحولها الى بايت  
                    coupon.Picture = picture;
                }

                db.Coupons.Update(coupon);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);

        }



        //Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var coupon = await db.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Coupon coupon)
        {



            db.Coupons.Remove(coupon);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // Details


        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var coupon = await db.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(Coupon coupon)
        {



            db.Coupons.Remove(coupon);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
