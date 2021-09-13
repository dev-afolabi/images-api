using FletcherProj.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FletcherProj.Interfaces
{
    public interface IImageService
    {
        public int TotalCount { get; set; }

        Task<bool> AddImageAsync(Images image);
        Task<IEnumerable<Images>> GetAllImagesAsync(int page, int perPage,string year);
        Task<bool> DeleteImagesAsync(string imageId);
    }
}
