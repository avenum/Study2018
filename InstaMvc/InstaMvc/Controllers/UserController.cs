using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Models.LoginModel model)
        {

            var user = BLL.Data.GetUser(Login: model.Login);


            var salt = Convert.FromBase64String(user.Salt);
            var passhash = BLL.Hash.GenerateSaltedHash(model.Password, salt);

            var oldHash = Convert.FromBase64String(user.PasswordHash);

            if (BLL.Hash.CompareByteArrays(passhash, oldHash))
                ViewBag.Message = "Cool";
            else
                ViewBag.Message = "Bad";



            return View();
        }
    }
}