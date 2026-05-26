using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;

namespace XClone.Infrastructure.Services;

public class CloudinaryService(Cloudinary cloudinary) : ICloudinaryService
{
    public async Task<MediaFileDto?> UploadImage(Stream file, string fileName, string folder)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, file),
            Folder = folder,
            Transformation = new Transformation().Quality("auto").FetchFormat("auto")
        };

        var result = await cloudinary.UploadAsync(uploadParams);

        if (result.Error != null)
            return null;

        return new MediaFileDto
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            Format = result.Format,
            ResourceType = "image"
        };
    }

    public async Task<MediaFileDto?> UploadVideo(Stream file, string fileName, string folder)
    {
        var uploadParams = new VideoUploadParams
        {
            File = new FileDescription(fileName, file),
            Folder = folder,
            Transformation = new Transformation().Quality("auto").FetchFormat("auto")
        };

        var result = await cloudinary.UploadAsync(uploadParams);

        if (result.Error != null)
            return null;

        return new MediaFileDto
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            Format = result.Format,
            ResourceType = "video"
        };
    }

    public async Task<bool> Delete(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await cloudinary.DestroyAsync(deleteParams);
        return result.Result == "ok";
    }
}
