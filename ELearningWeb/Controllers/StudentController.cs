using ELearningWeb.Data;
using ELearningWeb.Models;
using ELearningWeb.Models.ViewModel;
using ELearningWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ELearningWeb.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Student/Dashboard (Shows enrolled courses, progress, and grades)
        [HttpGet]
        public IActionResult Dashboard()
        {
            var userId = _userManager.GetUserId(User);
            var model = new StudentDashboardViewModel
            {
                EnrolledCourses = _context.CourseProgress
                    .Where(scp => scp.StudentId == userId)
                    .Include(scp => scp.Course)
                    .ToList(),
                Classes = _context.ClassStudents
                    .Where(cs => cs.StudentId == userId)
                    .Include(cs => cs.Class)
                        .ThenInclude(c => c.Quizzes)
                        .ThenInclude(q => q.Questions)
                    .Include(cs => cs.Class)
                        .ThenInclude(c => c.QuizAttempts)
                        .ThenInclude(qa => qa.Student)
                    .Select(cs => cs.Class)
                    .ToList(),
                QuizAttemptViewModels = _context.QuizAttempts
                    .Where(qa => qa.StudentId == userId)
                    .Include(qa => qa.Quiz)
                    .Select(qa => new StudentQuizAttemptViewModel
                    {
                        Id = qa.Id,
                        QuizTitle = qa.Quiz.Title,
                        Answers = qa.Answers,
                        Grade = qa.Grade
                    })
                    .ToList()
            };
            return View(model);
        }

        // GET: /Student/AttemptQuiz/{quizId}
        [HttpGet]
        public async Task<IActionResult> AttemptQuiz(int quizId)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null || quiz.Questions.Count != 5)
            {
                TempData["Error"] = "Quiz not found or does not contain exactly 5 questions.";
                return RedirectToAction(nameof(Dashboard));
            }

            var viewModel = new QuizAttemptViewModel
            {
                QuizId = quiz.Id,
                //Title = quiz.Title,
                Questions = quiz.Questions.Select(q => new AttemptQuestionViewModel
                {
                    Id = q.Id,
                    Text = q.Text,
                    Type = q.Type,
                    Options = q.Type == "MultipleChoice" ? q.Options : null
                }).ToList()
            };

            // Initialize Answers dictionary with all question IDs
            foreach (var question in viewModel.Questions)
            {
                if (!viewModel.Answers.ContainsKey(question.Id))
                {
                    viewModel.Answers[question.Id] = string.Empty; // Default empty string
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AttemptQuiz(QuizAttemptViewModel viewModel)
        {
            // Debug ModelState
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Debug.WriteLine($"Validation Error: {error.ErrorMessage}");
                }

                var quizForValidation = await _context.Quizzes
                    .Include(q => q.Questions)
                    .FirstOrDefaultAsync(q => q.Id == viewModel.QuizId);

                if (quizForValidation == null || quizForValidation.Questions.Count != 5)
                {
                    TempData["Error"] = "Quiz not found or does not contain exactly 5 questions.";
                    return RedirectToAction(nameof(Dashboard));
                }

                viewModel.Questions = quizForValidation.Questions.Select(q => new AttemptQuestionViewModel
                {
                    Id = q.Id,
                    Text = q.Text,
                    Type = q.Type,
                    Options = q.Type == "MultipleChoice" ? q.Options : null
                }).ToList();

                // Reinitialize Answers for redisplay
                foreach (var question in viewModel.Questions)
                {
                    if (!viewModel.Answers.ContainsKey(question.Id))
                    {
                        viewModel.Answers[question.Id] = string.Empty;
                    }
                }

                return View(viewModel);
            }

            Debug.WriteLine($"QuizId: {viewModel.QuizId}");
            Debug.WriteLine($"Answers: {Newtonsoft.Json.JsonConvert.SerializeObject(viewModel.Answers)}");

            var quiz = await _context.Quizzes
                .FirstOrDefaultAsync(q => q.Id == viewModel.QuizId);

            //if (quiz == null || quiz.Questions.Count != 5)
            //{
            //    TempData["Error"] = "Quiz not found or does not contain exactly 5 questions.";
            //    return RedirectToAction(nameof(Dashboard));
            //}

            // Validate all questions are answered
            if (viewModel.Answers.Count != 5 || viewModel.Answers.Values.Any(string.IsNullOrEmpty))
            {
                ModelState.AddModelError("", "Please answer all 5 questions.");
                viewModel.Questions = (await _context.Quizzes
                    .Include(q => q.Questions)
                    .FirstOrDefaultAsync(q => q.Id == viewModel.QuizId))?.Questions
                    .Select(q => new AttemptQuestionViewModel
                    {
                        Id = q.Id,
                        Text = q.Text,
                        Type = q.Type,
                        Options = q.Type == "MultipleChoice" ? q.Options : null
                    }).ToList() ?? new List<AttemptQuestionViewModel>();
                return View(viewModel);
            }

            var answersJson = JsonConvert.SerializeObject(viewModel.Answers);
            var attempt = new QuizAttempt
            {
                QuizId = viewModel.QuizId,
                StudentId = _userManager.GetUserId(User),
                Answers = answersJson
            };

            try
            {
                _context.QuizAttempts.Add(attempt);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Quiz submitted successfully!";
                return RedirectToAction(nameof(Dashboard));
            }
            catch (DbUpdateException ex)
            {
                Debug.WriteLine($"Database Error: {ex.InnerException?.Message}");
                ModelState.AddModelError("", "An error occurred while saving the quiz attempt. Please try again.");
                viewModel.Questions = (await _context.Quizzes
                    .Include(q => q.Questions)
                    .FirstOrDefaultAsync(q => q.Id == viewModel.QuizId))?.Questions
                    .Select(q => new AttemptQuestionViewModel
                    {
                        Id = q.Id,
                        Text = q.Text,
                        Type = q.Type,
                        Options = q.Type == "MultipleChoice" ? q.Options : null
                    }).ToList() ?? new List<AttemptQuestionViewModel>();
                return View(viewModel);
            }
        }

        // POST: /Student/MarkCourseComplete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkCourseComplete(int courseId)
        {
            var userId = _userManager.GetUserId(User);
            var progress = await _context.CourseProgress
                .FirstOrDefaultAsync(scp => scp.StudentId == userId && scp.CourseId == courseId);

            if (progress == null)
            {
                TempData["Error"] = "You are not enrolled in this course.";
                return RedirectToAction(nameof(Dashboard));
            }

            if (progress.Progress == 100)
            {
                TempData["Message"] = "This course is already marked as complete.";
            }
            else
            {
                progress.Progress = 100;
                await _context.SaveChangesAsync();
                TempData["Message"] = "Course marked as complete!";
            }
            return RedirectToAction(nameof(Dashboard));
        }
    }
}
