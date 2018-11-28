using InstaMvc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstaMvc
{
    [Authorize]

    public class PostController : Controller
    {
        private long? _currentUserId { get { return ((CustomAuth.CustomPrincipal)User)?.UserId; } }
        // GET: Post
        public ActionResult Index()
        {
            var model = new Models.PostsModel { FormMode = PostFormMode.all };
            model.Posts = BLL.Data.GetPosts();

            model.NextExist = BLL.Data.GetPosts(1).Any();

            return View(model);
        }
        public ActionResult My()
        {
            var model = new Models.PostsModel { FormMode = PostFormMode.my };
            model.Posts = BLL.Data.GetPosts(userId: _currentUserId);
            model.NextExist = BLL.Data.GetPosts(1, _currentUserId).Any();
            return View("Index", model);
        }

        [HttpPost]
        public JsonResult PublishPost(long id)
        {
            var result = new JsonResultResponse { Success = true };
            try
            {
                BLL.Data.PublishPost(id);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Result = ex.Message;
            }
            return Json(result);

        }

        [HttpGet]
        public PartialViewResult Create()
        {
            ViewBag.Images = BLL.Data.GetMyImagesIds(_currentUserId.Value);



            return PartialView();
        }
        [HttpPost]
        public JsonResult Create(BLL.DTO.PostDTO model)
        {
            var result = new JsonResultResponse { Success = true };
            try
            {
                model.UserId = _currentUserId.Value;

                BLL.Data.CreatePost(model);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Result = ex.Message;
            }
            return Json(result);
        }




        [HttpPost]
        public JsonResult UploadImages()
        {
            var result = new JsonResultResponse { Success = true };
            try
            {
                if (Request.Files != null && Request.Files.Count != 0 && Request.Files[0].ContentLength > 0)
                {
                    var file = Request.Files[0];

                    var userId = _currentUserId.Value;

                    result.Result = BLL.Data.AddImage(userId, new BLL.DTO.ImageWrapper(file));
                }
                else
                    throw new Exception("Не найдено файлов");

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Result = ex.Message;
            }
            return Json(result);

        }

        public ActionResult GetImage(long Id)
        {
            var avatar = BLL.Data.GetImage(Id);

            if (avatar.Content == null)
                return HttpNotFound();

            return File(avatar.Content, avatar.Mime);
        }

        [HttpPost]
        public JsonResult DelImage(long id)
        {
            var result = new JsonResultResponse { Success = true };
            try
            {
                BLL.Data.DelImage(id);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Result = ex.Message;
            }
            return Json(result);

        }

        public PartialViewResult GetPosts(int page, bool my = false)
        {
            var model = new Models.PostsModel { FormMode = my ? PostFormMode.my : PostFormMode.all };
            model.Posts = BLL.Data.GetPosts(page, my ? _currentUserId : null);
            model.NextExist = BLL.Data.GetPosts(page + 1, my ? _currentUserId : null).Any();

            return PartialView("_MorePostsView", model);
        }

        public PartialViewResult GetPost(long id)
        {
            var model = BLL.Data.GetPostById(id);
            return PartialView("_PostView", model);
        }

    }
}