using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FletcherProj.Models
{
    public class Images
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Year { get; set; }
        public string DateCreated { get; set; } = DateTime.Now.ToString();
        public string DateUpdated { get; set; } = DateTime.Now.ToString();
        [Required]
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }

    }
}
