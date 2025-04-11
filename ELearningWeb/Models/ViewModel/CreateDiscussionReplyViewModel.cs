using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.ViewModels
{
    public class CreateDiscussionReplyViewModel
    {
        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Content { get; set; }

        public int PostId { get; set; }
    }
}