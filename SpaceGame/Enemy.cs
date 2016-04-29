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
    enum pObjectType { Asteroid }

    class PowerUp : Sprite
    {
        public Texture2D texture;
        Random rand;
        pObjectType type;
        float rotation = 0;

        const string ObjectAssetName = "Object";
        const int StartPositionX = 100;
        const int StartPositionY = 250;

        Point movement, start;
        //int health = 10;

        public PowerUp(ContentManager theContentManager, pObjectType type, int rand)
        {
            this.type = type;
            switch (type)
            {
                case pObjectType.Asteroid:
                    movement = new Point(5, 0);
                    pStart = new Point(900, rand);
                    break;

                case pObjectType.Box:
                    pStart = new Point(300, rand);
                    movement = Point.Zero;
                    break;

                default:
                    break;
            }
            LoadContent(theContentManager);
        }

        public void Update(GameTime gameTime)
        {
            spriteBoundingBox = new Rectangle(pStart.X, pStart.Y, pTexture.Width, pTexture.Height);

            if (type == pObjectType.Asteroid)
            {
                pStart -= movement;
                rotation += MathHelper.ToRadians(-3);
                if (MathHelper.ToDegrees(rotation) >= 360 || MathHelper.ToDegrees(rotation) <= -360) rotation = 0;
            }
        }

        public override void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(StartPositionX, pStartPositionY);

            try
            {
                switch (type)
                {
                    case pObjectType.Asteroid:
                        pTexture = Main.GameContent.Load<Texture2D>("Sprites/Asteriod");
                        break;

                    case pObjectType.Box:
                        pTexture = Main.GameContent.Load<Texture2D>("Sprites/BoxyBox");
                        break;

                    default:
                        break;
                }
            }
            catch { }

            base.LoadContent(theContentManager, pObjectAssetName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (type)
            {
                case pObjectType.Asteroid:
                    spriteBatch.Draw(pTexture, new Vector2(SpriteBoundingBox.Location.X+25, SpriteBoundingBox.Location.Y+25), null, Color.White, rotation, new Vector2(25, 25), 1, SpriteEffects.None, 0);
                    break;

                case pObjectType.Box:
                    spriteBatch.Draw(pTexture, SpriteBoundingBox, Color.White);
                    break;
                
                default:
                    break;
            }
        }
    }
}
