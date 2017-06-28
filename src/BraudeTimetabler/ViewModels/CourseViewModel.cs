namespace BraudeTimetabler.Models
{
    public class CourseViewModel
    {
        public CourseViewModel(string id, string name)
        {
            this.Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }
    }
}