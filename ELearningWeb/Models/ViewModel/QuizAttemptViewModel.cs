using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models.ViewModel
{
    public class QuizAttemptViewModel
    {
        //[Key]
        //public int Id { get; set; }

        [Required(ErrorMessage = "Quiz ID is required")]
        [Display(Name = "Quiz")]
        public int QuizId { get; set; }

        [Required(ErrorMessage = "Student ID is required")]
        [Display(Name = "Student")]
        public string StudentId { get; set; }

        [Required(ErrorMessage = "Answers are required")]
        [StringLength(1000, ErrorMessage = "Answers cannot exceed 1000 characters")]
        [Display(Name = "Answers")]
        public string Answers { get; set; }

        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        [Display(Name = "Grade")]
        public double? Grade { get; set; }

        // Display-specific properties
        [Display(Name = "Quiz Title")]
        public string QuizTitle { get; set; } // For display purposes in views

        [Display(Name = "Student Name")]
        public string StudentName { get; set; } // For display purposes in views
    }
}
