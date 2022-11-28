using Microsoft.AspNetCore.Mvc;

namespace ModelBindingTask.Models
{
    public class Course
    {
        [FromHeader]
        public int Id { get; set; }
        [FromHeader]
        public string CName { get; set; }
        public ICollection<StudentCourse> StudentCourse { get; set; }
    }
}
