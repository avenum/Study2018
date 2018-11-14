using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class PostDTO
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        public long UserId { get; set; }
        public DbGeography Position { get; set; }
        public string LocationName { get; set; }
        public DateTime? PublicateDate { get; set; }

        public virtual List<CommentDTO> Comments { get; set; }
        public virtual List<ImageDTO> Images { get; set; }
        public virtual List<LikeDTO> Likes { get; set; }
        public virtual List<PostTagDTO> PostTags { get; set; }
        public virtual UserDTO User { get; set; }

    }
}
