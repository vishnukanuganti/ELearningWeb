using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.ViewModels
{
    public class CreateDiscussionPostViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Content { get; set; }

        public int? ClassId { get; set; }
    }
}