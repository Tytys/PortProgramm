//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PortProgramm.DataFolder
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public int RoleId { get; set; }
        public string UserFIO { get; set; }
        public string UserEmail { get; set; }
        public string PhoneNumber { get; set; }
        public int GenderId { get; set; }
        public int AdressId { get; set; }
        public byte[] PassportPhoto { get; set; }
    
        public virtual Adress Adress { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Role Role { get; set; }
    }
}
