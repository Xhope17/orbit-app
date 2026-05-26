using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Responses;
using XClone.Domain.Exceptions;

namespace XClone.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MediaController(ICloudinaryService cloudinaryService) : ControllerBase
{
    [HttpPost("upload")]
    [EndpointSummary("Subir archivo multimedia")]
    [EndpointDescription("Sube una imagen o video a Cloudinary y devuelve la URL. Soporta imágenes (jpg, png, gif) y videos (mp4, mov).")]
    [ProducesResponseType<GenericResponse<MediaFileDto>>(StatusCodes.Status200OK)]
    [Tags("media", "upload")]
    public async Task<GenericResponse<MediaFileDto>> Upload(IFormFile file)
    {
        if (file is null || file.Length == 0)
            throw new BadRequestException("No se proporcionó un archivo válido");

        using var stream = file.OpenReadStream();
        var isVideo = file.ContentType.StartsWith("video/");
        var folder = isVideo ? "orbit/videos" : "orbit/images";

        MediaFileDto? result;
        if (isVideo)
            result = await cloudinaryService.UploadVideo(stream, file.FileName, folder);
        else
            result = await cloudinaryService.UploadImage(stream, file.FileName, folder);

        if (result is null)
            throw new BadRequestException("Error al subir el archivo a Cloudinary");

        return new GenericResponse<MediaFileDto>
        {
            Data = result,
            Message = "Archivo subido correctamente"
        };
    }

    [HttpDelete("{publicId}")]
    [EndpointSummary("Eliminar archivo multimedia")]
    [EndpointDescription("Elimina un archivo de Cloudinary usando su PublicId.")]
    [ProducesResponseType<GenericResponse<bool>>(StatusCodes.Status200OK)]
    [Tags("media", "upload")]
    public async Task<GenericResponse<bool>> Delete(string publicId)
    {
        var result = await cloudinaryService.Delete(publicId);
        return new GenericResponse<bool>
        {
            Data = result,
            Message = result ? "Archivo eliminado correctamente" : "Error al eliminar el archivo"
        };
    }
}
