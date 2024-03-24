
using System.ComponentModel.DataAnnotations;

 
namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage="UserName Required")]
        public string UserName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public DateOnly BirthDate { get; set; }  
        [Required]
        public string Country { get; set; }
        [Required]
        public string City{ get; set; }
        
        [Required(ErrorMessage="Password Required")]
        [StringLength(8,MinimumLength=4 , ErrorMessage ="Password Must be between 4 and 8 Charachters ")]
        public string Password { get; set; }
        

    }
}