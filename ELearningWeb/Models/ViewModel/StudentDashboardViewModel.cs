namespace ELearningWeb.Models.ViewModel
{
    public class StudentDashboardViewModel
    {
        public List<StudentCourseProgress> EnrolledCourses { get; set; } = new List<StudentCourseProgress>();
        public List<Class> Classes { get; set; } = new List<Class>();
        public List<StudentQuizAttemptViewModel> QuizAttemptViewModels { get; set; } = new List<StudentQuizAttemptViewModel>();
    }
}
