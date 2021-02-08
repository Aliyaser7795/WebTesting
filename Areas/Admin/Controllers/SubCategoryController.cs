using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModels;
using Spice.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]// تصريح للمدير 
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext db;

        [TempData]
        public String StatusMessage { get; set; }
        public SubCategoryController(ApplicationDbContext db)
        {
            this.db = db;
        }
        [HttpGet]


        public async Task<IActionResult> Index()
        {
            var subCategories = await db.SubCategories.Include(m => m.Category).ToListAsync();
            return View(subCategories);

        }
        [HttpGet]

        public async Task<IActionResult> Create()
        {
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoriesList = await db.Categories.ToListAsync(),
                SubCategory = new Models.SubCategory(),
              //التكررار لعدم
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesExistSubCategory = await db.SubCategories.Include(m => m.Category).Where(m => m.Category.Id == model.SubCategory.CategoryId && m.Name == model.SubCategory.Name).ToListAsync();
                    if (doesExistSubCategory.Count() > 0)
                {
                    /* عدم التكرار  */
                    StatusMessage = "Error : This is Sub Category Exists under  " + doesExistSubCategory.FirstOrDefault().Category.Name + "   Category ";
                }
                else {
                    db.SubCategories.Add(model.SubCategory);
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

                SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
                {
                    CategoriesList = await db.Categories.ToListAsync(),
                    SubCategory = model.SubCategory,
                 /*عدم التكرار */
                    StatusMessage=StatusMessage
                };
                return View(modelVM);
            }

        [HttpGet]


        public async Task<IActionResult> GetSubCategories(int id)
        {
            List<SubCategory> subCategories = new List<SubCategory>();
            subCategories =await db.SubCategories.Where(m => m.CategoryId == id).ToListAsync();
            return Json(new SelectList(subCategories, "Id", "Name"));
        }




        [HttpGet]

        public async Task<IActionResult> Edit(int? id)
        {


            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await db.SubCategories.FindAsync(id);
            if (subCategory == null)
            {
                return NotFound();
            }

            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoriesList = await db.Categories.ToListAsync(),
                SubCategory =subCategory
                //التكررار لعدم
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesExistSubCategory = await db.SubCategories.Include(m => m.Category).Where(m => m.Category.Id == model.SubCategory.CategoryId && m.Name == model.SubCategory.Name && m.Id != model.SubCategory.Id).ToListAsync();
                if (doesExistSubCategory.Count() > 0)
                {
                    /* عدم التكرار  */
                    StatusMessage = "Error : This is Sub Category Exists under  " + doesExistSubCategory.FirstOrDefault().Category.Name + "   Category ";
                }
                else
                {
                    db.SubCategories.Update(model.SubCategory);
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
            {
                CategoriesList = await db.Categories.ToListAsync(),
                SubCategory = model.SubCategory,
                /*عدم التكرار */
                StatusMessage = StatusMessage
            };
            return View(modelVM);
        }





        [HttpGet]

        public IActionResult Delete(int? id)
        {


            if (id == null)
            {
                return NotFound();
            }

            var subCategory =  db.SubCategories.Include(m=>m.Category).Where(m=>m.Id==id).SingleOrDefault();
            if (subCategory == null)
            {
                return NotFound();
            }

           
            return View(subCategory);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task< IActionResult> Delete(SubCategory subCategory)
        {

            db.SubCategories.Remove(subCategory);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        [HttpGet]

        public IActionResult Details(int? id)
        {


            if (id == null)
            {
                return NotFound();
            }

            var subCategory = db.SubCategories.Include(m => m.Category).Where(m => m.Id == id).SingleOrDefault();
            if (subCategory == null)
            {
                return NotFound();
            }


            return View(subCategory);
        }
    } 
}
