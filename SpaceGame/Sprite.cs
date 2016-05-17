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
    public class Sprite
    {
        public string AssetName;
        private Texture2D baseShip, Starrybackground, Wall;
        // Size of sprite
        public Rectangle Size;
       public Rectangle spriteBoundingBox, asteroidBoundingBox;
        // Amount to increase/decrease size
        private float mScale = 1.0f;
        //For Updating under move
        public Vector2 Position = Vector2.Zero;

        protected int spriteWidth;
        protected int spriteHeight;
        protected int asteroidWidth;
        protected int asteroidHeight;

        public Rectangle SpriteBoundingBox
        {
            get
            {
                return spriteBoundingBox;
            }
        }

        public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                Size = new Rectangle(0, 0, (int)(baseShip.Width * Scale), (int)(baseShip.Height * Scale));
            }
        }

        public Sprite() { }

        public Sprite(Vector2 pos)
        {
            Position = pos;
        }

        public virtual void LoadContent(ContentManager cM) { }
        public virtual void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            baseShip = theContentManager.Load<Texture2D>("Sprites/Base Ship");
            Wall = theContentManager.Load<Texture2D>("Sprites/Wall");
            Starrybackground = theContentManager.Load<Texture2D>("Sprites/maxresdefault");
        }
        public virtual void Update(GameTime gameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            Position += theDirection * theSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            spriteBoundingBox = new Rectangle((int)Position.X, (int)Position.Y, spriteWidth, spriteHeight);
            asteroidBoundingBox = new Rectangle((int)Position.X, (int)Position.Y, asteroidWidth, asteroidHeight);

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
