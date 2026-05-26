using XClone.Application.Models.DTOs;

namespace XClone.Application.Interfaces.Services;

public interface ICloudinaryService
{
    Task<MediaFileDto?> UploadImage(Stream file, string fileName, string folder);
    Task<MediaFileDto?> UploadVideo(Stream file, string fileName, string folder);
    Task<bool> Delete(string publicId);
}
