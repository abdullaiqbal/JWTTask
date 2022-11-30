using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelBindingTask.Models;
using System.Security.Claims;

namespace ModelBindingTask.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [Authorize(Roles =UserRoles.Admin)]
        [HttpGet("getWithAny")]
        public IActionResult GetWithAny()
        {
            return Ok(new { Message = $"Hello to Code Maze {GetUsername()}" });
        }
        [HttpGet("getWithSecondJwt")]
        public IActionResult GetWithSecondJwt()
        {
            return Ok(new { Message = $"Hello to Code Maze {GetUsername()}" });
        }
        private string? GetUsername()
        {
            return HttpContext.User.Claims
                .Where(x => x.Type == ClaimTypes.Name)
                .Select(x => x.Value)
                .FirstOrDefault();
        }


        
       
    }
}
