using System;

namespace BLL.DTO
{
    public class CommentDTO
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public long UserId { get; set; }
        public long PostId { get; set; }
        public string CommentText { get; set; }
       
        public  UserDTO User { get; set; }

    }
}