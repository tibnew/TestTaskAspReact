using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestTaskAspReact.Data;

namespace TestTaskAspReact.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;
        public HomeController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;

        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            var userData = applicationDbContext.Users.Select(x => new {Name = x.UserName, ProfilePicture = x.ProfilePicture})
                .ToList().Select(x =>
            {
                string? imgDataURL = null;
                if (x.ProfilePicture != null)
                {
                    string imreBase64Data = Convert.ToBase64String(x.ProfilePicture);
                    imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
                }
                return new User { Name = x.Name, ProfilePictureUrl = imgDataURL };
            }).ToArray();
            return userData;
        }
    }

    public class User
    {
        public string? Name { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
