using FletcherProj.Data;
using FletcherProj.Interfaces;
using FletcherProj.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FletcherProj.Implementations
{
    public class ImageService : IImageService
    {
        private readonly AppDbContext _dbcontext;

        public ImageService(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public int TotalCount { get; set; }

        public async Task<bool> AddImageAsync(Images image)
        {
            await _dbcontext.Images.AddAsync(image);
            return await _dbcontext.SaveChangesAsync() > 0? true:false;
        }

        public async Task<IEnumerable<Images>> GetAllImagesAsync(int page, int perPage, string year)
        {
            var images = await _dbcontext.Images.Where(x => x.Year == year).Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            TotalCount = images.Count();
            return images;
        }

        public async Task<bool> DeleteImagesAsync(string imageId)
        {
            var image = await _dbcontext.Images.FirstOrDefaultAsync(x => x.Id == imageId);

            if (image != null)
            {
                _dbcontext.Remove(image);  
            }
            return await _dbcontext.SaveChangesAsync() > 0 ? true : false;

        }
    }
}
