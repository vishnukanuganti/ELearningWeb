using ELearningWeb.Data;
using ELearningWeb.Models;
using ELearningWeb.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningWeb.Controllers
{
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(DiscussionPostViewModel postViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid post data." });
            }

            var post = new DiscussionPost
            {
                ClassId = postViewModel.ClassId,
                UserId = _userManager.GetUserId(User),
                Content = postViewModel.Content,
                PostedAt = DateTime.Now
            };

            try
            {
                _context.DiscussionPosts.Add(post);
                await _context.SaveChangesAsync();

                // Return the new post as a view model for the client to append
                var newPost = new DiscussionPostViewModel
                {
                    Id = post.Id,
                    ClassId = post.ClassId,
                    UserId = post.UserId,
                    UserName = (await _userManager.GetUserAsync(User)).FullName,
                    Content = post.Content,
                    PostedAt = post.PostedAt
                };

                return Json(new { success = true, post = newPost });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error creating post: {ex.Message}" });
            }
        }

        [HttpGet]
        public IActionResult GetPosts(int classId, DateTime? after)
        {
            var query = _context.DiscussionPosts
                .Include(dp => dp.User)
                .Where(dp => dp.ClassId == classId);

            if (after.HasValue)
            {
                query = query.Where(dp => dp.PostedAt > after.Value);
            }

            var posts = query
                .OrderBy(dp => dp.PostedAt)
                .Select(dp => new DiscussionPostViewModel
                {
                    Id = dp.Id,
                    ClassId = dp.ClassId,
                    UserId = dp.UserId,
                    UserName = dp.User.FullName,
                    Content = dp.Content,
                    PostedAt = dp.PostedAt
                })
                .ToList();

            return Json(new { success = true, posts });
        }
    }
}