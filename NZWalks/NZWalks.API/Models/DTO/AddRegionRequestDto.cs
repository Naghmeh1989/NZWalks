using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string Code { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
