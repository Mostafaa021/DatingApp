using System.ComponentModel.DataAnnotations;
 
namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage="UserName Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage="Password Required")]
        public string Password { get; set; }


    }
}