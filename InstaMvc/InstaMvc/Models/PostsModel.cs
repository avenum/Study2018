using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaMvc.Models
{
    public class PostsModel
    {
        public PostFormMode FormMode { get; set; }

        public bool NextExist { get; set; }

        public List<BLL.DTO.PostDTO> Posts { get; set; }
    }

    public enum PostFormMode
    {
        my,
        all
    }
}