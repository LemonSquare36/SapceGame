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
    public class Bullet : Sprite
    {
        public Vector2 position
        {
            get { return Position; }
        }

        public Bullet(Vector2 pos)
        {
            BulletStart = pos;
            LoadContent();
        }

        Texture2D bullet;

        Vector2 BulletStart;
        Vector2 BulletMove = new Vector2(2, 0);
        KeyboardState mPreviousKeyboardState;
        public void LoadContent()
        {
            BulletStart = Position;
            bullet = Main.GameContent.Load<Texture2D>("Sprites/Bullet");

        }
        public void Update()
        {
            BulletStart += BulletMove;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bullet, BulletStart, new Rectangle(0, 0, bullet.Width, bullet.Height), Color.White);
        }
    }
}

