namespace BLL.DTO
{
    public class PostTagDTO
    {
        public long PostId { get; set; }
        public string Tag { get; set; }

        public PostDTO Post { get; set; }

    }
}