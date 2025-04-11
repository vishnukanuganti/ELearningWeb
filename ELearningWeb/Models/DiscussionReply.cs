using ELearningWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models
{
    public class DiscussionReply
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public int PostId { get; set; }
        public DiscussionPost Post { get; set; }
    }
}