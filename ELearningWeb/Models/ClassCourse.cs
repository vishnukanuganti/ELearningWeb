using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models
{
    public class ClassCourse
    {
        [Required]
        public int ClassId { get; set; }
        public Class Class { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
