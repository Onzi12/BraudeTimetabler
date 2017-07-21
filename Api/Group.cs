using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Api
{
    [DebuggerDisplay("{Id} {Lecturer}")]
    public class Group
    {
        public Group()
        { }
        public Group(string id, string lecturer, GroupEvent[] events)
        {
            
            this.Id = id;
            Lecturer = lecturer;
            Events = events;
        }

        public Group(string id, string lecturer, GroupEvent groupEvent) : this(id, lecturer, new[] { groupEvent })
        {
        }

        public string Id
        {
            get; set;
        }
        public string Lecturer
        {
            get; set;
        }
        public bool IsMandatory
        {
            get; set;
        }
        public GroupEvent[] Events
        {
            get; set;
        }

        public ClassType ClassType
        {
            get; set;
        }

        public Course Course
        {
            get; set;
        }

        public override string ToString()
        {
            return this.Lecturer.Length <= 20 ? Lecturer : Lecturer.Substring(0, 17)+"..." ;
        }
    }
}