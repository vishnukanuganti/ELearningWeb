using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Course ID is required")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Review cannot exceed 500 characters")]
        public string Comment { get; set; }
    }
}
