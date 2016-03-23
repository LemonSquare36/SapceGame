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
    class Ship : Sprite
    {
        // For Movement
        const string ShipAssetName = "Ship";
        const int StartPositionX = 125;
        const int StartPositionY = 245;
        const int SpeedLR = 400;//100
        const int SpeedUD = 400;//160
        const int MoveUp = -1;
        const int MoveDown = 1;
        const int MoveLeft = -1;
        const int MoveRight = 1;

        enum State
        {
            Moving
        }
        State mCurrentState = State.Moving;

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        KeyboardState mPreviousKeyboardState;

        public void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(StartPositionX, StartPositionY);
            base.LoadContent(theContentManager, ShipAssetName);
        }
        public void Update(GameTime gameTime)
        {
            KeyboardState CurrentKeyBoardState = Keyboard.GetState();
            UpdateMovement(CurrentKeyBoardState);
            mPreviousKeyboardState = CurrentKeyBoardState;

            //upper left hand corner
            if (Position.X <= 0 && Position.Y <= 0)
            {
                Position.X = 0;
                Position.Y = 0;
            }

            //left wall
            else if (Position.X <= 0)
            {
                Position.X = 0;
            }

            //lower right hand corner
            else if (Position.X >= 775 && Position.Y >= 460)
            {
                Position.X = 775;
                Position.Y = 460;
            }

            //right wall
            else if (Position.X >= 775)
            {
                Position.X = 775;
            }


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
    }
}
