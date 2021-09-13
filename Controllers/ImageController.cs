using FletcherProj.Commons;
using FletcherProj.Dtos;
using FletcherProj.Interfaces;
using FletcherProj.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FletcherProj.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IFileUpload _fileupload;
        private readonly IImageService _imageService;
        private readonly int _perPage;

        public ImageController(IFileUpload fileupload, IImageService imageService, IConfiguration configuration)
        {
            _fileupload = fileupload;
            _imageService = imageService;
            _perPage = Convert.ToInt32(configuration.GetSection("PaginationSettings:perPage").Value);
        }

        [HttpGet]
        [Route("year/{year}")]
        public async Task<IActionResult> GetAllImagesByYear([FromQuery] int page, [FromRoute]string year)
        {
            if (!ModelState.IsValid)
            {
                var responseObj = Utilities.CreateResponse("Model state error", ModelState, "");
                return BadRequest(responseObj);
            }

            page = page <= 0 ? 1 : page;

            var response = await _imageService.GetAllImagesAsync(page, _perPage, year);

            var pageMetaData = Utilities.Paginate(page, _perPage, _imageService.TotalCount);
            var pagedItems = new PaginatedResultDto<Images> { PageMetaData = pageMetaData, ResponseData = response };

            var message = pageMetaData.Total > 0 ? "Photos Retrieved Successfully." : "No Images in the database.";

            return Ok(Utilities.CreateResponse(message, errs: null, data: pagedItems));
        }


        [HttpPost]
        [Route("{year}")]
        public async Task<IActionResult> UploadImage([FromRoute] string year, [FromForm] IFormFile file)
        {

            if (!ModelState.IsValid)
            {
                var responseObj = Utilities.CreateResponse("Model state error", ModelState, "");
                return BadRequest(responseObj);
            }

            if (string.IsNullOrWhiteSpace(year))
            {
                var responseObj = Utilities.CreateResponse("Please add a valid year", ModelState, "");
                return BadRequest(responseObj);
            }

            // Checking for valid image format
            var vetPic = CheckPictureTypeAndSize(file);
            if (vetPic == "SizeError")
                return BadRequest("Image size is not valid");

            if (vetPic == null)
                return BadRequest("Could not add image");


            var uploadImageResponse = _fileupload.UploadImage(file);

            Images uploadImages = new Images
            {
                ImageUrl = uploadImageResponse.ImageUrl, 
                PublicId = uploadImageResponse.PublicId,
                Year = year

            };

            var imgage = await _imageService.AddImageAsync(uploadImages);

            if (!imgage)
            {
                ModelState.AddModelError("Failed to add", "Could not add image"); // put the error you wish to see on the MVC View here...
                return NotFound(Utilities.CreateResponse(message: "Could not add image", errs: ModelState, ""));
            }

            var response = new PhotoDto
            {
                ImageUrl = uploadImages.ImageUrl,
                PublicId = uploadImages.PublicId
            };

            return Ok(Utilities.CreateResponse("Image successfully added.", errs: null, data: response));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImages(string id, string publicId)
        {

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(publicId))
            {
                ModelState.AddModelError("Invalid Id", "Ensure ids are added");
                return BadRequest(Utilities.CreateResponse("errors", ModelState, ""));
            }

            var result = await _imageService.DeleteImagesAsync(id);
            if (!result)
            {
                ModelState.AddModelError("Db Delete failed", "Failed to delete images from db");
                return BadRequest(Utilities.CreateResponse("Not found", ModelState, ""));
            }

            var errs = new Dictionary<string, string>();

            var deletionResult = _fileupload.DeleteImage(publicId);

            if (deletionResult.Error != null)
            {
                errs.Add($"Delete failed {publicId}", $"Failed to delete from cloudinary for public id: {publicId}");
            }

            if (errs.Count > 0)
            {
                foreach (var err in errs)
                {
                    ModelState.AddModelError(err.Key, err.Value);
                }
                return BadRequest(Utilities.CreateResponse("Cloudnary error", ModelState, ""));
            }

            return Ok(Utilities.CreateResponse(message: "Image successfully deleted!", errs: null, data: ""));

        }

        private string CheckPictureTypeAndSize(IFormFile picture)
        {
            var extensions = new List<string>() { ".jpg", ".jpeg", ".png" };
            string format = null;
            //check if picture is more than 1MB
            if (picture.Length > 1000000)
            {
                format = "SizeError";
                return format;
            }
            // check if picture has a valid extension
            foreach (var ext in extensions)
            {
                if (picture.FileName.ToLower().EndsWith(ext))
                {
                    format = "CorrectFormat";
                    break;
                }
            }
            return format;
        }
    }
}
