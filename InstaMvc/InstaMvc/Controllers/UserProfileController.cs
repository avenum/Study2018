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

            //if (Request.Files != null && Request.Files.Count != 0 && Request.Files[0].ContentLength > 0)
            // {
            //    var file = Request.Files[0];
            if (AvatarImage != null)
            {
                user.AvatarMime = AvatarImage.ContentType;
                using (var binaryReader = new BinaryReader(AvatarImage.InputStream))
                {
                    user.AvatarContent = binaryReader.ReadBytes(AvatarImage.ContentLength);

                }
            }
            // }


            user.LoginName = model.LoginName;
            user.Nickname = model.Nickname;
            user.SharedProfile = model.SharedProfile;

            BLL.Data.CreateUpdateUser(user);

            return View(model);
        }

        public ActionResult Avatar(long Id)
        {
            var user = BLL.Data.GetUser(Id);

            if (user.AvatarContent == null)

                return HttpNotFound();
            return File(user.AvatarContent, user.AvatarMime);
        }

    }
}