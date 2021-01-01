using Microsoft.Xna.Framework.Graphics;

namespace Clkd.Assets
{
    public class GeneratedTextureBatchStrategy : IBatchStrategy
    {
        public void Begin(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(blendState: BlendState.NonPremultiplied);
        }

        public void End(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
        }
    }
}