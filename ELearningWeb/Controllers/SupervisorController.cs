using ELearningWeb.Data;
using ELearningWeb.Models.ViewModel;
using ELearningWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ELearningWeb.Controllers
{
    [Authorize(Roles = "Supervisor")]
    public class SupervisorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SupervisorController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Supervisor/Index (List of classes)
        [HttpGet]
        public IActionResult Index()
        {
            var classes = _context.Classes.ToList();
            return View(classes);
        }

        // GET: /Supervisor/CreateClass
        [HttpGet]
        public IActionResult CreateClass()
        {
            return View();
        }

        // POST: /Supervisor/CreateClass
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClass(Class model)
        {
            if (ModelState.IsValid)
            {
                _context.Classes.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: /Supervisor/AddCourse/{classId}
        [HttpGet]
        public IActionResult AddCourse(int classId)
        {
            var classEntity = _context.Classes.FirstOrDefault(c => c.Id == classId);
            if (classEntity == null) return NotFound();
            var model = new AddCourseViewModel
            {
                ClassId = classId,
                Courses = _context.Courses.ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCourse(int classId, int courseId)
        {
            if (_context.ClassCourses.Any(cc => cc.ClassId == classId && cc.CourseId == courseId))
            {
                var model = new AddCourseViewModel
                {
                    ClassId = classId,
                    Courses = _context.Courses.ToList()
                };
                ModelState.AddModelError("", "This course is already added to the class.");
                return View(model);
            }

            var classCourse = new ClassCourse { ClassId = classId, CourseId = courseId };
            _context.ClassCourses.Add(classCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ClassDetails), new { id = classId });
        }

        // GET: /Supervisor/AddStudent/{classId}
        [HttpGet]
        public IActionResult AddStudent(int classId)
        {
            var classEntity = _context.Classes.FirstOrDefault(c => c.Id == classId);
            if (classEntity == null) return NotFound();
            var model = new AddStudentViewModel
            {
                ClassId = classId,
                Students = _context.Users.Where(u => u.ClassStudents.All(cs => cs.ClassId != classId)).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(int classId, string studentId)
        {
            if (_context.ClassStudents.Any(cs => cs.ClassId == classId && cs.StudentId == studentId))
            {
                var model = new AddStudentViewModel
                {
                    ClassId = classId,
                    Students = _context.Users.Where(u => u.ClassStudents.All(cs => cs.ClassId != classId)).ToList()
                };
                ModelState.AddModelError("", "This student is already enrolled in the class.");
                return View(model);
            }

            var classStudent = new ClassStudent { ClassId = classId, StudentId = studentId };
            _context.ClassStudents.Add(classStudent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ClassDetails), new { id = classId });
        }

        [HttpGet]
        public IActionResult CreateQuiz(int? classId)
        {
            var classes = _context.Classes.ToList();
            if (!classes.Any())
            {
                TempData["Error"] = "No classes available. Please create a class first.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Classes = new SelectList(
                classes.Select(c => new { c.Id, c.Name }),
                "Id",
                "Name",
                classId.HasValue && classes.Any(c => c.Id == classId.Value) ? classId : classes.First().Id
            );

            var quizViewModel = new QuizViewModel();
            if (classId.HasValue && classes.Any(c => c.Id == classId.Value))
            {
                quizViewModel.ClassId = classId.Value;
            }
            else
            {
                quizViewModel.ClassId = classes.First().Id; // Default to the first class
            }

            return View(quizViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuiz(QuizViewModel quizViewModel)
        {
            System.Diagnostics.Debug.WriteLine($"CreateQuiz POST - ClassId: {quizViewModel.ClassId}, Title: {quizViewModel.Title}");

            if (quizViewModel.ClassId <= 0 || !_context.Classes.Any(c => c.Id == quizViewModel.ClassId))
            {
                ModelState.AddModelError("ClassId", $"The selected Class ID {quizViewModel.ClassId} is invalid. Available Class IDs are: {string.Join(", ", _context.Classes.Select(c => c.Id))}.");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = "Failed to create quiz: " + string.Join(", ", errors);
                System.Diagnostics.Debug.WriteLine("ModelState Errors: " + string.Join(", ", errors));
                ViewBag.Classes = new SelectList(
                    _context.Classes.Select(c => new { c.Id, c.Name }),
                    "Id",
                    "Name",
                    quizViewModel.ClassId
                );
                return View(quizViewModel);
            }

            var quiz = new Quiz
            {
                ClassId = quizViewModel.ClassId,
                Title = quizViewModel.Title
            };

            try
            {
                _context.Quizzes.Add(quiz);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Quiz created successfully!";
                return RedirectToAction(nameof(ClassDetails), new { id = quiz.ClassId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error saving quiz: {ex.Message}";
                ViewBag.Classes = new SelectList(
                    _context.Classes.Select(c => new { c.Id, c.Name }),
                    "Id",
                    "Name",
                    quizViewModel.ClassId
                );
                return View(quizViewModel);
            }
        }


        // GET: /Supervisor/ClassDetails/{id}
        [HttpGet]
        public IActionResult ClassDetails(int id)
        {
            var classEntity = _context.Classes
                .Include(c => c.ClassCourses).ThenInclude(cc => cc.Course)
                .Include(c => c.ClassStudents).ThenInclude(cs => cs.Student)
                .Include(c => c.Quizzes).ThenInclude(q => q.Attempts).ThenInclude(a => a.Student)
                .FirstOrDefault(c => c.Id == id);

            if (classEntity == null)
            {
                return NotFound();
            }
            return View(classEntity);
        }

        // POST: /Supervisor/GradeAttempt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GradeAttempt(int attemptId, double grade)
        {
            var attempt = await _context.QuizAttempts
                .Include(qa => qa.Quiz)
                .FirstOrDefaultAsync(qa => qa.Id == attemptId);

            if (attempt == null)
            {
                TempData["Error"] = "Quiz attempt not found.";
                return RedirectToAction(nameof(Index));
            }

            if (grade < 0 || grade > 100)
            {
                TempData["Error"] = "Grade must be between 0 and 100.";
                return RedirectToAction(nameof(ClassDetails), new { id = attempt.Quiz.ClassId });
            }

            attempt.Grade = grade;
            await _context.SaveChangesAsync();
            TempData["Message"] = "Grade updated successfully!";
            return RedirectToAction(nameof(ClassDetails), new { id = attempt.Quiz.ClassId });
        }
    }
}



