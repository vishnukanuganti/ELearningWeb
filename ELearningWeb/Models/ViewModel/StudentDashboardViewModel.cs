namespace ELearningWeb.Models.ViewModel
{
    public class StudentDashboardViewModel
    {
        public List<QuizAttemptViewModel> QuizAttemptViewModels { get; set; }

        public List<StudentCourseProgress> EnrolledCourses { get; set; }
        public List<Class> Classes { get; set; }
        public List<QuizAttempt> QuizAttempts { get; set; }
    }
}
