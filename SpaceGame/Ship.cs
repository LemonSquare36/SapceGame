using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace SpaceGame
{
    public enum sPowerUpType { DoubleShot }
    public class Ship : Sprite
    {

        public Texture2D baseShip;
        bool elapsed = true;
        // For Movement
        int hp = 100;
        const string ShipAssetName = "Ship";
        const int StartPositionX = 125;
        const int StartPositionY = 245;
        const int SpeedLR = 250;
        const int SpeedUD = 250;//160
        const int MoveUp = -1;
        const int MoveDown = 1;
        const int MoveLeft = -1;
        const int MoveRight = 1;
        Main world;
        //timer for bullets
        public Timer timer = new Timer(300), doubleShotTimer = new Timer(10000);

        public List<Bullet> bullets = new List<Bullet>();
        public List<sPowerUpType> sPowerUp = new List<sPowerUpType>();
        //int theWidth = 27;
        //int theHeight = 23;
       

        public int HP
        {
            get { return hp; }
        }

        private Main World
        {
            get { return world; }
        }

        enum State
        {
            Moving
        }
        State mCurrentState = State.Moving;

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        KeyboardState mPreviousKeyboardState;
    
        public Ship(Main main)
        {
            world = main;
        }

        public override void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(StartPositionX, StartPositionY);
            baseShip = Main.GameContent.Load<Texture2D>("Sprites/Base Ship");
            //Size = new Rectangle(0, 0, (int)(baseShip.Width * Scale), (int)(baseShip.Height * Scale));

            spriteWidth = baseShip.Width;//theWidth;
            spriteHeight = baseShip.Height;//theHeight;

            timer.Elapsed += TimerElapsed;
            doubleShotTimer.Elapsed += DoubleShotExpired;

            base.LoadContent(theContentManager, ShipAssetName);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState CurrentKeyBoardState = Keyboard.GetState();
            UpdateMovement(CurrentKeyBoardState);
            mPreviousKeyboardState = CurrentKeyBoardState;

            Shoot(CurrentKeyBoardState);

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update();
                if (bullets[i].SpriteBoundingBox.X > 800) bullets.Remove(bullets[i]);
            }

            //upper left hand corner
            if (Position.X <= 0 && Position.Y <= 0)
            {
                Position.X = 0;
                Position.Y = 0;
            }

            //left wall
            if (Position.X <= 0)
            {
                Position.X = 0;
            }


            //right wall
            else if (Position.X >= 775)
            {
                Position.X = 775;
            }


            //lower right hand corner
            else if (Position.X >= 775 && Position.Y >= 460)
            {
                Position.X = 775;
                Position.Y = 460;
            }


            //top wall
            else if (Position.Y <= 0)
            {
                Position.Y = 0;
            }

            //bottom wall
            else if (Position.Y >= 460)
            {
                Position.Y = 460;
            }
             
            //lower left hand corner
            if (Position.X <= 0 && Position.Y >= 460)
            {
                Position.X = 0;
                Position.Y = 460;
            }

            //upper right hand corner
            else if (Position.X >= 775 && Position.Y <= 0)
            {
                Position.X = 775;
                Position.Y = 0;
            }

            //if (CurrentKeyBoardState.IsKeyDown(Keys.A)) hp--;

            base.Update(gameTime, mSpeed, mDirection);

        }
        private void UpdateMovement(KeyboardState CurrentKeyBoardState)
        {
            if (mCurrentState == State.Moving)
            {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;

                if (CurrentKeyBoardState.IsKeyDown(Keys.Left) == true)
                {
                    mSpeed.X = SpeedLR;
                    mDirection.X = MoveLeft;
                }
                else if (CurrentKeyBoardState.IsKeyDown(Keys.Right) == true)
                {
                    mSpeed.X = SpeedLR;
                    mDirection.X = MoveRight;
                }

                if (CurrentKeyBoardState.IsKeyDown(Keys.Up) == true)
                {
                    mSpeed.Y = SpeedUD;
                    mDirection.Y = MoveUp;
                }
                else if (CurrentKeyBoardState.IsKeyDown(Keys.Down) == true)
                {
                    mSpeed.Y = SpeedUD;
                    mDirection.Y = MoveDown;
                }

            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(baseShip, Position, new Rectangle(0, 0, baseShip.Width, baseShip.Height), Color.White);

                foreach (Bullet i in bullets)
                {
                    i.Draw(spriteBatch);
                }

        }
        public void Shoot(KeyboardState CurrentKeyBoardState)
        {
            if (CurrentKeyBoardState.IsKeyDown(Keys.Z) == true && elapsed)
            {
                bullets.Add(new Bullet(Position));
                elapsed = false;
                timer.Stop();
                timer.Start();
            }      
        } 

        private void TimerElapsed(object sender, EventArgs e)
        {
            elapsed = true;
        }

        private void DoubleShotExpired(object sender, EventArgs e)
        {
            timer.Interval = 300;
            doubleShotTimer.Stop();
            sPowerUp.Remove(sPowerUpType.DoubleShot);
        }

        public void AddPowerup(sPowerUpType type)
        {
            sPowerUp.Add(type);
            switch (type)
            {
                case sPowerUpType.DoubleShot:
                    doubleShotTimer.Start();
                    timer.Interval = 150;
                    break;

                default:
                    break;
            }
        }
    }
}
