using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GeneticAlgorithm
{
    public interface IUpdateable
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }

}
