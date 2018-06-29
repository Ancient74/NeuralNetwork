using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallingRects
{
    class Row
    {
        public List<Rectangle> Rect { get; set; }
        public int RectHeight { get; set; }
        public int RectWidth { get; set; }
        public int SpawnX { get; set; }
        public int SpawnY { get; set; }
        public Texture2D RectTexture { get; set; }
        public int FallingSpeed { get; set; } = 5;
        public int SW { get; set; }
        public int SH { get; set; }

        ///<summary>
        ///Init new row with 
        ///</summary>
        /// <param name="w">Width of Rectangle</param>
        /// <param name="h">Height of Rectangle</param>
        /// <param name="x">Spawn x position</param>
        /// <param name="y">Spawn y position</param>
        /// <param name = "sw" >Screen width</param>
        /// <param name="sh">Screen height</param>
        public Row(int w, int h, int x, int y,int sw,int sh)
        {
            Rect = new List<Rectangle>();
            RectHeight = h;
            RectWidth = w;
            SpawnX = x;
            SpawnY = y;
            SW = sw;
            SH = sh;           
        }
        /// <summary>
        /// Spawn rect at the X, Y spawn position
        /// </summary>
        public void Spawn()
        {
            Rect.Add(new Rectangle(SpawnX, SpawnY, RectWidth, RectHeight));
        }
        /// <summary>
        /// Update positon of the rectangles
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Rect.Count; i++)
                Rect[i] = new Rectangle(Rect[i].X, Rect[i].Y + FallingSpeed, Rect[i].Width, Rect[i].Height);
            for (int i = 0; i < Rect.Count; i++)
                if (Rect[i].Y > SH) { Rect.RemoveAt(i); i--; }


        }
        /// <summary>
        /// Draw rectangles
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (RectTexture != null)
                for (int i = 0; i < Rect.Count; i++)
                    spriteBatch.Draw(RectTexture, Rect[i], Color.White);
        }
        public float GetDistance(int PlayerY)
        {
            if (Rect.Count > 0)
            {
                return Rect[0].Bottom - PlayerY;
            }
            return SH-PlayerY;
        }

    }
}
