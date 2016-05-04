using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        Timer CannonTimer = new Timer(1000);
        float ShipX, ShipY;
        Ship ShipSprite;

        private int bulletValue;

        public int BulletValue
        {
            get { return bulletValue; }
        }
        public Point Start       
        {
            get { return start; }
        }

        public int BulletStart { get; set; }

        const string ObjectAssetName = "Object";
        const int StartPositionX = 100;
        const int StartPositionY = 250;

        public List<CannonBullet> CannonBullet = new List<CannonBullet>();

        Point movement, start;
        //int health = 10;

        public Enemy(ContentManager theContentManager, ObjectType type, int rand, Ship shipSprite)
        {
            this.type = type;
            ShipSprite = shipSprite;
            switch (type)
            {
                case ObjectType.Asteroid:
                    movement = new Point(5, 0);
                    start = new Point(900, rand);
                    bulletValue = 1;
                    BulletStart = 0;
                    break;


                case ObjectType.Cannon:
                    start = new Point(900, rand);
                    movement = new Point(3, 0);
                    bulletValue = 2;
                    BulletStart = 0;
                    CannonTimer.Start();
                    CannonTimer.Elapsed += CannonTimeElasped;

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
                ShipX = ShipSprite.Position.X;
                ShipY = ShipSprite.Position.Y;
                start -= movement;
                if (start.X <= 500)
                {
                    start.X = 500;
                    for (int i = 0; i < CannonBullet.Count; i++)
                    {
                        CannonBullet[i].Update();
                        if (CannonBullet[i].SpriteBoundingBox.X < 0) CannonBullet.Remove(CannonBullet[i]);
                        
                    }
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
                foreach (CannonBullet i in CannonBullet)
                {
                    i.Draw(spriteBatch);
                }
                    break;
                default:
                    break;
            }
        }
        private void CannonBulletAdd()
        {
            CannonBullet.Add(new CannonBullet(new Vector2(spriteBoundingBox.X, spriteBoundingBox.Y), ShipX, ShipY));
        }
        private void CannonTimeElasped(object sender, EventArgs e)
        {
            if (start.X <= 500)
            {
                CannonBulletAdd();
            }
                CannonTimer.Stop();
                CannonTimer.Start();
            }
        }
    }
