using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models
{
    public class Class
    {
        

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Class name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Class name must be between 2 and 50 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        public List<ClassCourse> ClassCourses { get; set; } = new List<ClassCourse>();
        public List<ClassStudent> ClassStudents { get; set; } = new List<ClassStudent>();
        public List<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public List<DiscussionPost> DiscussionPosts { get; set; } = new List<DiscussionPost>(); // Added
    }
}
