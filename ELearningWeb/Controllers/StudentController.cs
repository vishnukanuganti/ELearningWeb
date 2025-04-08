using ELearningWeb.Data;
using ELearningWeb.Models;
using ELearningWeb.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                EnrolledCourses = _context.StudentCourseProgress
                    .Where(scp => scp.StudentId == userId)
                    .Include(scp => scp.Course)
                    .ToList(),
                Classes = _context.ClassStudents
                    .Where(cs => cs.StudentId == userId)
                    .Include(cs => cs.Class)
                        .ThenInclude(c => c.ClassCourses)
                        .ThenInclude(cc => cc.Course)
                    .Include(cs => cs.Class)
                        .ThenInclude(c => c.Quizzes)
                        .ThenInclude(q => q.Attempts)
                    .Include(cs => cs.Class) // Added to include DiscussionPosts
                        .ThenInclude(c => c.DiscussionPosts) // Include discussion posts
                        .ThenInclude(dp => dp.User)
                    .Select(cs => cs.Class)
                    .ToList(),
                QuizAttemptViewModels = _context.QuizAttempts
                    .Where(qa => qa.StudentId == userId)
                    .Include(qa => qa.Quiz)
                    .Include(qa => qa.Student)
                    .Select(qa => new QuizAttemptViewModel
                    {
                        //Id = qa.Id,
                        QuizId = qa.QuizId,
                        QuizTitle = qa.Quiz.Title,
                        StudentId = qa.StudentId,
                        StudentName = qa.Student.FullName,
                        Answers = qa.Answers,
                        Grade = qa.Grade
                    })
                    .ToList()
            };
            return View(model);
        }

        // GET: /Student/AttemptQuiz/{quizId}
        [HttpGet]
        public IActionResult AttemptQuiz(int QuizId)
        {
            var userId = _userManager.GetUserId(User);
            var quiz = _context.Quizzes
                .Include(q => q.Attempts)
                .Include(q => q.Class).ThenInclude(c => c.ClassStudents)
                .FirstOrDefault(q => q.Id == QuizId);

            if (quiz == null || !quiz.Class.ClassStudents.Any(cs => cs.StudentId == userId))
            {
                TempData["Message"] = "You must be enrolled in the class to attempt this quiz.";
                return RedirectToAction(nameof(Dashboard));
            }

            if (quiz.Attempts.Any(a => a.StudentId == userId))
            {
                TempData["Message"] = "You have already attempted this quiz.";
                return RedirectToAction(nameof(Dashboard));
            }

            return View(quiz);
        }

        // POST: /Student/AttemptQuiz
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AttemptQuiz(int quizId, string answers)
        {
            var userId = _userManager.GetUserId(User);
            var quiz = _context.Quizzes
                .Include(q => q.Class).ThenInclude(c => c.ClassStudents)
                .FirstOrDefault(q => q.Id == quizId);

            if (quiz == null || !quiz.Class.ClassStudents.Any(cs => cs.StudentId == userId))
            {
                TempData["Message"] = "You must be enrolled in the class to attempt this quiz.";
                return RedirectToAction(nameof(Dashboard));
            }

            if (_context.QuizAttempts.Any(a => a.QuizId == quizId && a.StudentId == userId))
            {
                TempData["Message"] = "You have already attempted this quiz.";
                return RedirectToAction(nameof(Dashboard));
            }

            var attempt = new QuizAttempt
            {
                QuizId = quizId,
                StudentId = userId,
                Answers = answers
            };

            _context.QuizAttempts.Add(attempt);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Quiz submitted successfully!";
            return RedirectToAction(nameof(Dashboard));
        }

        // POST: /Student/MarkCourseComplete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkCourseComplete(int courseId)
        {
            var userId = _userManager.GetUserId(User);
            var progress = await _context.StudentCourseProgress
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
