using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Users.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using Users.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Users.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {

            return View(UserManager.Users);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateModel model)
        {
            if (this.ModelState.IsValid)
            {
                AppUser user = new AppUser()
                {
                    Email = model.Email,
                    UserName = model.Name
                };

                IdentityResult result = await UserManager.CreateAsync(
                    user: user, 
                    password: model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            
            return View(model);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                this.ModelState.AddModelError("", error);
            }
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}