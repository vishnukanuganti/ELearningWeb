using ELearningWeb.Data;
using ELearningWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class CourseController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CourseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Index(string searchString, string subjectFilter, string sortOrder)
    {
        var courses = from c in _context.Courses select c;

        // Search by name
        if (!string.IsNullOrEmpty(searchString))
        {
            courses = courses.Where(c => c.Name.Contains(searchString));
        }

        // Filter by subject
        if (!string.IsNullOrEmpty(subjectFilter) && subjectFilter != "All")
        {
            courses = courses.Where(c => c.Subject == subjectFilter);
        }

        // Sort by review
        switch (sortOrder)
        {
            case "rating_desc":
                courses = courses.OrderByDescending(c => c.AverageRating);
                break;
            case "rating_asc":
                courses = courses.OrderBy(c => c.AverageRating);
                break;
            default:
                courses = courses.OrderBy(c => c.Name);
                break;
        }

        //ViewBag.Subjects = _context.Courses.Select(c => c.Subject).Distinct().ToList();
        //var courses1 = _context.Courses.ToList();
        //return View(courses1);
        ViewBag.Subjects = _context.Courses.Select(c => c.Subject).Distinct().ToList();
        return View(courses.ToList());
    }

    [HttpGet]
    public IActionResult CourseDetails(int id)
    {
        var course = _context.Courses
            .Include(c => c.Reviews)
            .ThenInclude(r => r.User)
            .FirstOrDefault(c => c.Id == id);
        if (course == null) return NotFound();

        var userId = _userManager.GetUserId(User);
        ViewBag.HasReviewed = userId != null && course.Reviews.Any(r => r.UserId == userId);
        return View(course);
    }

    [Authorize(Roles = "Student,Supervisor")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddReview(int courseId, int rating, string comment)
    {
        var course = await _context.Courses.FindAsync(courseId);
        if (course == null)
        {
            TempData["Error"] = "Course not found.";
            return RedirectToAction(nameof(Index));
        }

        var userId = _userManager.GetUserId(User);
        if (_context.Reviews.Any(r => r.CourseId == courseId && r.UserId == userId))
        {
            TempData["Error"] = "You have already reviewed this course.";
            return RedirectToAction(nameof(CourseDetails), new { id = courseId });
        }

        var review = new Review
        {
            CourseId = courseId,
            UserId = userId,
            Rating = rating,
            Comment = comment
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
        TempData["Message"] = "Review submitted successfully!";
        return RedirectToAction(nameof(CourseDetails), new { id = courseId });
    }

    [Authorize(Roles = "Student")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Enroll(int courseId)
    {
        var userId = _userManager.GetUserId(User);
        var course = await _context.Courses.FindAsync(courseId);
        if (course == null)
        {
            TempData["Error"] = "Course not found.";
            return RedirectToAction(nameof(Index));
        }

        if (_context.StudentCourseProgress.Any(scp => scp.StudentId == userId && scp.CourseId == courseId))
        {
            TempData["Message"] = "You are already enrolled in this course.";
        }
        else
        {
            var progress = new StudentCourseProgress
            {
                StudentId = userId,
                CourseId = courseId,
                Progress = 0
            };
            _context.StudentCourseProgress.Add(progress);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Successfully enrolled in the course!";
        }
        return RedirectToAction(nameof(CourseDetails), new { id = courseId });
    }
}