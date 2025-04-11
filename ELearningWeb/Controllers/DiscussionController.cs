using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ELearningWeb.Data;
using ELearningWeb.Models;
using ELearningWeb.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

[Authorize]
public class DiscussionController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DiscussionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? classId)
    {
        var posts = string.IsNullOrEmpty(classId.ToString())
            ? _context.DiscussionPosts.Include(p => p.User).Include(p => p.Replies).ThenInclude(r => r.User).ToList()
            : _context.DiscussionPosts
                .Where(p => p.ClassId == classId)
                .Include(p => p.User)
                .Include(p => p.Replies).ThenInclude(r => r.User)
                .ToList();

        ViewBag.ClassId = classId;
        return View(posts);
    }

    [HttpGet]
    public IActionResult CreatePost(int? classId)
    {
        ViewBag.ClassId = classId;
        return View(new CreateDiscussionPostViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePost(CreateDiscussionPostViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ClassId = viewModel.ClassId;
            return View(viewModel);
        }

        var userId = _userManager.GetUserId(User);
        var post = new DiscussionPost
        {
            Title = viewModel.Title,
            Content = viewModel.Content,
            UserId = userId,
            ClassId = viewModel.ClassId
        };

        _context.DiscussionPosts.Add(post);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Post created successfully!";
        return RedirectToAction(nameof(Index), new { classId = viewModel.ClassId });
    }

    [HttpGet]
    public IActionResult CreateReply(int postId)
    {
        var post = _context.DiscussionPosts.FirstOrDefault(p => p.Id == postId);
        if (post == null)
        {
            TempData["Error"] = "Post not found.";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.PostId = postId;
        return View(new CreateDiscussionReplyViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateReply(CreateDiscussionReplyViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.PostId = viewModel.PostId;
            return View(viewModel);
        }

        var post = await _context.DiscussionPosts.FindAsync(viewModel.PostId);
        if (post == null)
        {
            TempData["Error"] = "Post not found.";
            return RedirectToAction(nameof(Index));
        }

        var reply = new DiscussionReply
        {
            Content = viewModel.Content,
            UserId = _userManager.GetUserId(User),
            PostId = viewModel.PostId
        };

        _context.DiscussionReplies.Add(reply);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Reply added successfully!";
        return RedirectToAction(nameof(Index), new { classId = post.ClassId });
    }
}