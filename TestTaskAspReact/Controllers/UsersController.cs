using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TestTaskAspReact.Contract;
using TestTaskAspReact.Data;

namespace TestTaskAspReact.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;
        public UsersController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            var userData = applicationDbContext.Users
                .Select(x => new {Id = x.Id, Name = x.UserName, ProfilePicture = x.ProfilePicture,
                    IsAdmin = applicationDbContext.UserRoles.Any(y => y.RoleId == Constants.AdminRoleId && y.UserId == x.Id)
                })
                .ToList()
                .Select(x =>
            {
                string? imgDataURL = null;
                if (x.ProfilePicture != null)
                {
                    string imreBase64Data = Convert.ToBase64String(x.ProfilePicture);
                    imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
                }
                return new User { Id = x.Id, Name = x.Name, ProfilePictureUrl = imgDataURL, IsAdmin = x.IsAdmin };
            }).ToArray();
            return userData;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            var user = await applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return new NotFoundResult();
            }

            applicationDbContext.Users.Remove(user);

            await applicationDbContext.SaveChangesAsync();

            return new NoContentResult();
        }

        [HttpGet("current")]
        public async Task<User> GetCurrentUser()
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserName = HttpContext.User.FindFirstValue(ClaimTypes.Name);

            return new User
            {
                Id = currentUserId,
                Name = currentUserName,
                IsAdmin = await applicationDbContext.UserRoles
                .AnyAsync(y => y.RoleId == Constants.AdminRoleId && y.UserId == currentUserId)
            };
        }
    }
}
