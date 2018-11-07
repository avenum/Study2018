using InstaMvc.CustomAuth;
using InstaMvc.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InstaMvc.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Registration()
        {
            var model = new Models.RegistrationModel()
            { SharedProfile = true };

            return View(model);
        }

        [HttpPost]
        public ActionResult Registration(Models.RegistrationModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            try
            {
                var salt = BLL.Hash.CreateSalt(16);
                var passhash = BLL.Hash.GenerateSaltedHash(model.Password, salt);
                var res = BLL.Data.CreateUpdateUser(new BLL.DTO.UserDTO
                {
                    Salt = Convert.ToBase64String(salt),
                    PasswordHash = Convert.ToBase64String(passhash),
                    Nickname = model.Nickname,
                    Birtdate = model.Birtdate,
                    RegDate = DateTime.Now,
                    SharedProfile = model.SharedProfile,
                    LoginName = model.LoginName
                });
                ViewBag.Message = res;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }


            return View(model);
        }

        [HttpGet]
        public ActionResult Login(string ReturnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.LoginModel model, string ReturnUrl = "")
        {

            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Login, model.Password))
                {
                    var user = (CustomMembershipUser)Membership.GetUser(model.Login, false);
                    if (user != null)
                    {
                        CustomSerializeModel userModel = new Models.CustomSerializeModel()
                        {
                            UserId = user.UserId,
                            Nickname = user.Nickname,
                        };

                        string userData = JsonConvert.SerializeObject(userModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                            (
                            1, model.Login, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                            );

                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie("Cookie1", enTicket);
                        Response.Cookies.Add(faCookie);
                    }

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Something Wrong : Username or Password invalid ^_^ ");
            return View(model);

        }
        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("Cookie1", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User", null);
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}