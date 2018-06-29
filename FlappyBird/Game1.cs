using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using NeuralNetwork;
using FlappyBird.GeneticAlg;
using GeneticAlgorithm;

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

        GeneticAlgorithmUpdateable<MathNet.Numerics.LinearAlgebra.Matrix<float>> GA;

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

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Width = GraphicsDevice.Viewport.Width;
            Height = GraphicsDevice.Viewport.Height;
            birdSize = new Point(15, 15);      
            pipes = new List<Pipe>();
  
            testBird = new Bird(Width, Height, birdSize.X, birdSize.Y);
            pipeWidth = 100;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
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
            GA =
                new GeneticAlgorithmUpdateable<MathNet.Numerics.LinearAlgebra.Matrix<float>>(100, 0.2f, 5, typeof(Player), 3, 2, 1, birdTexture, Width, Height, birdSize.X, birdSize.Y, pipes);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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
            
            /*
            var newState = Keyboard.GetState();
            if (!testBird.IsDead)
            {
                if (old.IsKeyUp(Keys.Space) && newState.IsKeyDown(Keys.Space))
                {
                    testBird.Flap();
                }
                old = newState;
              
                for (int i = 0; i < pipes.Count; i++)
                {
                    if (!testBird.IsDead && (testBird.IsCollide(pipes[i].Bottom) || testBird.IsCollide(pipes[i].Top)) )//|| testBird.Position.Y + testBird.Size.Y >= Height)
                    {
                        testBird.IsDead = true;
                    }
                }
            }
            testBird.Update(gameTime);
            
            foreach (var p in pipes)
            {
                if (!p.Passed && testBird.IsCollide(p.PointRect))
                {
                    p.Passed = true;
                    testBird.Score++;
                }
            }
            */
            for (int i = 0; i < pipes.Count; i++)
            {
                if (pipes[i].X + pipes[i].PipeWidth < 0)
                {
                    pipes.RemoveAt(i);
                    i--;
                }
                
            }          

            
            // TODO: Add your update logic here

            base.Update(gameTime);
        }


        /*
                public void NewGame()
                {
                    bird = new Bird(Width, Height, birdSize.X, birdSize.Y);
                    bird.BirdTexture = birdTexture;
                    pipes.Clear();
                    index++;
                }
                */
        int score=0;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //testBird.Draw(spriteBatch);
            GA.Draw(spriteBatch);

            foreach (var p in pipes)
            {
                p.Draw(spriteBatch);
            }
            spriteBatch.DrawString(font, "Generation = " + GA.CurrentGeneration, new Vector2(50, 50), Color.Cyan);


            //spriteBatch.Draw(pipeTexture, pipes[0].PointRect, Color.Green);
            // spriteBatch.DrawString(font, "Player1 Xnorm = " + GeneticAlg.Population[0].Xnorm + "\nYnorm = " + GeneticAlg.Population[0].Ynorm, new Vector2(50, 100), Color.Cyan);
            spriteBatch.DrawString(font, score.ToString(), new Vector2(Width / 2, 50), Color.Cyan);

            //spriteBatch.DrawString(font, "Score = " + testBird.Score+"\nIsDead = "+testBird.IsDead, new Vector2(50, 50), Color.Cyan);

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
