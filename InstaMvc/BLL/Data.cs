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


    }
}
