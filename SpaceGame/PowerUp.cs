﻿using System;
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
    enum PowerUpType {Shield}
    class PowerUp : Sprite
    {
        Random Rand = new Random();
        PowerUpType pType;

        Point startPoint;

        public Texture2D pTexture;

        const string pObjectAssetName = "Object";
        const int pStartPositionX = 100;
        const int pStartPositionY = 250;

        public PowerUp(ContentManager theContentManager, PowerUpType pType, int rand)
        {
            this.pType = pType;
            int randInt = Rand.Next(100, 775);

            switch (pType)
            {
                case PowerUpType.Shield:
                    startPoint = new Point(randInt, rand);
                    break;

                default:
                    break;
            }
            hLoadContent(theContentManager);
        }

        public void Update(GameTime gametime)
        {
            spriteBoundingBox = new Rectangle(startPoint.X, startPoint.Y, pTexture.Width, pTexture.Height);

            if (pType == PowerUpType.Shield)
            {
                
            }
        }

        public void hLoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(pStartPositionX, pStartPositionY);

            try
            {
                switch (pType)
                {
                    case PowerUpType.Shield:
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
            switch (pType)
            {
                case PowerUpType.Shield:
                    spriteBatch.Draw(pTexture, new Vector2(SpriteBoundingBox.Location.X, SpriteBoundingBox.Y), null, Color.White);
                    break;

                default:
                    break;
            }
        }
    }
}
