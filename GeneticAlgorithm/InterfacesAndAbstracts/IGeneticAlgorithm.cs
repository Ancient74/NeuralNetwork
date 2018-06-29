using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public interface IGeneticAlgorithm
    {
        float MutationRate { get; set; }
        int CurrentGeneration { get; set; }
        Type TypeOfEvolutionableObject { get;  set; }
        object[] InstanceArgs { get;  set; }
        int Winners { get; set; }

        float[] Fitnesses { get; set; }

        void Evolve();
        void Selection();
        bool TryToGo();
    }
}
