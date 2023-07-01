using System.ComponentModel.DataAnnotations;
 
namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage="UserName Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage="Password Required")]
        [StringLength(8,MinimumLength=4 , ErrorMessage ="Password Must be between 4 and 8 Charachters ")]
        public string Password { get; set; }


    }
}