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
        public Texture2D texture;
        Random rand;
        ObjectType type;

        const string ObjectAssetName = "Object";
        const int StartPositionX = 100;
        const int StartPositionY = 250;

        Point movement, start;
        //int health = 10;

        public GameObject(ContentManager theContentManager, ObjectType type, int seed)
        {
            this.type = type;
            rand = new Random(seed);
            switch (type)
            {
                case ObjectType.Asteroid:
                    movement = new Point(5, 0);
                    break;

                case ObjectType.Box:
                    start = new Point(300, rand.Next(150, 350));
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
        }

        public override void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(StartPositionX, StartPositionY);

            switch (type)
            {
                case ObjectType.Asteroid:
                    texture = Main.GameContent.Load<Texture2D>("Sprites/Asteriod");
                    start = new Point(900, rand.Next(150, 350));
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
            spriteBatch.Draw(texture, SpriteBoundingBox, Color.White);
        }
    }
}
