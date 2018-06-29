using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork;
using Microsoft.Xna.Framework; 
using Microsoft.Xna.Framework.Graphics;

namespace FallingRects
{
    class Player
    {
        public float Score { get; private set; }
        public Vector2 Position { get; set; }
        public Point Size { get; set; }
        public Texture2D texture { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public Rows Rows { get; set; }

        public Player(Texture2D texture, int px, int py, int width, int height)
        {
            this.texture = texture;
            Size = new Point(px, py);
            ScreenWidth = width;
            ScreenHeight = height;
            Position = new Vector2();
           
        }
        public void Update(GameTime gameTime)
        {

          

        }

        public void GoLeft()
        {
            
        }
        public void GoRight() { }

        public void Draw(SpriteBatch sprtiteBacth)
        {
            sprtiteBacth.Draw(texture, new Rectangle(Position.ToPoint(),Size), Color.White);
        }
        float[] GetDistances()
        {
            float[] res = new float[Rows.Rects.Count];
            for (int i = 0; i < Rows.Rects.Count; i++)
            {
                res[i] = Rows.Rects[i].GetDistance((int)Position.Y);
            }
            return res;
        }
        static int GetMaxIndex(float[] arr)
        {
            float max = arr.Max();
            for (int i = 0; i < arr.Length; i++)
            {
                if (max == arr[i]) return i;
            }
            return -1;
        }
    }
}
