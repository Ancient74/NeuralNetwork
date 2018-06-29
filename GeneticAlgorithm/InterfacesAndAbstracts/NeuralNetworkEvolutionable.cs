using System;
using MathNet.Numerics.LinearAlgebra;


namespace GeneticAlgorithm
{
    abstract public class NeuralNetworkEvolutionable : NeuralNetwork.NeuralNetwork, IEvolutionable<Matrix<float>>
    {
        public DNA<Matrix<float>> DNA
        {
            get
            {
                return new DNA<Matrix<float>>(WeightsIH, WeightsHO, BiasH, BiasO);
            }

            set
            {
                WeightsIH = value[0];
                WeightsHO = value[1];
                BiasH = value[2];
                BiasO = value[3];
            }
        }

        public NeuralNetworkEvolutionable(int InputN = 1, int HiddenN = 1, int OutputN = 1) : base(InputN, HiddenN, OutputN) { }

        public abstract bool IsReady { get; set; }

        public DNA<Matrix<float>> Crossover(DNA<Matrix<float>> parentA, DNA<Matrix<float>> parentB)
        {
            Random r = new Random();
            Matrix<float>[] Genes = new Matrix<float>[parentA.Length];           
            if (parentA.Length != parentB.Length) return null;
            for (int i = 0; i < parentA.Length; i++)
            {
                Matrix<float> ChildGene = Matrix<float>.Build.Dense(parentA[i].RowCount, parentA[i].ColumnCount);
                for (int j = 0; j < parentA[i].RowCount; j++)
                {
                    for (int k = 0; k < parentA[i].ColumnCount; k++)
                    {
                        if (r.NextDouble() < 0.5)
                            ChildGene[j, k] = parentA[i][j, k];
                        else
                            ChildGene[j, k] = parentB[i][j, k];
                    }
                }
                Genes[i] = ChildGene;

            }
            return new DNA<Matrix<float>>(Genes);     
        }

        public abstract float EvaluateFitness();

        public void MutateAll(float MutationRate)
        {
            foreach(Matrix<float> d in DNA)
            {
                MutateGene(d,MutationRate);
            }
        }

        public void MutateGene(Matrix<float> Gene, float MutationRate)
        {
            Random random = new Random();
            for (int i = 0; i < Gene.RowCount; i++)
            {
                for (int j = 0; j < Gene.ColumnCount; j++)
                {
                    if (random.NextDouble() < MutationRate)
                    {
                        Gene[i, j] *= 1 + (float)((random.NextDouble() - 0.5) * 3 + (random.NextDouble() - 0.5));
                    }
                }
            }
        }

    }
}
