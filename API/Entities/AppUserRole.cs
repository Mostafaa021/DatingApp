using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    
    public class AppUserRole : IdentityUserRole<int>   // represent table that join  many to many relationship between user and role 
    {
        public virtual AppUser User { get; set; }
        public  virtual AppRole Role { get; set; }

    }
}
