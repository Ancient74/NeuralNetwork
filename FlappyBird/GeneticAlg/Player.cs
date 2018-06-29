using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithm;
using MathNet.Numerics.LinearAlgebra;

namespace FlappyBird.GeneticAlg
{
    class Player : NeuralNetworkEvolutionable, IVisualisationable<Matrix<float>>
    {
        public List<Pipe> pipes { get; }
        public Bird bird { get; }
        public int Width { get; }
        public int Height { get; }      
        public float Distance => bird.Distance;
        public float Fitness { get; set; }
        public bool IsWinner { get; set; } = false;
        public override bool IsReady { get { return bird.IsDead; }set { bird.IsDead = value; } }
        public int Score { get { return bird.Score; }set { bird.Score = value; } }
        public float Xnorm => (GetNearPipe().X+GetNearPipe().PipeWidth - bird.Position.X) / Width;
        public float YnormTop => (GetNearPipe().PointRect.Top - bird.Position.Y) / Height;
        public float YnormBot => (GetNearPipe().PointRect.Bottom - bird.Position.Y) / Height;   

        public bool IsCollide(Rectangle b)
        {
            return bird.IsCollide(b);
        }


        public void SetBirdTexture(Texture2D texture)
        {
            bird.BirdTexture = texture;
        }

        public Player(Player copy)
        {
            this.pipes = copy.pipes;
            this.Width = copy.Width;
            Height = copy.Height;
            bird = new Bird(copy.Width, copy.Height, copy.bird.Size.X, copy.bird.Size.Y);
            SetBirdTexture(copy.bird.BirdTexture);           
            this.LearningRate = copy.LearningRate;
            this.WeightsHO = Matrix<float>.Build.DenseOfMatrix(copy.WeightsHO);
            this.WeightsIH = Matrix<float>.Build.DenseOfMatrix(copy.WeightsIH);
            this.BiasH = Matrix<float>.Build.DenseOfMatrix(copy.BiasH);
            this.BiasO = Matrix<float>.Build.DenseOfMatrix(copy.BiasO);
        }
        /// <summary>
        /// Create player
        /// </summary>
        /// <param name="args">1,2,3 = input, hidden, output neurons, 4 - bird texture, 5,6 - screen width and height,7,8 - bird width and height, 9 pipes list </param>
        public Player(params object[]args):base((int)args[0], (int)args[1], (int)args[2])
        {
            this.pipes = (List<Pipe>)args[8];
            Width = (int)args[4];
            Height = (int)args[5];
            bird = new Bird(Width, Height, (int)args[6], (int)args[7]);    
            bird.BirdTexture = (Texture2D)args[3];
        }
        public void Flap()
        {            
                if (FeedForward(new float[] {Xnorm,YnormTop,YnormBot})[0] > 0.5f)
                {
                    bird.Flap();
                }
        }

        public void Update(GameTime gameTime)
        {
            Flap();
            if (Score > 0)
                Fitness += (float)gameTime.TotalGameTime.Milliseconds;
            else {Fitness+= (float)gameTime.TotalGameTime.Milliseconds/100 + 10/(1 + Math.Abs(Vector2.Distance(bird.Position,GetNearPipe().PointRect.Location.ToVector2()))); }
            for (int i = 0; i < pipes.Count; i++)
            {
                if (!IsReady && (IsCollide(pipes[i].Bottom) || IsCollide(pipes[i].Top)) || bird.Position.Y+bird.Size.Y>=Height) //|| bird.Position.Y<=0)
                {
                    IsReady = true;
                }
            }
            bird.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            bird.Draw(spriteBatch);
        }

        public Pipe GetNearPipe()
        {
            for (int i = 0; i < pipes.Count; i++)
            {
                if (!pipes[i].Passed) return pipes[i];
            }

            return null;
        }

        public override float EvaluateFitness()
        {
            return Fitness;
        }
    }
}
