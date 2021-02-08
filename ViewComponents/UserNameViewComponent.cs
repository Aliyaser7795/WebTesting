using Microsoft.AspNetCore.Mvc;
using Spice.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Spice.ViewComponents
{
    public class UserNameViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext db;

        public UserNameViewComponent(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var userfromDb = await db.ApplicationUsers.FindAsync(Claim.Value);

            return View(userfromDb);
        }
    }
}
