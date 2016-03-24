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
    class Sprite
    {
        public string AssetName;
        private Texture2D baseShip, Starrybackground;
        // Size of sprite
        public Rectangle Size;
        // Amount to increase/decrease size
        private float mScale = 1.0f;
        //For Updating under move
        public Vector2 Position = Vector2.Zero;

        public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                Size = new Rectangle(0, 0, (int)(baseShip.Width * Scale), (int)(baseShip.Height * Scale));
            }
        }
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            Starrybackground = theContentManager.Load<Texture2D>("Sprites/maxresdefault");
            baseShip = Main.GameContent.Load<Texture2D>("Sprites/Base Ship");
            Size = new Rectangle(0, 0, (int)(baseShip.Width * Scale), (int)(baseShip.Height * Scale));
        }
        public void Update(GameTime gameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            Position += theDirection * theSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(baseShip, Position, new Rectangle(0, 0, baseShip.Width, baseShip.Height), Color.White);
            spriteBatch.Draw(Starrybackground, Position, new Rectangle(0, 0, Starrybackground.Width, Starrybackground.Height), Color.White);
        }
    }
}
