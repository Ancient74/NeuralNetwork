using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird
{
    class Bird
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Point Size { get; set; }
        public Rectangle BirdRect => new Rectangle(Position.ToPoint(), Size);
        public Texture2D BirdTexture { get; set; }
        public int Score { get; set; }
        public int Distance { get; set; }
        public bool IsDead { get; set; } = false;

        public Bird(int Width, int Height, int SizeX, int SizeY)
        {
            this.Height = Height;
            this.Width = Width;

            Size = new Point(SizeX, SizeY);
            Position = new Vector2(50,Height/2-SizeY/2);
            Velocity = new Vector2();
            Acceleration = new Vector2(0, 0.99f);

        }
        public void Update(GameTime gameTime)
        {
            // Acceleration += Velocity * (-0.03f);
            Distance += 4;
            Velocity += Acceleration;
            Velocity *= new Vector2(0, 0.9f);
            Position += Velocity;
            if (Position.Y < 0) { Position = new Vector2(Position.X, 0); Velocity = Vector2.Zero; }
            if (Position.Y > Height - Size.Y) { Position = new Vector2(Position.X, Height - Size.Y); Velocity = Vector2.Zero; }
        }

        

        public void Flap()
        {
            Velocity += new Vector2(0, -30f);
        }

        public bool IsCollide(Rectangle b)
        {
            var a = BirdRect;
            return !(a.X + a.Width < b.X || b.X + b.Width < a.X || a.Y + a.Height < b.Y || b.Y + b.Height < a.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BirdTexture, Position, Color.White);
        }
    }
}
