using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FletcherProj.Dtos;
using FletcherProj.Interfaces;
using FletcherProj.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;

namespace FletcherProj.Implementations
{
    public class FileUpload : IFileUpload
    {
        private readonly CloudinaryConfig _config;
        private readonly Cloudinary _cloudinary;

        public FileUpload(IOptions<CloudinaryConfig> config)
        {
            _config = config.Value;
            Account account = new Account(
                _config.CloudName,
                _config.ApiKey,
                _config.ApiSecret
             );

            _cloudinary = new Cloudinary(account);
        }

        public UploadImageResponse UploadImage(IFormFile file)
        {
            var imageUploadResult = new ImageUploadResult();

            if (file.Length <= 0)
                throw new InvalidOperationException("Invalid file size");

            using (var fs = file.OpenReadStream())
            {
                var imageUploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, fs),
                };
                imageUploadResult = _cloudinary.Upload(imageUploadParams);
            }
            var publicId = imageUploadResult.PublicId;
            var avatarUrl = imageUploadResult.Url.ToString();
            var result = new UploadImageResponse
            {
                PublicId = publicId,
                ImageUrl = avatarUrl
            };
            return result;
        }

        public DeletionResult DeleteImage(string publicId)
        {
            var delParams = new DeletionParams(publicId) { ResourceType = ResourceType.Image };
            return _cloudinary.Destroy(delParams);
        }
    }
}
