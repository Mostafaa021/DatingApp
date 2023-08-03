using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Photos")]
    public class Photo
    {
         public int Id{ get; set; }
         public string URL{ get; set; }
         public bool IsMain {get; set; }

         public string PublicId { get; set; } // to be used with Cloudinary web Service

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
    
}
