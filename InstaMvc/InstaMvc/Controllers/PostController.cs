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
        // GET: Post
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult Create()
        {
            ViewBag.Images = BLL.Data.GetMyImagesIds(((CustomAuth.CustomPrincipal)User).UserId);



            return PartialView();
        }
        [HttpPost]
        public JsonResult Create(BLL.DTO.PostDTO model)
        {
            var result = new JsonResultResponse { Success = true };
            try
            {

                model.UserId = ((CustomAuth.CustomPrincipal)User).UserId;

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

                    var userId = ((CustomAuth.CustomPrincipal)User).UserId;

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

    }
}