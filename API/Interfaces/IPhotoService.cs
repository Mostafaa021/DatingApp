using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        // returning image with PublicId(string) after uploading it on Cloduinary which will be stored in Database
        Task<ImageUploadResult> UploadPhotoAsync(IFormFile formFile); 
        Task<DeletionResult> DeletePhotoAsync(string PublicId);
    }
}