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
    public class CannonBullet : Sprite
    {

        public Vector2 position
        {
            get { return Position; }
        }

        public CannonBullet(Vector2 pos)
        {
            BulletStart = pos;
            BulletStart -= new Vector2(-16, -10);
            LoadContent();
        }

        Texture2D Cannonbullet;

        Vector2 BulletStart;
        Vector2 BulletMove = new Vector2(-5, 0);
        public void LoadContent()
        {
            Cannonbullet = Main.GameContent.Load<Texture2D>("Sprites/CannonShot");

        }
        public void Update()
        {
            BulletStart += BulletMove;
            spriteBoundingBox = new Rectangle((int)BulletStart.X, (int)BulletStart.Y, Cannonbullet.Width, Cannonbullet.Height);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Cannonbullet, SpriteBoundingBox, Color.White);
        }

    }
}

