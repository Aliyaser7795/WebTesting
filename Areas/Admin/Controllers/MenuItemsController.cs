using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModels;
using Spice.Utility;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]// تصريح للمدير 
    [Area("Admin")]
    public class MenuItemsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        [BindProperty]
        public MenuItemViewModel MenuItemVM { get; set; }
        public MenuItem MenuItem { get; }
        public object CategoriesList { get; }

        public MenuItemsController(ApplicationDbContext db,IWebHostEnvironment webHostEnvironment)
        {
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
            MenuItemVM = new MenuItemViewModel()
            {
                MenuItem = new MenuItem(),
                CategoriesList=db.Categories.ToList()
            };
        }

        [HttpGet]
        public async Task< IActionResult> Index()
        {
            var menuItems = await db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync();
            return View(menuItems);
        }
        // Create
        [HttpGet]
        public IActionResult Create() {

            return View(MenuItemVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]

        public async Task<IActionResult> CreatePost()
        {
            if (ModelState.IsValid)
            {
                string webRootPath = webHostEnvironment.WebRootPath; //حيجيب ملف 
                string ImagePath = @"\images\b.jpg"; // صورة افتراضية موجودة 
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0) // في حالة قمت بتحميل صورة ينفذ الاتي  
                {
                    string ImageName = DateTime.Now.ToFileTime().ToString() + Path.GetExtension(files[0].FileName);// في حالة التكرار يغير اسم الصورة 
                    FileStream fileStream = new FileStream(Path.Combine(webRootPath,"images", ImageName), FileMode.Create); //  حيخل الصورة الجديدة بالمف الاميج
                    files[0].CopyTo(fileStream); // حيعمل نسخ للصورة نفسها بس  بغير اسم جديد في حالة التكرار الصورة 

                    ImagePath = @"\images\" + ImageName;// التخزين في قاعدة البيانات 
                }

                MenuItemVM.MenuItem.Image = ImagePath;
                db.MenuItems.Add(MenuItemVM.MenuItem);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
            return View(MenuItemVM);
        }


        //Edit



        [HttpGet]
        public async Task< IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var menuItem = db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefault(m => m.Id == id);//
            if (menuItem == null)
            {
                return NotFound();
            }
            MenuItemVM.MenuItem = menuItem;
            MenuItemVM.SubCatrgoriesList =await db.SubCategories.Where(m => m.CategoryId == MenuItemVM.MenuItem.CategoryId).ToListAsync();//تعبيتها من قواعد البيانات 
            return View(MenuItemVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]

        public async Task<IActionResult> EditPost()
        {
            if (ModelState.IsValid)
            {
                string webRootPath = webHostEnvironment.WebRootPath; //حيجيب ملف 
                
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0) // في حالة قمت بتحميل صورة ينفذ الاتي  
                {
                    string ImageName = DateTime.Now.ToFileTime().ToString() + Path.GetExtension(files[0].FileName);// في حالة التكرار يغير اسم الصورة 
                    FileStream fileStream = new FileStream(Path.Combine(webRootPath, "images", ImageName), FileMode.Create); //  حيخل الصورة الجديدة بالمف الاميج
                    files[0].CopyTo(fileStream); // حيعمل نسخ للصورة نفسها بس  بغير اسم جديد في حالة التكرار الصورة 

                   string ImagePath = @"\images\" + ImageName;// التخزين في قاعدة البيانات 
                    MenuItemVM.MenuItem.Image = ImagePath;
                }

                
                db.MenuItems.Update(MenuItemVM.MenuItem);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
            return View(MenuItemVM);
        }




        // Details

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var menuItem = db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefault(m => m.Id == id);//
            if (menuItem == null)
            {
                return NotFound();
            }
            MenuItemVM.MenuItem = menuItem;
            MenuItemVM.SubCatrgoriesList = await db.SubCategories.Where(m => m.CategoryId == MenuItemVM.MenuItem.CategoryId).ToListAsync();//تعبيتها من قواعد البيانات 
            return View(MenuItemVM);
        }

        // Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var menuItem = db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefault(m => m.Id == id);//
            if (menuItem == null)
            {
                return NotFound();
            }
            MenuItemVM.MenuItem = menuItem;
            MenuItemVM.SubCatrgoriesList = await db.SubCategories.Where(m => m.CategoryId == MenuItemVM.MenuItem.CategoryId).ToListAsync();//تعبيتها من قواعد البيانات 
            return View(MenuItemVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]

        public async Task<IActionResult> DeletePost()
        {
           
                /*
                string webRootPath = webHostEnvironment.WebRootPath; //حيجيب ملف 

                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0) // في حالة قمت بتحميل صورة ينفذ الاتي  
                {
                    string ImageName = DateTime.Now.ToFileTime().ToString() + Path.GetExtension(files[0].FileName);// في حالة التكرار يغير اسم الصورة 
                    FileStream fileStream = new FileStream(Path.Combine(webRootPath, "images", ImageName), FileMode.Create); //  حيخل الصورة الجديدة بالمف الاميج
                    files[0].CopyTo(fileStream); // حيعمل نسخ للصورة نفسها بس  بغير اسم جديد في حالة التكرار الصورة 

                    string ImagePath = @"\images\" + ImageName;// التخزين في قاعدة البيانات 
                    MenuItemVM.MenuItem.Image = ImagePath;
                }*/


                db.MenuItems.Remove(MenuItemVM.MenuItem);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


           
        }




    }
}
