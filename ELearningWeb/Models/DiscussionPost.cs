using System.ComponentModel.DataAnnotations;
using System;

namespace ELearningWeb.Models
{
    public class DiscussionPost
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int? ClassId { get; set; } // Links to Class, not Course
        public Class Class { get; set; } // Navigation property

        public ICollection<DiscussionReply> Replies { get; set; }
    }
}