using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models.ViewModel
{
    public class GradeAttemptViewModel
    {
        [Required]
        public int AttemptId { get; set; }

        [Required(ErrorMessage = "Student name is required for display")]
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Quiz title is required for display")]
        [Display(Name = "Quiz Title")]
        public string QuizTitle { get; set; }

        [Required(ErrorMessage = "Answers are required for display")]
        [Display(Name = "Student Answers")]
        public string Answers { get; set; }

        [Required(ErrorMessage = "Grade is required")]
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        [Display(Name = "Grade")]
        public int Grade { get; set; }

        [Required]
        public int ClassId { get; set; }

        public Dictionary<int, bool> Correctness { get; set; } = new Dictionary<int, bool>();
    }
}
