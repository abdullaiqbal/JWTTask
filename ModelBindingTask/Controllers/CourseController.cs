using Microsoft.AspNetCore.Mvc;
using ModelBindingTask.Data;
using ModelBindingTask.Models;

namespace ModelBindingTask.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _Context;

        public CourseController(ApplicationDbContext context)
        {
            _Context = context;
        }

        /// <summary>
        /// Get Secion of course
        /// </summary>
        /// <returns></returns>
        ///  /// Query String Method

        [HttpGet]
        public IActionResult Index()
        {
            var courses = _Context.Course.ToList();
            return View(courses);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var course = _Context.Course.Where(x => x.Id == id).FirstOrDefault();
            return View(course);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = _Context.Course.Where(x => x.Id == id).FirstOrDefault();
            return View(course);
        }

        public IActionResult Delete(int id)
        {
            var course = _Context.Course.Where(x => x.Id == id).FirstOrDefault();
            return View(course);
        }

        /// <summary>
        /// Post Secion of course
        /// </summary>
        /// <returns></returns>
       


        //[HttpPost]
        //public IActionResult Create(Course model)
        //{
        //    _Context.Course.Add(model);
        //    _Context.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public IActionResult Edit(Course model)
        {
            _Context.Course.Update(model);
            _Context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(Course model)
        {
            _Context.Course.Remove(model);
            _Context.SaveChanges();
            return RedirectToAction("Index");
        }

        //Model Binding: From Form
        //[HttpPost]
        //public IActionResult Create(IFormCollection formdata)
        //{
        //    Course course = new Course
        //    {
        //        CName = formdata["CName"]
        //    };
        //    //course.CName = formdata["CName"];
        //    _Context.Course.Add(course);
        //    _Context.SaveChanges();
        //    return RedirectToAction("Index");
        //}


        //[HttpPost]
        //public IActionResult Create(IFormCollection formdata)
        //{
        //    Course course = new Course
        //    {
        //        CName = formdata["CName"]
        //    };
        //    //course.CName = formdata["CName"];
        //    _Context.Course.Add(course);
        //    _Context.SaveChanges();
        //    return RedirectToAction("Index");
        //}


        //Model Binding: From Form
        [HttpPost]
        public IActionResult Create([FromForm]Course formdata)
        {
            
            _Context.Course.Add(formdata);
            _Context.SaveChanges();
            return RedirectToAction("Index");
        }

        //Model Binding: From Route
        [HttpPost("/{CName}")]
        public IActionResult CreateFromRoute([FromRoute]Course course)
        {
            //course.CName = formdata["CName"];
            _Context.Course.Add(course);
            _Context.SaveChanges();
            return RedirectToAction("Index");
        }

        //Model Binding: From Body
        [HttpPost]
        public ActionResult CreateFromBody([FromBody]Course course)
        {
            //course.CName = formdata["CName"];
            _Context.Course.Add(course);
            _Context.SaveChanges();
            return RedirectToAction("Index");
        }


        //Model Binding: From Query
        [HttpPost]
        public ActionResult CreateFromQuery([FromQuery] Course course)
        {
            //course.CName = formdata["CName"];
            _Context.Course.Add(course);
            _Context.SaveChanges();
            return RedirectToAction("Index");
        }


        //Model Binding: From Header
        [HttpGet]
        public ActionResult CreateFromHeader([FromHeader]Course course)
        {
            //course.CName = formdata["CName"];
            //_Context.Course.Add(course);
            //_Context.SaveChanges();

            var CourseName = _Context.Course.Where(x=>x.Id== course.Id).FirstOrDefault();
            return RedirectToAction("Index");
        }





        //    private readonly Action<IRouteBuilder> GetRoutes =
        //routes =>
        //{
        //    routes.MapRoute(
        //        name: "custom",
        //        template: "{language=fr-FR}/{controller=Home}/{action=Index}/{id?}/{name?}");

        //    routes.MapRoute(
        //        name: "default",
        //        template: "{controller=Home}/{action=Index}/{id?}");
        //};
    }
}
