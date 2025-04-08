using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models
{
    public class QuizAttempt
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Quiz ID is required")]
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        [Required(ErrorMessage = "Student ID is required")]
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        [Required(ErrorMessage = "Answers are required")]
        [StringLength(1000, ErrorMessage = "Answers cannot exceed 1000 characters")]
        public string Answers { get; set; }

        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        public double? Grade { get; set; }
    }
}