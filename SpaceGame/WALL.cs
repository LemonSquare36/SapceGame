using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceGame
{
    class WALL : Sprite
    {
        private Texture2D Wall;
        const string WallAssetName = "Wall";

        public void LoadContent(ContentManager theContentManager)
        {
           Wall = theContentManager.Load<Texture2D>("Sprites/Wall");
           base.LoadContent(theContentManager, WallAssetName);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Wall, Position, new Rectangle(200, 480, Wall.Width, Wall.Height), Color.White);
        }
    }
}
