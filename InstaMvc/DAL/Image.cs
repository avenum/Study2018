//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Image
    {
        public long Id { get; set; }
        public byte[] ImageContent { get; set; }
        public string MimeType { get; set; }
        public System.DateTime CreateDate { get; set; }
        public long UserId { get; set; }
        public Nullable<long> PostId { get; set; }
    
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
