using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public class GeneticAlgorithm<T,V> : IGeneticAlgorithm<T,V> where V : IEvolutionable<T>
    {
        public List<V> Population { get; set; }

        public float MutationRate { get; set; }
        public int CurrentGeneration { get; set; }
        public Func<V> CreateFunc { get; set; }
        public int Winners { get; set; }
        public float[] Fitnesses { get; set; }    

        public GeneticAlgorithm(int populationCount, float mutationRate, int winnersCount, Func<V>CreateFunc)
        {
                this.MutationRate = mutationRate;
                Population = new List<V>();
                for (int i = 0; i < populationCount; i++)
                {
                    Population.Add(CreateFunc());
                }
                
                Winners = winnersCount;
                Fitnesses = new float[populationCount];
        }

        /// <summary>
        /// Evolve population 
        /// If population is not ready => return
        /// </summary>
        public bool TryToGo()
        {
            foreach (var p in Population)
            {
                if (!p.IsReady) return false;
            }
            Selection();
            Evolve();
            return true;
        }
        /// <summary>
        /// Evolve population(apply crossover and mutation)
        /// </summary>
        public void Evolve()
        {
            var newPopulation = new List<V>();
            for (int i = 0; i < Winners; i++)
            {
                newPopulation.Add(Population[i]);
            }
            Random r = new Random();
            for (int i = Winners; i < Population.Count; i++)
            {
                V child = CreateFunc();
                if (i == Winners)
                {
                    var ParentA = Population[0];
                    var ParentB = Population[1];
                    child.DNA = ParentA.Crossover(ParentA.DNA, ParentB.DNA);

                }
                else if (i < Population.Count - 2)
                {
                    var ParentA = Population[r.Next(0, Winners)];
                    var ParentB = Population[r.Next(0, Winners)];
                    child.DNA = ParentA.Crossover(ParentA.DNA, ParentB.DNA);
                }
                else
                {
                    child = Population[r.Next(0, Population.Count)];
                }
                child.MutateAll(MutationRate);
                newPopulation.Add(child);

            }
            Population.Clear();
            Population = newPopulation;
            CurrentGeneration++;

        }
        /// <summary>
        /// Calculate fitness and sort population by those values 
        /// </summary>
        public void Selection()
        {
            for (int i = 0; i < Population.Count; i++)// get fitness of each member of the population
            {
                Fitnesses[i] = Population[i].EvaluateFitness();
            }
            for (int i = 0; i < Population.Count - 1; i++)//sort by fitness value
            {
                for (int j = i + 1; j < Population.Count; j++)
                {
                    if (Fitnesses[i] < Fitnesses[j])
                    {
                        var temp = Fitnesses[i];
                        Fitnesses[i] = Fitnesses[j];
                        Fitnesses[j] = Fitnesses[i];
                        var poptemp = Population[i];
                        Population[i] = Population[j];
                        Population[j] = poptemp;
                    }
                }
            }
            
        }


    }
}
