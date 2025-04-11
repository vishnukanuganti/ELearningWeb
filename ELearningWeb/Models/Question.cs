using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        [StringLength(500, ErrorMessage = "Question text cannot exceed 500 characters")]
        public string Text { get; set; }

        [Required]
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        [Required(ErrorMessage = "Question type is required")]
        [StringLength(20, ErrorMessage = "Question type cannot exceed 20 characters")]
        public string Type { get; set; } // "MultipleChoice" or "Text"

        [StringLength(500, ErrorMessage = "Options cannot exceed 500 characters")]
        public string Options { get; set; } // Comma-separated for MultipleChoice

        [Required(ErrorMessage = "Correct answer is required")]
        [StringLength(100, ErrorMessage = "Correct answer cannot exceed 100 characters")]
        public string CorrectAnswer { get; set; }
    }
}

