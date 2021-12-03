using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UserM1.Models;



namespace UserM1.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult SaveRegisterDetails(Register registerDetails)
        {
            if (ModelState.IsValid)
            {
                using(var databaseContext=new loginRegisterMVCEntities2())
                {
                    registeredUser2 relog = new registeredUser2();

                    relog.username = registerDetails.username;
                    relog.password = registerDetails.password;
                    relog.email = registerDetails.email;
                    relog.birthday = registerDetails.birthday;
                    relog.gender = registerDetails.gender;
                    relog.phone = registerDetails.phone;

                    databaseContext.registeredUser2.Add(relog);
                    databaseContext.SaveChanges();

                }

                ViewBag.Message = "User Details Saved";
                return View("Register");

            }

            else
            {
                return View("Register", registerDetails);
            }
        }


        [HttpPost]
        public ActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var isValidUser = IsValidUser(model);
                if (isValidUser != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("Index");
                }
                else
                {
                    //If the username and password combination is not present in DB then error message is shown.
                    ModelState.AddModelError("Failure", "Wrong Username and password combination !");
                    return View();
                }
            }
            else
            {
                //If model state is not valid, the model with error message is returned to the View.
                return View(model);
            }

        }

        public registeredUser2 IsValidUser(Login model)
        {
            using (var dataContext = new loginRegisterMVCEntities2())
            {
                //Retireving the user details from DB based on username and password enetered by user.
                registeredUser2 user = dataContext.registeredUser2.Where(query => query.email.Equals(model.Email) && query.password.Equals(model.Password)).SingleOrDefault();
                //If user is present, then true is returned.
                if (user == null)
                    return null;
                //If user is not present false is returned.
                else
                    return user;
            }
        }












    }
}