using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models.ViewModel
{
    public class DiscussionPostViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Class ID is required")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "Content must be between 2 and 1000 characters")]
        public string Content { get; set; }

        public DateTime PostedAt { get; set; }

        // Display-specific properties
        [Display(Name = "Posted By")]
        public string UserName { get; set; }

        [Display(Name = "Posted At")]
        public string FormattedPostedAt => PostedAt.ToString("g"); // e.g., 4/4/2025 12:23 AM
    }
}
