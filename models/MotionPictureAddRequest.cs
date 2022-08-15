using System.ComponentModel.DataAnnotations;

namespace MotionPicturesCore.Models
{
    public class MotionPictureAddRequest
    {
        [Required]
        [StringLength(50, ErrorMessage = "Motion Picture title cannot exceed 50 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Motion Picture description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required]
        [Range(minimum: 1000, maximum: 9999, ErrorMessage = "Release year must be entered")]
        public int ReleaseYear { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        public int CreatedBy { get; set; }
    }
}
