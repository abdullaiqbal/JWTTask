using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ModelBindingTask.Models
{
    public class User
    {
        [FromBody]
        public string? Username { get; set; }
        [FromBody]
        public string? UserPassword { get; set; }
    }
}
