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
    enum PowerUpType { HealthPack}
    class PowerUp : Sprite
    {
        Random rand;
        PowerUpType pType;

        Point startPoint;

        public Texture2D pTexture;

        const string pObjectAssetName = "Object";
        const int pStartPositionX = 100;
        const int pStartPositionY = 250;

        public PowerUp(ContentManager theContentManager, PowerUpType pType, int rand)
        {
            this.pType = pType;

            switch (pType)
            {
                case PowerUpType.HealthPack:
                    startPoint = new Point(100, 250);
                    break;

                default:
                    break;
            }
            LoadContent(theContentManager);
        }

        public void Update(GameTime gametime)
        {
            spriteBoundingBox = new Rectangle(startPoint.X, startPoint.Y, pTexture.Width, pTexture.Height);

            if (pType == PowerUpType.HealthPack)
            {

            }
        }

        public void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(pStartPositionX, pStartPositionY);

            try
            {
                switch (pType)
                {
                    case PowerUpType.HealthPack:
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
            switch pType
        }
    }
}
