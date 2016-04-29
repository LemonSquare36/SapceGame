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
    enum ObjectType { Asteroid, Cannon }

    class Enemy : Sprite
    {
        public Texture2D texture;
        Random rand;
        ObjectType type;
        float rotation = 0;

        const string ObjectAssetName = "Object";
        const int StartPositionX = 100;
        const int StartPositionY = 250;

        Point movement, start;
        //int health = 10;

        public Enemy(ContentManager theContentManager, ObjectType type, int rand)
        {
            this.type = type;
            switch (type)
            {
                case ObjectType.Asteroid:
                    movement = new Point(5, 0);
                    start = new Point(900, rand);
                    break;


                case ObjectType.Cannon:
                    start = new Point(900, rand);
                    movement = new Point(3, 0);
                    break;

                default:
                    break;
            }
            LoadContent(theContentManager);
        }

        public void Update(GameTime gameTime)
        {
            spriteBoundingBox = new Rectangle(start.X, start.Y, texture.Width, texture.Height);

            if (type == ObjectType.Asteroid)
            {
                start -= movement;
                rotation += MathHelper.ToRadians(-3);
                if (MathHelper.ToDegrees(rotation) >= 360 || MathHelper.ToDegrees(rotation) <= -360) rotation = 0;
            }
            if (type == ObjectType.Cannon)
            {
                start -= movement;
                if (spriteBoundingBox.X >= 500)
                {
                    spriteBoundingBox.X = 500;
                }
            }
        }

        public override void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(StartPositionX, StartPositionY);

            try
            {
                switch (type)
                {
                    case ObjectType.Asteroid:
                        texture = Main.GameContent.Load<Texture2D>("Sprites/Asteriod");
                        break;

                    case ObjectType.Cannon:
                        texture = Main.GameContent.Load<Texture2D>("Sprites/Cannon");
                        break;

                    default:
                        break;
                }
            }
            catch { }

            base.LoadContent(theContentManager, ObjectAssetName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (type)
            {
                case ObjectType.Asteroid:
                    spriteBatch.Draw(texture, new Vector2(SpriteBoundingBox.Location.X + 25, SpriteBoundingBox.Location.Y + 25), null, Color.White, rotation, new Vector2(25, 25), 1, SpriteEffects.None, 0);
                    break;

                case ObjectType.Cannon:
                    spriteBatch.Draw(texture, SpriteBoundingBox, Color.White);
                    break;
                default:
                    break;
            }
        }
    }
}
