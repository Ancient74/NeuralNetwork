using System;
using MathNet.Numerics.LinearAlgebra;


namespace NeuralNetwork
{
    public class NeuralNetwork
    {
        public int InputNeurons => WeightsIH.RowCount;
        public int HiddenNeurons => WeightsIH.ColumnCount;
        public int OutputNeurons => WeightsHO.RowCount;

        public float LearningRate { get; set; }

        public Matrix<float> WeightsIH { get; set; }
        public Matrix<float> WeightsHO { get; set; }
        public Matrix<float> BiasH { get; set; }
        public Matrix<float> BiasO { get; set; }

        /// <summary>
        /// Create a new neural network with 3 layers: input, hidden and output
        /// </summary>
        /// <param name="InputNeurons">Number of neurons that input layer contains</param>
        /// <param name="HiddenNeurons">Number of neurons that hidden layer contains</param>
        /// <param name="OutputNeurons">Number of neurons that output layer contains</param>
        /// <param name="LearningRate">Learning rate for the supervised learning</param>
        /// <param name="MutationRate">Mutation rate for some mutations</param>
        public NeuralNetwork(int InputNeurons = 1, int HiddenNeurons = 1, int OutputNeurons = 1, float LearningRate=0.5f)//, float MutationRate=0.01f)
        {
            if (InputNeurons < 1) InputNeurons = 1;
            if (HiddenNeurons < 1) HiddenNeurons = 1;
            if (OutputNeurons < 1) OutputNeurons = 1;
            if (LearningRate <= 0) LearningRate = 0.5f;
            //if (MutationRate <= 0) MutationRate = 0.01f;

            this.LearningRate = LearningRate;
            //this.MutationRate = MutationRate;

            WeightsIH = Matrix<float>.Build.Random(HiddenNeurons, InputNeurons);
            WeightsHO = Matrix<float>.Build.Random(OutputNeurons, HiddenNeurons);
            BiasH = Matrix<float>.Build.Random(HiddenNeurons, 1);
            BiasO = Matrix<float>.Build.Random(OutputNeurons, 1);
        }

        /// <summary>
        /// Create a copy of neural network with the same amaunt of neurons, same widths, learning rate and mutation rate
        /// </summary>
        /// <param name="copy">Some instance of neural network</param>
        public NeuralNetwork(NeuralNetwork copy)
        {
            this.LearningRate = copy.LearningRate;
            this.WeightsHO = Matrix<float>.Build.DenseOfMatrix(copy.WeightsHO);
            this.WeightsIH = Matrix<float>.Build.DenseOfMatrix(copy.WeightsIH);
            this.BiasH = Matrix<float>.Build.DenseOfMatrix(copy.BiasH);
            this.BiasO = Matrix<float>.Build.DenseOfMatrix(copy.BiasO);
        }

        /// <summary>
        /// Predict some output based on input data
        /// </summary>
        /// <param name="Input">Input array that NN use to make some output data</param>
        /// <returns>Retuns data that NN thinks is correct</returns>
        public virtual float[] FeedForward(float[] Input)
        {
            var vectorInput = Vector<float>.Build.DenseOfArray(Input);
            var hidden = WeightsIH.Multiply(vectorInput.ToColumnMatrix());
            hidden = hidden.Add(BiasH);
            hidden = hidden.Map(ActivationFunction);
            
            var output = WeightsHO.Multiply(hidden);
            output = output.Add(BiasO);
            output = output.Map(ActivationFunction);
         
            return output.ToColumnArrays()[0];
        }

        /// <summary>
        /// Activation function of the NN
        /// </summary>
        /// <param name="x">Argument of the function</param>
        /// <returns>Returns some value between its domain</returns>
        protected virtual float ActivationFunction(float x)
        {
            return 1f / (float)(1 + Math.Exp(-x));
        }

        /// <summary>
        /// Supervised learning based on gradient descent algorithm
        /// </summary>
        /// <param name="Input">Input data, the answer of which we know</param>
        /// <param name="Answer">The correct answer corresponds to the input data</param>
        public virtual void Teach(float[] Input, float[] Answer)
        {
            //feed forward part
            var vectorInput = Vector<float>.Build.DenseOfArray(Input);
            var hidden = WeightsIH.Multiply(vectorInput.ToColumnMatrix());
            hidden = hidden.Add(BiasH);
            hidden = hidden.Map(ActivationFunction);

            var output = WeightsHO.Multiply(hidden);
            output = output.Add(BiasO);
            output = output.Map(ActivationFunction);
            //done
            var targets = Matrix<float>.Build.DenseOfRowArrays(Answer);
            var errors = targets.Subtract(output);
           

            output = output.Map((x) => { return x * (1 - x); });
            output = errors.PointwiseMultiply(output);
            output = output * LearningRate;

            BiasO = BiasO.Add(output);

            var WeightHODealta = output * hidden.Transpose();
            WeightsHO = WeightsHO.Add(WeightHODealta);

            var hiddenErrors = WeightsHO.Transpose() * errors;

            hidden = hidden.Map((x) => { return x * (1 - x); });
            hidden =hiddenErrors.PointwiseMultiply(hidden);
            hidden = hidden * LearningRate;

            BiasH = BiasH.Add(hidden);

            var WeightHIDelta = hidden * vectorInput.ToRowMatrix();
            WeightsIH = WeightsIH.Add(WeightHIDelta);
            
        }       

        /// <summary>
        /// Check if number of neurons of each layer is equal to another NN
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Equals(NeuralNetwork obj)
        {
            return InputNeurons == obj.InputNeurons && HiddenNeurons == obj.HiddenNeurons && OutputNeurons == obj.OutputNeurons;
        }
    }
}
