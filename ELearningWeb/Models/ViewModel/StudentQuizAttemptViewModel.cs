using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models.ViewModel
{
    public class StudentQuizAttemptViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Quiz Title")]
        public string QuizTitle { get; set; }

        [Required]
        [Display(Name = "Answers")]
        public string Answers { get; set; }

        [Display(Name = "Grade")]
        public int? Grade { get; set; }
    }
}
