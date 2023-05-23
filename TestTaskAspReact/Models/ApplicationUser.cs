using Microsoft.AspNetCore.Identity;

namespace TestTaskAspReact.Models
{
    public class ApplicationUser : IdentityUser
    {
        public byte[]? ProfilePicture { get; set; }
    }
}