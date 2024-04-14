using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public virtual  ICollection<AppUserRole> UserRoles { get; set; } // navigation property User (M) <==> Role(M) 
        //public virtual ICollection<IdentityRoleClaim<int>> RoleClaims { get; set; }
    }
}
