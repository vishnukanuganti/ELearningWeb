using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models
{
    public class StudentCourseProgress
    {
        [Required]
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Range(0, 100, ErrorMessage = "Progress must be between 0 and 100")]
        public double Progress { get; set; } // Percentage complete, 100 when marked complete
    }
}
