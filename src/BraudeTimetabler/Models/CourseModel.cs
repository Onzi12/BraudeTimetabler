namespace BraudeTimetabler.Models
{
    public class CourseModel
    {
        public CourseModel(string id, string name)
        {
            this.Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }
    }
}