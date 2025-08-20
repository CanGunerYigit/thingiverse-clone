using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Thingiverse.Application.Contracts.DTO
{
    public class ItemWithImagesDto
    {
        [Required]  
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [StringLength(100, ErrorMessage = "Name can be max 100 characters")]
        public string Name { get; set; } = "";

        [StringLength(500, ErrorMessage = "Description can be max 500 characters")]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "ThumbnailUrl is Required")]
        [Url(ErrorMessage = "Invalid ThumbnailUrl format")]
        public string ThumbnailUrl { get; set; } = "";

        public List<ItemImageDto> Images { get; set; } = new();

        [Required(ErrorMessage = "CreatorName is Required")]
        [StringLength(100, ErrorMessage = "CreatorName can be max 100 characters")]
        public string CreatorName { get; set; } = "";

        [Required(ErrorMessage = "CreatorUrl is Required")]
        [Url(ErrorMessage = "Invalid CreatorUrl format")]
        public string CreatorUrl { get; set; } = "";

        public DateTime CreatedAt { get; set; } // tarih not nullable
    }
}
