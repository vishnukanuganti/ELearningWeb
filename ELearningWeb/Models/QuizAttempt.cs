using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models
{
    public class QuizAttempt
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        [Required]
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        [Required]
        public string? Answers { get; set; } // JSON serialized Dictionary<int, string>

        public int? Grade { get; set; } // Using int? for simplicity
    }
}

