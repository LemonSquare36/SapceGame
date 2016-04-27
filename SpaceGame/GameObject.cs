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
        public Texture2D asteroid;

        //int AsteroidPosX = 480;
        //int AsteroidPosY = 400;

        const string ObjectAssetName = "Object";
        const int StartPositionX = 100;
        const int StartPositionY = 250;
        //int health = 10;

        public GameObject(ContentManager theContentManager)
        {
            LoadContent(theContentManager);
        }

        public override void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(StartPositionX, StartPositionY);
            boxyBox = Main.GameContent.Load<Texture2D>("Sprites/BoxyBox");

            asteroid = Main.GameContent.Load<Texture2D>("Sprites/Asteriod");

            spriteWidth = boxyBox.Width;
            spriteHeight = boxyBox.Height;

            spriteBoundingBox = new Rectangle(StartPositionX, StartPositionY, spriteWidth, spriteHeight);
            AsteroidBoundingBox = new Rectangle();

            base.LoadContent(theContentManager, ObjectAssetName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(boxyBox, SpriteBoundingBox, Color.White);
            spriteBatch.Draw(asteroid, SpriteBoundingBox, Color.White);
        }
    }
}
