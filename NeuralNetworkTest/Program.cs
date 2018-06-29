using System;
using NeuralNetwork;
using System.Collections.Generic;

namespace NeuralNetworkTest
{
    class Program
    {
        static void Main(string[] args)
        {

            /*NeuralNetwork.NeuralNetwork nn = new NeuralNetwork.NeuralNetwork(2, 2, 1, 4);
            
            var dataset =  new[] {
                new
                {
                    Input = new float[] { 0f, 0f },
                    Answer = new float[] { 0 }
                },
                new
                {
                    Input = new float[] { 1f, 0f },
                    Answer = new float[] { 1 }
                },
                new
                {
                    Input = new float[] { 0f, 1f },
                    Answer = new float[] { 1 }
                },
                new
                {
                    Input = new float[] { 1f, 1f },
                    Answer = new float[] { 0 }
                }
            };
            Random r = new Random();
            for (int i = 0; i < 10000; i++)
            {
                int j = r.Next(0, 4);
                //nt.StochasticGradientDescent(4, dataset, 9, 2, 4, dataset);
            }

            Console.WriteLine(nn.FeedForward(dataset[0].Input)[0]);
            Console.WriteLine(nn.FeedForward(dataset[1].Input)[0]);
            Console.WriteLine(nn.FeedForward(dataset[2].Input)[0]);
            Console.WriteLine(nn.FeedForward(dataset[3].Input)[0]);*/

            Console.ReadKey();
        }
    }
}
