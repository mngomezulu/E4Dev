using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using E4UsersMVCWebApp.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;

namespace E4UsersMVCWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private string datafilepath;
        private string _imgDirpath;
        private IWebHostEnvironment Environment;
        public IUserRepository ds;

        public UserController(IWebHostEnvironment _environment,ILogger<UserController> logger)
        {
            this.Environment = _environment;
            _logger = logger;
            datafilepath = string.Concat(this.Environment.WebRootPath, "/xmldata/Users.xml");
            _imgDirpath = string.Concat(this.Environment.WebRootPath, "/img/");
            ds = new UserRepository(datafilepath);
        }

        #region Validation Services

            [AcceptVerbs("GET", "POST")]
            public IActionResult VerifyName(string firstName, string lastName)
            {
                 if (!ds.IsExistsNameLastname(firstName, lastName))
                 {
                     return Json($"A user named {firstName} {lastName} already exists.");
                 }
               
                return Json(true);
            }

            [AcceptVerbs("GET", "POST")]
            public IActionResult VerifyPhone(
            [RegularExpression(@"^\d{3}-\d{3}-\d{4}$")] string phone)
            {
                if (!ModelState.IsValid)
                {
                    return Json($"Phone {phone} has an invalid format. Format: ###-###-####");
                }

                return Json(true);
            }

        #endregion

        #region Controller Actions
            public IActionResult Index()
            {
                var data = ds.GetListOfUsers();
                ViewBag.Title = "List of Users";
                ViewBag.ImgDir = _imgDirpath;
              return View(data.ToList());
            }

            public IActionResult Details(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                UserModel user = ds.GetUserByID(id.Value);
                if (user == null)
                {
                    return NotFound();
                }
                ViewBag.Title = "User Details";
                ViewBag.ImgDir = _imgDirpath;
            return View(user);
            }

            public IActionResult Create()
            {
               ViewBag.Title = "Create New User";
               return View(new UserModel());
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create([Bind("Id,FirstName,LastName,DoB,Cellphone,Email,ImagePath")] UserModel user)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                      //Add new User
                        ds.InsertUserModel(user);
                        ViewBag.Title = "Create New User";
                    return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
                return View(user);
            }

            [Route("User/edit/{id?}")]
            [HttpGet]
            public IActionResult Edit(int? id)
            {

                if (id == null)
                {
                    return NotFound();
                }

                UserModel user = ds.GetUserByID(id.Value);
                if (user == null)
                {
                    return NotFound();
                }
                 ViewBag.PageName = "Edit User";
                   return View("Edit", user);
            }

            [HttpPost]
            public IActionResult Edit(int id, [Bind("Id,FirstName,LastName,DoB,Cellphone,Email,ImagePath")] UserModel user)
            {
                if (id != user.Id)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                       //Update user
                       ds.EditUserModel(user);
                    }
                    catch (Exception /* ex */)
                    {
                        //Log the error (uncomment ex variable name and write a log.)
                        ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.");
                    }
                }
                  ViewBag.PageName = "Edit User";
                 return View(user);
            }

            [HttpGet]
            public IActionResult Delete(int? id, bool saveChangesError = true)
            {
                if (id == null)
                {
                    return NotFound();
                }

                UserModel user = ds.GetUserByID(id.Value);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }

            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public IActionResult DeleteConfirmed(int id)
            {
                try
                {
                UserModel user = ds.GetUserByID(id);
                if (user == null)
                {
                    return NotFound();
                }

                //Delete the user
                ds.DeleteUserModel(id);

                return RedirectToAction(nameof(Index));
                }
                catch (Exception /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
                }
            }
        #endregion



    }
}
