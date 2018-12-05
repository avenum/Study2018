namespace BLL.DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class UserDTO
    {
        public UserDTO() {}
    
        public long Id { get; set; }
        public string LoginName { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Nickname { get; set; }
        public System.DateTime RegDate { get; set; }
        public DateTime Birtdate { get; set; }
        public string Description { get; set; }
        public bool SharedProfile { get; set; }

    }
}
