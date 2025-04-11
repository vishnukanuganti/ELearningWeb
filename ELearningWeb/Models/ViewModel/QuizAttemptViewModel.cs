using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models.ViewModel
{
    public class QuizAttemptViewModel
    {
        [Required]
        public int QuizId { get; set; }

        //[Display(Name = "Quiz Title")]
        //public string Title { get; set; } // No validation, only for display

        public List<AttemptQuestionViewModel> Questions { get; set; } = new List<AttemptQuestionViewModel>();

        public Dictionary<int, string> Answers { get; set; } = new Dictionary<int, string>(); // For form binding
    }

}
