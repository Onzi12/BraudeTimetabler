using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using C5;

namespace Api
{
    public class Scheduler
    {
        //public List<Timetable> SolveCourseRegistraionCSP(ConstraintsSatisfactionProblemSo csp) 
        public List<Timetable> GetAllSolutions(ConstraintsCollection constraints, System.Collections.Generic.IList<Course> courses)
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

        public IEnumerable<Timetable> SolveSssp(System.Collections.Generic.IList<Course> courses, ConstraintsCollection constraints)
        {
            var allClassTypes = GetAllClassTypes(courses);
            var variables = ApplyMrvHeuristic(allClassTypes);

            //allSolutions = MaxHeap
            var allSolutions = new IntervalHeap<Timetable>(200);
            
            //TODO: Genetic algorithm

            //BT(Variables, {}, Domains, allSolutions)
            BacktrackingAllSolutions(variables, new Timetable(), allSolutions, 0,  constraints);

            //ToDo: Rate(allSolutions)

            var sortedSolutions = SortByRate(allSolutions);

            //Return allSolutions
            return sortedSolutions;
        }

        private IEnumerable<Timetable> SortByRate(IntervalHeap<Timetable> allSolutions)
        {
            return allSolutions.OrderBy(x => x.Rating);
        }

        public void BacktrackingAllSolutions(List<ClassType> variables, Timetable instantiation, IPriorityQueue<Timetable> allSolutions, int index, ConstraintsCollection constraints)
        {
            if (variables.Count == index)
            {
                var rate = instantiation.Rate(constraints);
                if (allSolutions.Count < 200)
                {
                    allSolutions.Add(instantiation);
                    return;
                }

                if (rate < allSolutions.FindMax().Rating)
                {
                    allSolutions.DeleteMax();
                    allSolutions.Add(instantiation);
                }

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

        private IEnumerable<ClassType> GetAllClassTypes(System.Collections.Generic.IList<Course> courses)
        {
            return courses.SelectMany(course => course.ClassTypes); 
        }

        private List<ClassType> ApplyMrvHeuristic(IEnumerable<ClassType> allCoursesClassTypeGroups)
        {
            return allCoursesClassTypeGroups.OrderBy(x => x.Groups.Count).ToList();
        }
    }
}