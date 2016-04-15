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
        public Texture2D Wall;
        const string WallAssetName = "Wall";

        int theWidth = 800;
        int theHeight = 30;

        public WALL(Vector2 pos) : base(pos)
        {
            Position = pos;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            Wall = theContentManager.Load<Texture2D>("Sprites/Wall");
            base.LoadContent(theContentManager, WallAssetName);


            spriteWidth = Wall.Width;//theWidth;
            spriteHeight = Wall.Height;//theHeight;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Wall, SpriteBoundingBox, Color.White);
        }
    }
}
