using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using C5;

namespace Api
{
    public class Scheduler
    {

        public IEnumerable<Timetable> SolveSssp(System.Collections.Generic.IList<Course> courses, ConstraintsCollection constraints)
        {
            var allClassTypes = GetAllClassTypes(courses);
            var variables = ApplyMrvHeuristic(allClassTypes);

            //allSolutions = MaxHeap
            var backtrackingSolutions = new IntervalHeap<Timetable>(200);

            // BT(Variables, {}, Domains, allSolutions)
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(20));
            var cancellationToken = cancellationTokenSource.Token;
            var backtrackingTask = Task.Factory.StartNew(
                () =>
                {
                    BacktrackingAllSolutions(variables, new Timetable(), backtrackingSolutions, 0, constraints, cancellationToken);
                    return backtrackingSolutions;
                }, cancellationToken);

            // Generic algorithm
            var geneticAlgorithmRunner = new TimetablerGeneticAlgorithmRunner(variables, constraints);
            var geneticTask = geneticAlgorithmRunner.RunAsync();

            Task.WaitAll(backtrackingTask);

            var timetables = new List<Timetable>();
            if (cancellationToken.IsCancellationRequested)
            {
                geneticTask.Wait();
                timetables.Add(geneticAlgorithmRunner.GeneticSolution);
            }

            if (backtrackingSolutions.Any())
            {
                timetables.AddRange(backtrackingSolutions);
            }

            var sortedSolutions = SortByRate(timetables);

            //Return allSolutions
            return sortedSolutions;
        }

        private IEnumerable<Timetable> SortByRate(IEnumerable<Timetable> allSolutions)
        {
            return allSolutions.OrderBy(x => x.Rating);
        }

        public void BacktrackingAllSolutions(List<ClassType> variables, Timetable instantiation, IPriorityQueue<Timetable> allSolutions, int index, ConstraintsCollection constraints, CancellationToken cancellationToken)
        {
            if (variables.Count == index)
            {
                // check constraints who can be calculated only after timetable is fully instantiated
                if (!constraints.IsConsistent(instantiation, true))
                {
                    return;
                }

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
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                var tempTimetable = instantiation.Copy();
                tempTimetable.Add(value);
                if (constraints.IsConsistent(tempTimetable, false))
                {
                    BacktrackingAllSolutions(variables, tempTimetable, allSolutions, index, constraints, cancellationToken);
                }
            }
        }

        private IEnumerable<ClassType> GetAllClassTypes(System.Collections.Generic.IList<Course> courses)
        {
            return courses.SelectMany(course => course.ClassTypes).Where(classType => classType.IsMandatory); 
        }

        private List<ClassType> ApplyMrvHeuristic(IEnumerable<ClassType> allCoursesClassTypeGroups)
        {
            return allCoursesClassTypeGroups.OrderBy(x => x.Groups.Count).ToList();
        }
    }
}