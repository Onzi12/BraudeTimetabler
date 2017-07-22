using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;

namespace Api
{
    public class TimetablerGeneticAlgorithmRunner
    {
        public IList<ClassType> Variables
        {
            get;
        }
        public ConstraintsCollection Constraints
        {
            get;
        }

        public TimetablerGeneticAlgorithmRunner(IList<ClassType> variables, ConstraintsCollection constraints)
        {
            Variables = variables;
            Constraints = constraints;
        }

        private Timetable ChromosomeToTimetable(IChromosome c)
        {
            var chromosome = c as FloatingPointChromosome;
            var values = chromosome.ToFloatingPoints();

            var groups = new List<Group>();
            for (int i = 0; i < Variables.Count; i++)
            {
                var group = Variables[i].Groups[(int)Math.Floor(values[i])];
                groups.Add(group);
            }
            
            return new Timetable(groups, c.Fitness * -1 ?? double.MaxValue);
        }

        private double TimetablerFitnessFunction(IChromosome c)
        {
            var timetable = ChromosomeToTimetable(c);

            if (Constraints.IsConsistent(timetable, true))
            {
                return timetable.Rate(Constraints) * -1;
            }

            return double.MinValue;
        }

        public Timetable GeneticSolution
        {
            get; private set;
        }

        public Task<Timetable> RunAsync()
        {
            var adamChromosome = CreateAdamChromosome();

            var population = new Population(50, 100, adamChromosome);

            var fitness = new FuncFitness(TimetablerFitnessFunction);

            var selection = new EliteSelection();

            var crossover = new UniformCrossover(0.5f);

            var mutation = new FlipBitMutation();

            var termination = new TimeEvolvingTermination(TimeSpan.FromSeconds(20));

            var geneticAlgorithm = new GeneticAlgorithm(
                population,
                fitness,
                selection,
                crossover,
                mutation)
            {
                Termination = termination
            };

            geneticAlgorithm.GenerationRan += (sender, e) =>
            {
                var bestChromosome = geneticAlgorithm.BestChromosome as FloatingPointChromosome;
                GeneticSolution = ChromosomeToTimetable(bestChromosome);
            };

            GeneticAlgorithm = geneticAlgorithm;

            return Task.Factory.StartNew(() =>
            {
                geneticAlgorithm.Start();
                return GeneticSolution;
            });
        }

        public GeneticAlgorithm GeneticAlgorithm
        {
            get; private set;
        }

        public bool RanToCompletion => GeneticAlgorithm.IsRunning == false;

        private IChromosome CreateAdamChromosome()
        {
            var count = Variables.Count;
            var variableMinValues = new double[count];
            var variableMaxValues = new double[count];
            var variableTotalBits = new int[count];
            var fractionBits = new int[count];
            for (var i = 0; i < count; i++)
            {
                var variable = Variables[i];
                variableMinValues[i] = 0d;
                variableMaxValues[i] = variable.Groups.Count - 0.0001;
                variableTotalBits[i] = GetBitLength(variable.Groups.Count);
                fractionBits[i] = 0;
            }

            return new FloatingPointChromosome(
                variableMinValues,
                variableMaxValues,
                variableTotalBits,
                fractionBits
            );
        }

        public static int GetBitLength(int bits)
        {
            int size = 0;

            for (; bits != 0; bits >>= 1)
                size++;

            return size;
        }
    }
}