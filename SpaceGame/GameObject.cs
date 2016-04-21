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
    class GameObject : Sprite
    {
        public Texture2D boxyBox;
        const string ObjectAssetName = "Object";
        const int StartPositionX = 100;
        const int StartPositionY = 250;
        //int health = 10;

        public override void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(StartPositionX, StartPositionY);
            boxyBox = Main.GameContent.Load<Texture2D>("Sprites/BoxyBox");

            spriteWidth = boxyBox.Width;
            spriteHeight = boxyBox.Height;

            spriteBoundingBox = new Rectangle(StartPositionX, StartPositionY, spriteWidth, spriteHeight);

            base.LoadContent(theContentManager, ObjectAssetName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(boxyBox, SpriteBoundingBox, Color.White);
        }
    }
}
