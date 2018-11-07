using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaMvc.Models
{
    public class UserProfileModel
    {
        public UserModel User { get; set; }
    }

    public class UserModel
    {
        public UserModel() { }
        public UserModel(BLL.DTO.UserDTO dbUser)
        {

            Description = dbUser.Description;
            LoginName = dbUser.LoginName;
            Nickname = dbUser.Nickname;
            SharedProfile = dbUser.SharedProfile;
            Id = dbUser.Id;

        }
        public long Id { get; set; }
        public string LoginName { get; set; }
        public string Nickname { get; set; }
        public string Description { get; set; }
        public bool SharedProfile { get; set; }

    }
}