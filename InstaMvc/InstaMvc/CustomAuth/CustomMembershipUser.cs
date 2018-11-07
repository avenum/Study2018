using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using BLL.DTO;

namespace InstaMvc.CustomAuth
{
    public class CustomMembershipUser : MembershipUser
    {
        #region User Properties

        public long UserId { get; set; }
        public string Nickname { get; set; }


        #endregion

        public CustomMembershipUser(UserDTO user) : base("CustomMembership", user.LoginName, user.Id, user.LoginName, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            UserId = user.Id;
            Nickname = user.Nickname;
        }
    }
}