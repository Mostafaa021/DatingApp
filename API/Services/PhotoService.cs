using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            Account Account = new (
                config.Value.CloudName ,
                config.Value.ApiKey ,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(Account);
            _cloudinary.Api.Secure = true;  
        }
        public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile file)
        {
            var UploadResult = new ImageUploadResult();
            if(file.Length > 0 )
            { 
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName,stream),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face"),
                    Folder = "Directory-Dotnet7" 
                };
               UploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return UploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string PublicId)
        {
            var DeleteParams = new DeletionParams(PublicId);

            return await _cloudinary.DestroyAsync(DeleteParams);
        }

        
    }
}