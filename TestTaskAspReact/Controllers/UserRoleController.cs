using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestTaskAspReact.Data;

namespace TestTaskAspReact.Controllers
{
    [Authorize]
    [ApiController]
    [Route("")]
    public class UserRoleController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;
        public UserRoleController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;

        }

        [HttpPost("users/{id}/roles/{roleId}")]
        public async Task<ActionResult> Post(string id, string roleId)
        {
            var userExists = await applicationDbContext.Users.AnyAsync(x => x.Id == id);
            if (!userExists)
            {
                return new NotFoundResult();
            }

            var roleExists = await applicationDbContext.Roles.AnyAsync(x => x.Id == roleId);
            if (!roleExists)
            {
                return new NotFoundResult();
            }

            var userRoleExists = await applicationDbContext.UserRoles.AnyAsync(x => x.UserId == id && x.RoleId == roleId);
            if (userRoleExists)
            {
                return new ConflictResult();
            }

            var userRole = new IdentityUserRole<string>();
            userRole.UserId = id;
            userRole.RoleId = roleId;

            applicationDbContext.UserRoles.Add(userRole);
            await applicationDbContext.SaveChangesAsync();

            return new OkResult();
        }
    }
}
