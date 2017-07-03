using System.Collections.Generic;
using System.Linq;

namespace Api
{
    public class Scheduler
    {
        //public List<Timetable> SolveCourseRegistraionCSP(ConstraintsSatisfactionProblemSo csp) 
        public List<Timetable> GetAllSolutions(ConstraintsCollection constraints, IList<Course> courses)
        {
            //var allCoursesClassTypes = new List<ClassType>();

            //foreach (var course in courses)
            //{
            //    foreach (var courseClassType in course.ClassTypes)
            //    {
            //        allCoursesClassTypes.Add(courseClassType);
            //    }
            //}


            //var orderedVariables = ApplyMrvHeuristic(allCoursesClassTypes);

            ////RecursiveBt(orderedVariables, solutions, domains);
            //var timetableTree = new TimetableTree(orderedVariables.Count);
            //var level = 0;
            //foreach (var variable in orderedVariables)
            //{
            //    var partialAssignments = timetableTree.GetNodesAtLevel(level);
            //    if (level == 10)
            //    {

            //    }
            //    if (level == 18)
            //    {

            //    }
            //    if (partialAssignments.Any() == false)
            //    {
            //        break;
            //    }


            //    foreach (var partialAssignment in partialAssignments)
            //    {
            //        var timetable = partialAssignment.CreateTimetableFromNode();
            //        foreach (var courseTypeGroup in variable.Groups)
            //        {
            //            if (constraints.IsConsistent(timetable, courseTypeGroup))
            //            {
            //                //timetableTree.AddChildrenToLevelNodes(level, typeGroup);
            //                timetableTree.AddToChildren(partialAssignment, courseTypeGroup);
            //                // leaf.AddToChildren(Group);
            //            }
            //        }

            //    }
            //    level++;
            //}

            //return timetableTree.LeafsToTimetables();
            return null;
        }

        public List<Timetable> SolveSssp(IList<Course> courses, ConstraintsCollection constraints)
        {
            var allClassTypes = GetAllClassTypes(courses);
            var variables = ApplyMrvHeuristic(allClassTypes);

            //allSolutions = {}
            var allSolutions = new List<Timetable>();

            //BT(Variables, {}, Domains, allSolutions)
            BacktrackingAllSolutions(variables, new Timetable(), allSolutions, 0,  constraints);

            //ToDo: Rate(allSolutions)

            SortByRate(allSolutions);

            //Return allSolutions
            return allSolutions;
        }

        private void SortByRate(List<Timetable> allSolutions)
        {
            allSolutions.Sort( (x, y) => (int)(x.Rating - y.Rating));
        }

        public void BacktrackingAllSolutions(List<ClassType> variables, Timetable instantiation, List<Timetable> allSolutions, int index, ConstraintsCollection constraints)
        {
            if (variables.Count == index)
            {
                instantiation.Rate(constraints);
                allSolutions.Add(instantiation);
                return;
            }

            var variable = variables[index++];
            var domain = variable.Groups;

            foreach (var value in domain)
            {
                var tempTimetable = instantiation.Copy();
                if (constraints.IsConsistent(tempTimetable, value))
                {
                    BacktrackingAllSolutions(variables, tempTimetable, allSolutions, index, constraints);
                }
            }
        }

        private IEnumerable<ClassType> GetAllClassTypes(IList<Course> courses)
        {
            return courses.SelectMany(course => course.ClassTypes); 
        }

        private List<ClassType> ApplyMrvHeuristic(IEnumerable<ClassType> allCoursesClassTypeGroups)
        {
            return allCoursesClassTypeGroups.OrderBy(x => x.Groups.Count).ToList();
        }
    }
}