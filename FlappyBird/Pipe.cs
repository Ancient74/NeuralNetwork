using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird
{
    public class Pipe
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int H { get; set; }
        public int PipeWidth { get; set; }
        public int X { get; set; }
        public bool Passed { get; set; } = false;
        int XSpeed { get; set; } = 4;
        public Rectangle Top => new Rectangle(X, 0, PipeWidth, H);
        public Rectangle Bottom => new Rectangle(X, H + HOLE, PipeWidth, Height - (H + HOLE));
        public Rectangle FullRect => new Rectangle(X, 0, PipeWidth, Height);
        public Rectangle HoleRect => new Rectangle(X, H, PipeWidth, HOLE);
        public Rectangle PointRect => new Rectangle(X+PipeWidth, H, 1, HOLE);
        public Texture2D PipeTexture { get; set; }
        
        public const int HOLE = 150;

        public Pipe(int Width, int Height, int PipeWidth=25)
        {
            this.Width = Width;
            this.Height = Height;
            X = Width + PipeWidth;
            Random r = new Random();
            H = r.Next(25, Height - HOLE);
            this.PipeWidth = PipeWidth;
            
        }
        public void Update(GameTime gameTime)
        {
            X -= XSpeed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PipeTexture, Top, Color.White);
            spriteBatch.Draw(PipeTexture, Bottom, Color.White);
        }
    }
}
