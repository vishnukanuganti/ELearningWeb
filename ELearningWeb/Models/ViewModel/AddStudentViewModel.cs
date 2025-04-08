namespace ELearningWeb.Models.ViewModel
{
    public class AddStudentViewModel
    {
        public int ClassId { get; set; }
        public List<ApplicationUser> Students { get; set; }
    }
}
