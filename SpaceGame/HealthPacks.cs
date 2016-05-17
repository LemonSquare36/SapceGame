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
    enum PackType { HealthPack, SuperPack }
    class HealthPacks : Sprite
    {
        Random hRand = new Random();
        PackType hType;

        Point startingPoint;

        public Texture2D hTexture;

        const string hObjectAssetName = "Object";
        const int hStartPositionX = 100;
        const int hStartPositionY = 250;

        public HealthPacks(ContentManager contentManager, PackType hType, int rand)
        {
            this.hType = hType;
            int randomInt = hRand.Next(100, 775);

            switch (hType)
            {
                case PackType.HealthPack:
                    startingPoint = new Point(rand, randomInt);
                    break;

                default:
                    break;
            }
            LoadContent(contentManager);
        }

        public void Update(GameTime gametime)
        {
            spriteBoundingBox = new Rectangle(startingPoint.X, startingPoint.Y, hTexture.Width, hTexture.Height);

            if (hType == PackType.HealthPack)
            {

            }
        }

        public void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(hStartPositionX, hStartPositionY);

            try
            {
                switch (hType)
                {
                    case PackType.HealthPack:
                        hTexture = Main.GameContent.Load<Texture2D>("Sprites/BoxyBox");
                        break;

                    default:
                        break;
                }
            }
            catch { }

            base.LoadContent(theContentManager, hObjectAssetName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (hType)
            {
                case PackType.HealthPack:
                    spriteBatch.Draw(hTexture, new Vector2(SpriteBoundingBox.Location.X, SpriteBoundingBox.Y), null, Color.White);
                    break;

                default:
                    break;
            }
        }
    }
}