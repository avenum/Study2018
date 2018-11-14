using InstaMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstaMvc
{
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
            return PartialView();
        }
        [HttpPost]
        public JsonResult Create(BLL.DTO.PostDTO model)
        {
            var result = new JsonResultResponse { Success = true };
            try
            {







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