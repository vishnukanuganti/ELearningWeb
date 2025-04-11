using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters")]
        public string Title { get; set; }

        [Required]
        public int ClassId { get; set; }
        public Class Class { get; set; }

        public List<Question> Questions { get; set; } = new List<Question>();
        public List<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    }
}
