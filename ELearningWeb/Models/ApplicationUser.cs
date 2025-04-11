using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Phone number must be between 7 and 15 characters")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters")]
        public string FullName { get; set; }

        public List<ClassStudent> ClassStudents { get; set; } = new List<ClassStudent>();

        public List<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();

        public List<StudentCourseProgress> CourseProgress { get; set; } = new List<StudentCourseProgress>(); // New: Tracks course enrollment and completion
    }
}