using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models.ViewModel
{
    public class AttemptQuestionViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Question")]
        public string Text { get; set; }

        [Required]
        public string Type { get; set; } // "MultipleChoice" or "Text"

        [Display(Name = "Options")]
        public string Options { get; set; }

        [Display(Name = "Answer")]
        public string Answer { get; set; }
    }
}
