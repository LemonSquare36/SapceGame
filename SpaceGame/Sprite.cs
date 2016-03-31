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
        private Texture2D baseShip, Starrybackground, Wall;
        // Size of sprite
        public Rectangle Size;
        // Amount to increase/decrease size
        private float mScale = 1.0f;
        //For Updating under move
        public Vector2 Position = Vector2.Zero;

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, baseShip.Width, baseShip.Height);
            }
        }

        //public Rectangle BoundingBox
        //{
           // get
           // {
           //     return new Rectangle((int)Position.X, (int)Position.Y, Wall.Width, Wall.Height);
           // }
       // }

        public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                Size = new Rectangle(0, 0, (int)(baseShip.Width * Scale), (int)(baseShip.Height * Scale));
            }
        }
        public virtual void LoadContent(ContentManager cM) { }
        public virtual void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            Wall = theContentManager.Load<Texture2D>("Sprites/Wall");
            Starrybackground = theContentManager.Load<Texture2D>("Sprites/maxresdefault");
        }
        public virtual void Update(GameTime gameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            Position += theDirection * theSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
