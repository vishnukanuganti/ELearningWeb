using ELearningWeb.Models;
using System.ComponentModel.DataAnnotations;

public class DiscussionPost
{
   

    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Class ID is required")]
    public int ClassId { get; set; }
    public Class Class { get; set; }

    [Required(ErrorMessage = "User ID is required")]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    [Required(ErrorMessage = "Content is required")]
    [StringLength(1000, MinimumLength = 2, ErrorMessage = "Content must be between 2 and 1000 characters")]
    public string Content { get; set; }

    [Required]
    public DateTime PostedAt { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public List<DiscussionReply> Replies { get; set; } = new List<DiscussionReply>();

}