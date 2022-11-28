namespace ModelBindingTask.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string SName { get; set; }
        public string program { get; set; }

        public ICollection<StudentCourse> StudentCourse { get; set; }
    }
}
