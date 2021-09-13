using FletcherProj.Models;
using System.Collections.Generic;

namespace FletcherProj.Dtos
{
    public class PaginatedResultDto<T>
    {
        public PageMetadata PageMetaData { get; set; }

        public IEnumerable<T> ResponseData { get; set; } = new List<T>();
    }
}
