using System;

namespace BLL.DTO
{
    public class ImageDTO
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public long UserId { get; set; }
        public long? PostId { get; set; }

    }
}