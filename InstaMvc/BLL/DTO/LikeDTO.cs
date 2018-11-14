using System;

namespace BLL.DTO
{
    public class LikeDTO
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public long UserId { get; set; }
        public long PostId { get; set; }
        public  UserDTO User { get; set; }

    }
}