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
    enum ObjectType { Box, Asteroid }

    class GameObject : Sprite
    {
        public Texture2D texture, pix;
        Random rand;
        ObjectType type;
        float rotation = 0;

        const string ObjectAssetName = "Object";
        const int StartPositionX = 100;
        const int StartPositionY = 250;

        Point movement, start;
        //int health = 10;

        public GameObject(ContentManager theContentManager, ObjectType type, int rand)
        {
            this.type = type;
            switch (type)
            {
                case ObjectType.Asteroid:
                    movement = new Point(5, 0);
                    start = new Point(900, rand);
                    break;

                case ObjectType.Box:
                    start = new Point(300, rand);
                    movement = Point.Zero;
                    break;

                default:
                    break;
            }
            LoadContent(theContentManager);
        }

        public void Update(GameTime gameTime)
        {
            spriteBoundingBox = new Rectangle(start.X, start.Y, texture.Width, texture.Height);
            start -= movement;

            if (type == ObjectType.Asteroid)
            {
                rotation += MathHelper.ToRadians(-3);
                if (MathHelper.ToDegrees(rotation) >= 360 || MathHelper.ToDegrees(rotation) <= -360) rotation = 0;
            }
        }

        public override void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(StartPositionX, StartPositionY);

            switch (type)
            {
                case ObjectType.Asteroid:
                    texture = Main.GameContent.Load<Texture2D>("Sprites/Asteriod");
                    break;
                
                case ObjectType.Box:
                    texture = Main.GameContent.Load<Texture2D>("Sprites/BoxyBox");
                    break;

                default:
                    break;
            }

            base.LoadContent(theContentManager, ObjectAssetName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (type)
            {
                case ObjectType.Asteroid:
                    spriteBatch.Draw(texture, new Vector2(SpriteBoundingBox.Location.X+25, SpriteBoundingBox.Location.Y+25), null, Color.White, rotation, new Vector2(25, 25), 1, SpriteEffects.None, 0);
                    break;

                case ObjectType.Box:
                    spriteBatch.Draw(texture, SpriteBoundingBox, Color.White);
                    break;
                
                default:
                    break;
            }
        }
    }
}
