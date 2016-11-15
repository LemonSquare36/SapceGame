using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceGame
{
    class Background : Sprite
    {
        private Texture2D Starrybackground;
        const string BackgroundAssetName = "Background";
        public override void LoadContent(ContentManager theContentManager)
        {
           Starrybackground = theContentManager.Load<Texture2D>("Sprites/maxresdefault");
           base.LoadContent(theContentManager, BackgroundAssetName);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Starrybackground, Position, new Rectangle(0, 0, Starrybackground.Width, Starrybackground.Height), Color.White);
        }
    }
}
