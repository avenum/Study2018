using InstaMvc.CustomAuth;
using InstaMvc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstaMvc.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private UserProfileModel InitModel()
        {
            var dbUser = BLL.Data.GetUser(((CustomPrincipal)User).UserId);
            var model = new UserProfileModel
            {
                User = new UserModel(dbUser)
            };

            return model;
        }
        // GET: UserProfile
        public ActionResult Index()
        {

            return View(InitModel());
        }

        [HttpGet]
        public ActionResult EditProfile()
        {


            return View(InitModel().User);
        }

        [HttpPost]
        public ActionResult EditProfile(UserModel model, HttpPostedFileBase AvatarImage)
        {
            var user = BLL.Data.GetUser(model.Id);

            if (AvatarImage != null)
            {
                BLL.Data.SetAvatar(model.Id, new BLL.DTO.ImageWrapper(AvatarImage));
            }


            user.LoginName = model.LoginName;
            user.Nickname = model.Nickname;
            user.SharedProfile = model.SharedProfile;
            user.Description = model.Description;

            BLL.Data.CreateUpdateUser(user);

            return View(model);
        }

        public ActionResult Avatar(long Id)
        {
            var avatar = BLL.Data.GetAvatar(Id);

            if (avatar.Content == null)

                return HttpNotFound();
            return File(avatar.Content, avatar.Mime);
        }

    }
}