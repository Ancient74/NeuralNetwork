using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using NeuralNetwork;
using FlappyBird.GeneticAlg;
using GeneticAlgorithm;
using System;

namespace FlappyBird
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Point birdSize;
        public Texture2D birdTexture;

        Bird testBird;

        GeneticAlgorithmUpdateable<MathNet.Numerics.LinearAlgebra.Matrix<float>,Player> GA;

        SpriteFont font;
       

        public List<Pipe> pipes;
        int pipeWidth;
        Texture2D pipeTexture;

        public int Width;
        public int Height;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Width = GraphicsDevice.Viewport.Width;
            Height = GraphicsDevice.Viewport.Height;
            birdSize = new Point(15, 15);      
            pipes = new List<Pipe>();
  
            testBird = new Bird(Width, Height, birdSize.X, birdSize.Y);
            pipeWidth = 100;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var s = birdSize.X * birdSize.Y;
            Color[] BirdColors = new Color[s];
            for (int i = 0; i < s; i++)
            {
                BirdColors[i] = Color.Orange;
            }
            birdTexture = new Texture2D(GraphicsDevice, birdSize.X, birdSize.Y);
            birdTexture.SetData<Color>(BirdColors);
            var pipesCols = new Color[pipeWidth * Height];            
            for (int i = 0; i < pipesCols.Length; i++)
            {
                pipesCols[i] = Color.Green;
            }
            pipeTexture = new Texture2D(GraphicsDevice, pipeWidth, Height);
            pipeTexture.SetData<Color>(pipesCols);
            testBird.BirdTexture = birdTexture;
            font = Content.Load<SpriteFont>("File");
            Func<Player>createFunc= () =>{
                return new Player(3, 2, 1, birdTexture, Width, Height, birdSize.X, birdSize.Y, pipes);
            };
            GA =
                new GeneticAlgorithmUpdateable<MathNet.Numerics.LinearAlgebra.Matrix<float>,Player>(100, 0.2f, 10, createFunc);
        }

        protected override void UnloadContent()
        {
        }

        public void Restart()
        {
            pipes.Clear();
            var p = new Pipe(Width, Height, pipeWidth);
            p.PipeTexture = pipeTexture;
            pipes.Add(p);
            time = 0f;
            score = 0;
        }

        float time=1.5f;
        KeyboardState old = new KeyboardState();
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var p in pipes)
            {
                p.Update(gameTime);
                if (p.PointRect.X <= 50 && !p.Passed) { p.Passed = true;score++;
            }
              
            }

            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time >= 1.5)
            {
                var p = new Pipe(Width, Height, pipeWidth);
                p.PipeTexture = pipeTexture;
                pipes.Add(p);
                time = 0;
            }
            GA.Update(gameTime);
            if (GA.TryToGo()) { Restart(); }
           
            for (int i = 0; i < pipes.Count; i++)
            {
                if (pipes[i].X + pipes[i].PipeWidth < 0)
                {
                    pipes.RemoveAt(i);
                    i--;
                }
                
            }          

            

            base.Update(gameTime);
        }
        int score=0;
       
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            GA.Draw(spriteBatch);

            foreach (var p in pipes)
            {
                p.Draw(spriteBatch);
            }
            spriteBatch.DrawString(font, "Generation = " + GA.CurrentGeneration, new Vector2(50, 50), Color.Cyan);
            spriteBatch.DrawString(font, score.ToString(), new Vector2(Width / 2, 50), Color.Cyan);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public Pipe GetNearPipe()
        {
            for (int i = 0; i < pipes.Count; i++)
            {
                if (!pipes[i].Passed) return pipes[i];
            }

            return null;
        }
    }
}
