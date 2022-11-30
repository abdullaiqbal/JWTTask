using Microsoft.AspNetCore.Identity;

namespace ModelBindingTask.Models
{
    public class ApplicationUser : IdentityUser
    {
        //public String UserName { get; set; }
        public String Password { get; set; }
    }
}
