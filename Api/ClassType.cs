using System.Collections.Generic;
using System.Diagnostics;

namespace Api
{
    [DebuggerDisplay("{Type}")]
    public class ClassType
    {
        public ClassType(string classType, List<Group> groups = null)
        {
            Type = classType;
            if (groups == null)
            {
                groups = new List<Group>();
            }

            Groups = groups;
        }

        public string Type
        {
            get; set;
        }
        public List<Group> Groups
        {
            get;
        }

        public Course Course { get; set; }

        public override string ToString()
        {
            return this.Type;
        }
    }
}