using ELearningWeb.Models;
using System.ComponentModel.DataAnnotations;


// Models/Course.cs (Updated)
public class Course
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Course name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Course name must be between 2 and 100 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Subject is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Subject must be between 2 and 50 characters")]
    public string Subject { get; set; }

    [StringLength(500, ErrorMessage = "Overview cannot exceed 500 characters")]
    public string Overview { get; set; }

    [StringLength(500, ErrorMessage = "Syllabus cannot exceed 500 characters")]
    public string Syllabus { get; set; }

    [StringLength(500, ErrorMessage = "Prerequisites cannot exceed 500 characters")]
    public string Prerequisites { get; set; }

    //[Range(0, 5, ErrorMessage = "Review rating must be between 0 and 5")]
   // public double ReviewRating { get; set; }

    public double AverageRating => Reviews.Any() ? Reviews.Average(r => r.Rating) : 0; // Calculate average rating

    public List<ClassCourse> ClassCourses { get; set; } = new List<ClassCourse>();
    public List<StudentCourseProgress> CourseProgress { get; set; } = new List<StudentCourseProgress>();
    public List<Review> Reviews { get; set; } = new List<Review>();

    public List<DiscussionPost> DiscussionPosts { get; set; } = new List<DiscussionPost>();


}
