using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
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
        public List<Photo> Photos { get; set; } = new List<Photo>();

        public virtual List<UserLike> LikedByUsers { get; set; } //users  who will like current users
        public  virtual List<UserLike> LikedUsers{ get; set; } // users who will be liked by current users


        
         /* Here we Removed Function To Can make AutoMapper Work Correctly and then Config in AutpMapper Profile 
          how to map MemberDto.Age with AppUser.BirthDate.CalculateAge()*/
        
        // public int GetAge(){
        //     return BirthDate.CalculateAge();  
        // }

    }
}
   
        