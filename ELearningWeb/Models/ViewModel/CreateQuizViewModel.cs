using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models.ViewModel
{

    public class CreateQuizViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters")]
        public string Title { get; set; }

        [Required]
        public int ClassId { get; set; }

        public List<CreateQuestionViewModel> Questions { get; set; } = new List<CreateQuestionViewModel>
        {
            new CreateQuestionViewModel(),
            new CreateQuestionViewModel(),
            new CreateQuestionViewModel(),
            new CreateQuestionViewModel(),
            new CreateQuestionViewModel()
        };
    }
}
