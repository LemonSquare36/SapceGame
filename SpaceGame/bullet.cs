using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceGame
{
    public class Bullet : Sprite
    {

        public Vector2 position
        {
            get { return Position; }
        }

        public Bullet(Vector2 pos)
        {
            BulletStart = pos;
            BulletStart -= new Vector2(-16, -10);
            LoadContent();
        }

        Texture2D bullet;

        Vector2 BulletStart;
        Vector2 BulletMove = new Vector2(5, 0);
        public void LoadContent() 
        {
            bullet = Main.GameContent.Load<Texture2D>("Sprites/Bullet");

        }
        public void Update()
        {
            BulletStart += BulletMove;
            spriteBoundingBox = new Rectangle((int)BulletStart.X, (int)BulletStart.Y, bullet.Width, bullet.Height);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bullet, SpriteBoundingBox, Color.White);
        }

    }
}

