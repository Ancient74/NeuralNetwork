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
        public List<Pipe> Pipes { get; }
        public Bird Bird { get; }
        public int Width { get; }
        public int Height { get; }      
        public float Distance => Bird.Distance;
        public float Fitness { get; set; }
        public bool IsWinner { get; set; } = false;
        public override bool IsReady { get { return Bird.IsDead; }set { Bird.IsDead = value; } }
        public int Score { get { return Bird.Score; }set { Bird.Score = value; } }
        public float Xnorm => (GetNearPipe().X+GetNearPipe().PipeWidth - Bird.Position.X) / Width;
        public float YnormTop => (GetNearPipe().PointRect.Top - Bird.Position.Y) / Height;
        public float YnormBot => (GetNearPipe().PointRect.Bottom - Bird.Position.Y) / Height;   

        public bool IsCollide(Rectangle b)
        {
            return Bird.IsCollide(b);
        }


        public void SetBirdTexture(Texture2D texture)
        {
            Bird.BirdTexture = texture;
        }

        public Player(Player copy)
        {
            this.Pipes = copy.Pipes;
            this.Width = copy.Width;
            Height = copy.Height;
            Bird = new Bird(copy.Width, copy.Height, copy.Bird.Size.X, copy.Bird.Size.Y);
            SetBirdTexture(copy.Bird.BirdTexture);           
            this.LearningRate = copy.LearningRate;
            this.WeightsHO = Matrix<float>.Build.DenseOfMatrix(copy.WeightsHO);
            this.WeightsIH = Matrix<float>.Build.DenseOfMatrix(copy.WeightsIH);
            this.BiasH = Matrix<float>.Build.DenseOfMatrix(copy.BiasH);
            this.BiasO = Matrix<float>.Build.DenseOfMatrix(copy.BiasO);
        }
        /// <summary>
        /// Create player
        /// </summary>
        public Player(int input,int hidden,int output,Texture2D birdText,int w,int h,int birdW,int birdH,List<Pipe>pipes):base(input, hidden, output)
        {
            this.Pipes = pipes;
            Width = w;
            Height = h;
            Bird = new Bird(Width, Height, birdW, birdH)
            {
                BirdTexture = birdText
            };
        }
        public void Flap()
        {            
                if (FeedForward(new float[] {Xnorm,YnormTop,YnormBot})[0] > 0.5f)
                {
                    Bird.Flap();
                }
        }

        public void Update(GameTime gameTime)
        {
            Flap();
            if (Score > 0)
                Fitness += (float)gameTime.TotalGameTime.Milliseconds;
            else {Fitness+= (float)gameTime.TotalGameTime.Milliseconds/100 + 10/(1 + Math.Abs(Vector2.Distance(Bird.Position,GetNearPipe().PointRect.Location.ToVector2()))); }
            for (int i = 0; i < Pipes.Count; i++)
            {
                if (!IsReady && (IsCollide(Pipes[i].Bottom) || IsCollide(Pipes[i].Top)) || Bird.Position.Y+Bird.Size.Y>=Height) //|| bird.Position.Y<=0)
                {
                    IsReady = true;
                }
            }
            Bird.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Bird.Draw(spriteBatch);
        }

        public Pipe GetNearPipe()
        {
            for (int i = 0; i < Pipes.Count; i++)
            {
                if (!Pipes[i].Passed) return Pipes[i];
            }

            return null;
        }

        public override float EvaluateFitness()
        {
            return Fitness;
        }
    }
}
