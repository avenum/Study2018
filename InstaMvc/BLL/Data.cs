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
        public static long CreateUpdateUser(UserDTO user)
        {
            try
            {
                using (var ctx = new DAL.InstaDbEntities())
                {





                    var dbUser = ctx.Users.FirstOrDefault(x => x.Id == user.Id) ?? ctx.Users.Add(new DAL.User());

                    if (ctx.Users.Any(x => x.LoginName == user.LoginName && x.Id != dbUser.Id))
                        throw new Exception($"User with loginName :{user.LoginName} exist");


                    dbUser.Birtdate = user.Birtdate;
                    dbUser.Description = user.Description;
                    dbUser.LoginName = user.LoginName;
                    dbUser.Nickname = user.Nickname;
                    dbUser.PasswordHash = user.PasswordHash;
                    dbUser.RegDate = user.RegDate;
                    dbUser.Salt = user.Salt;
                    dbUser.SharedProfile = user.SharedProfile;

                    dbUser.AvatarContent = user.AvatarContent;
                    dbUser.AvatarMime = user.AvatarMime;


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
        public static UserDTO GetUser(long? id = null, string Login = null)
        {
            if (!id.HasValue && string.IsNullOrEmpty(Login))
                return null;
            try
            {
                using (var ctx = new DAL.InstaDbEntities())
                {
                    var dbUser = ctx.Users.FirstOrDefault(x => x.Id == id || x.LoginName == Login);
                    if (dbUser != null)
                    {
                        var user = new UserDTO
                        {
                            Birtdate = dbUser.Birtdate,
                            Description = dbUser.Description,
                            LoginName = dbUser.LoginName,
                            Nickname = dbUser.Nickname,
                            PasswordHash = dbUser.PasswordHash,
                            RegDate = dbUser.RegDate,
                            Salt = dbUser.Salt,
                            SharedProfile = dbUser.SharedProfile,
                            AvatarContent = dbUser.AvatarContent,
                            AvatarMime = dbUser.AvatarMime,
                            Id=dbUser.Id,
                        };

                        return user;
                    }

                    throw new Exception($"Not found User with ID:{id}");
                }
            }
            catch (Exception ex)
            {
                //throw;
                return null;
            }

        }

        public static bool ValidateUser(string login,string password)
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
