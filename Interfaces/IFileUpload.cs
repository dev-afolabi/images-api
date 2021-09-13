using FletcherProj.Dtos;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet.Actions;

namespace FletcherProj.Interfaces
{
    public interface IFileUpload
    {
        UploadImageResponse UploadImage(IFormFile file);
        DeletionResult DeleteImage(string publicId);
    }
}
