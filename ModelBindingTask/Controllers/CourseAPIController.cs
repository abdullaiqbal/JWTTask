using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelBindingTask.Data;
using ModelBindingTask.Models;

namespace ModelBindingTask.Controllers
{
    [Route("api/c")]
    [ApiController]
    public class CourseAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;

        public CourseAPIController(ApplicationDbContext context)
        {
            _Context = context;
        }
        //Model Binding: From Route
        [HttpPost("c/{CName}")]
        public IActionResult CreateFromRoute([FromRoute] Course course)
        {
            //course.CName = formdata["CName"];
            _Context.Course.Add(course);
            _Context.SaveChanges();
            return Ok();
        }



    }
}
