using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class Data
    {
        static Data()
        {

        }

        public static long CreateUpdateUser(UserDTO user)
        {
            try
            {
                using (var ctx = new DAL.InstaDbEntities())
                {
                    var dbUser = ctx.Users.FirstOrDefault(x => x.Id == user.Id) ?? ctx.Users.Add(new DAL.User());

                    if (ctx.Users.Any(x => x.LoginName == user.LoginName && x.Id != dbUser.Id))
                        throw new Exception($"User with loginName :{user.LoginName} exist");

                    AutoMapper.Mapper.Map(user, dbUser);


                    ctx.SaveChanges();

                    return dbUser.Id;
                }
            }
            catch (Exception ex)
            {
                throw;
                //return -1;
            }

        }

        public static void SetAvatar(long UserId, ImageWrapper avatar)
        {
            using (var ctx = new DAL.InstaDbEntities())
            {
                var user = ctx.Users.FirstOrDefault(x => x.Id == UserId);
                if (user == null)
                    throw new Exception("WATTT?");

                user.AvatarContent = avatar.Content;
                user.AvatarMime = avatar.Mime;

                ctx.SaveChanges();
            }
        }

        public static ImageWrapper GetAvatar(long UserId)
        {
            var res = new ImageWrapper();
            using (var ctx = new DAL.InstaDbEntities())
            {
                var user = ctx.Users.FirstOrDefault(x => x.Id == UserId);
                if (user == null)
                    throw new Exception("User not Found");

                res.Content = user.AvatarContent;
                res.Mime = user.AvatarMime;
            }

            return res;
        }
        public static UserDTO GetUser(long? id = null, string Login = null)
        {
            if (!id.HasValue && string.IsNullOrEmpty(Login))
                return null;
            try
            {
                using (var ctx = new DAL.InstaDbEntities())
                {
                    var user = ctx.Users.Where(x => x.Id == id || x.LoginName == Login).Select(AutoMapper.Mapper.Map<DTO.UserDTO>).FirstOrDefault();
                    if (user != null)

                        return user;

                    throw new Exception($"Not found User with ID:{id}");
                }
            }
            catch (Exception ex)
            {
                //throw;
                return null;
            }

        }

        public static bool ValidateUser(string login, string password)
        {
            var user = BLL.Data.GetUser(Login: login);
            if (user != null)
            {
                var salt = Convert.FromBase64String(user.Salt);
                var passhash = BLL.Hash.GenerateSaltedHash(password, salt);

                var oldHash = Convert.FromBase64String(user.PasswordHash);

                return BLL.Hash.CompareByteArrays(passhash, oldHash);
            }
            return false;
        }

        public static long AddImage(long UserId, ImageWrapper ImageData)
        {
            using (var ctx = new DAL.InstaDbEntities())
            {
                var img = ctx.Images.Add(new DAL.Image
                {
                    UserId = UserId,
                    CreateDate = DateTime.Now,
                    ImageContent = ImageData.Content,
                    MimeType = ImageData.Mime,
                });


                ctx.SaveChanges();

                return img.Id;
            }
        }

        public static ImageWrapper GetImage(long ImageId)
        {
            var res = new ImageWrapper();
            using (var ctx = new DAL.InstaDbEntities())
            {
                var user = ctx.Images.FirstOrDefault(x => x.Id == ImageId);
                if (user == null)
                    throw new Exception("Image not Found");

                res.Content = user.ImageContent;
                res.Mime = user.MimeType;
            }

            return res;

        }

        public static void DelImage(long ImageId)
        {
            using (var ctx = new DAL.InstaDbEntities())
            {
                var img = ctx.Images.FirstOrDefault(x => x.Id == ImageId);
                if (img == null)
                    throw new Exception("Нет такой фотки");

                ctx.Images.Remove(img);

                ctx.SaveChanges();
            }

        }
        public static List<long> GetMyImagesIds(long UserId)
        {
            var res = new List<long>();
            using (var ctx = new DAL.InstaDbEntities())
            {
                res.AddRange(ctx.Images.Where(x => x.UserId == UserId && !x.PostId.HasValue).Select(x => x.Id));

            }

            return res;
        }

        public static void CreatePost(PostDTO post)
        {
            using (var ctx = new DAL.InstaDbEntities())
            {
                var imgIds = post.Images.Select(x => x.Id).ToArray();
                var images = ctx.Images.Where(x => imgIds.Contains(x.Id)).ToList();

                if (images.Any(x => x.PostId.HasValue))
                    throw new Exception("Это чужие картинки!!!");

                var dbPost = AutoMapper.Mapper.Map<DAL.Post>(post);

                dbPost.Images.Clear();
                images.ForEach((x) => dbPost.Images.Add(x));
                dbPost.CreateDate = DateTime.Now;



                ctx.Posts.Add(dbPost);
                ctx.SaveChanges();
            }
        }
        public static long CreateComment(CommentDTO com)
        {
            using (var ctx = new DAL.InstaDbEntities())
            {
                var dbCom = AutoMapper.Mapper.Map<DAL.Comment>(com);

                ctx.Comments.Add(dbCom);
                ctx.SaveChanges();

                return dbCom.Id;
            }
        }

        public static CommentDTO GetComment(long id)
        {
            using (var ctx = new DAL.InstaDbEntities())
            {
                var ct = ctx.Comments
                    .Where(x => x.Id == id)
                    .Select(AutoMapper.Mapper.Map<CommentDTO>)
                    .FirstOrDefault();

                return ct;
            }
        }


        public static PostDTO GetPostById(long id)
        {
            var res = (PostDTO)null;
            using (var ctx = new DAL.InstaDbEntities())
            {
                res = ctx.Posts.Where(x => x.Id == id).Select(AutoMapper.Mapper.Map<PostDTO>).FirstOrDefault();
            }

            return res;
        }
        public static List<PostDTO> GetPosts(int page = 0, long? userId = null)
        {
            var take = 3;
            var skip = take * page;
            var res = (List<PostDTO>)null;
            using (var ctx = new DAL.InstaDbEntities())
            {
                var temp = ctx.Posts.Where(x => !userId.HasValue && x.PublicateDate.HasValue || userId.HasValue && x.UserId == userId.Value);

                res = temp.OrderByDescending(x => x.PublicateDate).ThenByDescending(x => x.CreateDate).Skip(skip).Take(take).Select(AutoMapper.Mapper.Map<PostDTO>).ToList();
            }

            return res;
        }

        public static void PublishPost(long id)
        {
            using (var ctx = new DAL.InstaDbEntities())
            {
                var post = ctx.Posts.FirstOrDefault(x => x.Id == id);
                if (post != null)
                {
                    post.PublicateDate = DateTime.Now;
                    ctx.SaveChanges();
                }
            }

        }
    }
}
