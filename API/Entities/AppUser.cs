using API.Extensions;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        //public int Id { get; set; }
        //public string UserName { get; set; }
        //public byte[] PasswordHash { get; set; } 
        //public byte[] PasswordSalt { get; set; } // identity take care of salting password 
        public DateOnly BirthDate { get; set; } 
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string KnownAs { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public string Interests { get; set; }
        public ICollection<Photo> Photos { get; set; } = new List<Photo>();

        public virtual ICollection<UserLike> LikedByUsers { get; set; } //users  who will like current users
        public  virtual ICollection<UserLike> LikedUsers{ get; set; } // users who will be liked by current users

        public virtual ICollection<Message> MessagesSent { get; set; }
        public virtual ICollection<Message> MessagesRecieved { get; set; }

        public virtual ICollection<AppUserRole> UserRoles { get; set; } // navigation property User (M) <==> Role(M) 
        //public virtual ICollection<IdentityUserClaim<int>> Claims { get; set; }
        //public virtual ICollection<IdentityUserLogin<int>> Logins { get; set; }
        //public virtual ICollection<IdentityUserToken<int>> Tokens { get; set; }




        /* Here we Removed Function To Can make AutoMapper Work Correctly and then Config in AutpMapper Profile 
         how to map MemberDto.Age with AppUser.BirthDate.CalculateAge()*/

        // public int GetAge(){
        //     return BirthDate.CalculateAge();  
        // }

    }
}
   
        