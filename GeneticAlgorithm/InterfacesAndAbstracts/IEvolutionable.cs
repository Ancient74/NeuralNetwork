
namespace GeneticAlgorithm
{
    public interface IEvolutionable<T> 
    {
        /// <summary>
        /// Apply crossover between 2 parents
        /// </summary>
        /// <param name="parentA">First parent</param>
        /// <param name="parentB">Second parent</param>
        /// <returns>Return parent`s "child"</returns>
        DNA<T> Crossover(DNA<T> parentA, DNA<T>parentB);

        DNA<T> DNA { get; set; }

        /// <summary>
        /// Apply mutation to DNA
        /// </summary>
        /// <param name="MutationRate">Rate of mutation</param>
        void MutateAll(float MutationRate);

        /// <summary>
        /// Apply mutation to some gene in DNA
        /// </summary>
        /// <param name="Gene">Gene that will be mutated</param>
        /// <param name="MutationRate">Rate of mutation</param>
        void MutateGene(T Gene, float MutationRate);

        float EvaluateFitness();

        bool IsReady { get; set; }

    }
}
