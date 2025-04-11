using ELearningWeb.Data;
using ELearningWeb.Models.ViewModel;
using ELearningWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Newtonsoft.Json;
using System.Diagnostics;

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
        public IActionResult CreateQuiz(int classId)
        {
            if (!_context.Classes.Any(c => c.Id == classId))
            {
                TempData["Error"] = "Class not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ClassId = classId;
            var viewModel = new CreateQuizViewModel
            {
                ClassId = classId,
                Questions = new List<CreateQuestionViewModel>
            {
                new CreateQuestionViewModel(),
                new CreateQuestionViewModel(),
                new CreateQuestionViewModel(),
                new CreateQuestionViewModel(),
                new CreateQuestionViewModel()
            }
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuiz(CreateQuizViewModel viewModel)
        {
            // Debug incoming model
            Debug.WriteLine($"Received Title: {viewModel.Title}");
            Debug.WriteLine($"Received ClassId: {viewModel.ClassId}");
            Debug.WriteLine($"Received Questions Count: {viewModel.Questions?.Count ?? 0}");
            foreach (var q in viewModel.Questions ?? new List<CreateQuestionViewModel>())
            {
                Debug.WriteLine($"Question: Text={q.Text}, Type={q.Type}, Options={q.Options}, CorrectAnswer={q.CorrectAnswer}");
            }

            // Validate ModelState
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Debug.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                ViewBag.ClassId = viewModel.ClassId;
                return View(viewModel);
            }

            // Validate exactly 5 questions
            if (viewModel.Questions == null || viewModel.Questions.Count != 5)
            {
                ModelState.AddModelError("", "Please provide exactly 5 questions.");
                ViewBag.ClassId = viewModel.ClassId;
                return View(viewModel);
            }

            // Validate each question
            for (int i = 0; i < viewModel.Questions.Count; i++)
            {
                var question = viewModel.Questions[i];
                if (string.IsNullOrEmpty(question.Text) || string.IsNullOrEmpty(question.Type) || string.IsNullOrEmpty(question.CorrectAnswer))
                {
                    ModelState.AddModelError($"Questions[{i}].Text", "All questions must have text, type, and correct answer.");
                    ViewBag.ClassId = viewModel.ClassId;
                    return View(viewModel);
                }
            }

            var classExists = await _context.Classes.AnyAsync(c => c.Id == viewModel.ClassId);
            if (!classExists)
            {
                TempData["Error"] = "Invalid class ID.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var quiz = new Quiz
                {
                    Title = viewModel.Title,
                    ClassId = viewModel.ClassId
                };
                _context.Quizzes.Add(quiz);
                await _context.SaveChangesAsync(); // Save quiz first to get Id

                Debug.WriteLine($"Quiz saved with Id: {quiz.Id}");

                foreach (var questionVm in viewModel.Questions)
                {
                    if (!string.IsNullOrEmpty(questionVm.Text))
                    {
                        var question = new Question
                        {
                            Text = questionVm.Text,
                            Type = questionVm.Type,
                            Options = questionVm.Options,
                            CorrectAnswer = questionVm.CorrectAnswer,
                            QuizId = quiz.Id
                        };
                        _context.Questions.Add(question);
                        Debug.WriteLine($"Added Question: Text={question.Text}, QuizId={question.QuizId}");
                    }
                }
                await _context.SaveChangesAsync(); // Save questions

                TempData["Message"] = "Quiz created successfully!";
                return RedirectToAction(nameof(ClassDetails), new { id = viewModel.ClassId });
            }
            catch (DbUpdateException ex)
            {
                Debug.WriteLine($"Database Error: {ex.InnerException?.Message}");
                ModelState.AddModelError("", "An error occurred while saving the quiz. Please check the data and try again.");
                ViewBag.ClassId = viewModel.ClassId;
                return View(viewModel);
            }
        }


        [HttpGet]
        public IActionResult ClassDetails(int id)
        {
            var classItem = _context.Classes
                .Include(c => c.ClassCourses)
                .ThenInclude(cc => cc.Course)
                .Include(c => c.ClassStudents)
                .ThenInclude(cs => cs.Student)
                .Include(c => c.Quizzes)
                .ThenInclude(q => q.Questions)
                .Include(c => c.QuizAttempts)
                .ThenInclude(qa => qa.Student)
                .FirstOrDefault(c => c.Id == id);

            if (classItem == null)
            {
                TempData["Error"] = "Class not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(classItem);
        }
        // POST: /Supervisor/GradeAttempt
        [HttpGet]
        public IActionResult GradeAttempt(int attemptId)
        {
            var attempt = _context.QuizAttempts
                .Include(qa => qa.Quiz)
                    .ThenInclude(q => q.Questions)
                .Include(qa => qa.Student)
                .FirstOrDefault(qa => qa.Id == attemptId);

            if (attempt == null)
            {
                TempData["Error"] = "Quiz attempt not found.";
                return RedirectToAction(nameof(Index));
            }

            if (attempt.Grade.HasValue)
            {
                TempData["Error"] = "This attempt has already been graded.";
                return RedirectToAction(nameof(ClassDetails), new { id = attempt.Quiz.ClassId });
            }

            var answers = JsonConvert.DeserializeObject<Dictionary<int, string>>(attempt.Answers);
            var gradeViewModel = new GradeAttemptViewModel
            {
                AttemptId = attempt.Id,
                StudentName = attempt.Student.FullName,
                QuizTitle = attempt.Quiz.Title,
                Answers = attempt.Answers,
                ClassId = attempt.Quiz.ClassId
            };

            foreach (var question in attempt.Quiz.Questions)
            {
                gradeViewModel.Correctness[question.Id] = answers.ContainsKey(question.Id) && answers[question.Id] == question.CorrectAnswer;
            }

            return View(gradeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GradeAttempt(GradeAttemptViewModel gradeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(gradeViewModel);
            }

            var attempt = await _context.QuizAttempts
                .Include(qa => qa.Quiz)
                .FirstOrDefaultAsync(qa => qa.Id == gradeViewModel.AttemptId);

            if (attempt == null)
            {
                TempData["Error"] = "Quiz attempt not found.";
                return RedirectToAction(nameof(Index));
            }

            attempt.Grade = gradeViewModel.Grade;
            await _context.SaveChangesAsync();
            TempData["Message"] = "Quiz attempt graded successfully!";
            return RedirectToAction(nameof(ClassDetails), new { id = attempt.Quiz.ClassId });
        }

    }

}

