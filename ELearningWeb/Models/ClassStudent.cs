using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models
{
    public class ClassStudent
    {
        [Required]
        public int ClassId { get; set; }
        public Class Class { get; set; }

        [Required]
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
    }
}
