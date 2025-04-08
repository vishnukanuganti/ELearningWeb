using ELearningWeb.Models;
using System.ComponentModel.DataAnnotations;

public class DiscussionReply
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Post ID is required")]
    public int PostId { get; set; }
    public DiscussionPost Post { get; set; }

    [Required(ErrorMessage = "User ID is required")]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    [Required(ErrorMessage = "Content is required")]
    [StringLength(1000, ErrorMessage = "Content cannot exceed 1000 characters")]
    public string Content { get; set; }

    public DateTime PostedAt { get; set; } = DateTime.UtcNow;
}