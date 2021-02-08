using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModels;
using Spice.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Spice.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }



        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //في حالة دخل مباشر بدون تسجيل دخول  في حالة اليوزر ما عمل كليمن 
            if (claim != null)
            {
                List<ShoppingCart> shoppingCarts = await db.ShoppingCarts.Where(m => m.ApplicationUserId == claim.Value).ToListAsync();
                HttpContext.Session.SetInt32(SD.ShoppingCartCount,shoppingCarts.Count);
            }
            IndexViewModel IndexVM = new IndexViewModel()
            {
                Categories = await db.Categories.ToListAsync(),
                Coupons = await db.Coupons.Where(m => m.IsActive == true).ToListAsync(),
                MenuItems = await db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync(),
            };
            return View(IndexVM);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int itemid)
        { //عرض معلومات مينيو ايتم 
            var menuItem = await db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).Where(m => m.Id == itemid).FirstOrDefaultAsync();

            //اعطاء قيم 
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                MenuItem = menuItem,
                MenuItemId = menuItem.Id
            };
            return View(shoppingCart);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {

               
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                shoppingCart.ApplicationUserId = claim.Value;

                // اضافة منتج الى الشوبي كارت وفي حالة كان موود زيادة العدد
                var shoppingCartFromDB = await db.ShoppingCarts.Where(m => m.ApplicationUserId == shoppingCart.ApplicationUserId && m.MenuItemId == shoppingCart.MenuItemId).FirstOrDefaultAsync();

                if (shoppingCartFromDB == null)
                {
                    db.ShoppingCarts.Add(shoppingCart);
                }
                else
                {
                    shoppingCartFromDB.Count += shoppingCart.Count;
                }
                await db.SaveChangesAsync();
                //اجلب عدد عدد الايتم في الشوفيكارت لاظهارها

                var count = db.ShoppingCarts.Where(m => m.ApplicationUserId == shoppingCart.ApplicationUserId).ToList().Count;

                HttpContext.Session.SetInt32(SD.ShoppingCartCount,count);

                return RedirectToAction(nameof(Index));
            }
            else {

                var menuItem = await db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).Where(m => m.Id == shoppingCart.MenuItemId).FirstOrDefaultAsync();

              
                ShoppingCart shoppingCartObj = new ShoppingCart()
                {
                    MenuItem = menuItem,
                    MenuItemId = menuItem.Id
                };
                return View(shoppingCartObj);

            }
        }
    }
}
