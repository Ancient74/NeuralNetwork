
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallingRects
{
    class Rows
    {
        public readonly int ROW_WIDTH = 50;
        public readonly int ROW_HEIGHT = 100;
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public List<Row> Rects { get; set; }

        public Rows(int count, int ScreenWidth, int ScreenHeight)
        {
            this.ScreenHeight = ScreenHeight;
            this.ScreenWidth = ScreenWidth;
            Rects = new List<Row>();
            for (int i = -count/2; i < count/2; i++)
            {
                Rects.Add(new Row(ROW_WIDTH,ROW_HEIGHT,ScreenWidth/2+ROW_WIDTH*i,-ROW_HEIGHT,ScreenWidth,ScreenHeight));
            }
        }
        public void SpawnRandomRect()
        {
            Random r = new Random();
            int index = r.Next(Rects.Count);
            Rects[index].Spawn();
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Rects.Count; i++)
            {
                Rects[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var r in Rects)
            {
                r.Draw(spriteBatch);
            }
        }

    }
}
