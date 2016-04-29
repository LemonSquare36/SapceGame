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
    public enum GameStates { MainMenu, GamePlaying }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        private GameStates gameState;
        event EventHandler GameStateChanged;

        private Texture2D YOUDIED;
        public GameStates GameState
        {
            get { return gameState; }
            set
            {
                gameState = value;
                OnGameStateChange();
            }
        }
        Menu menu;


        public Ship BaseShipSprite;
        Background Background1;
        Background Background2;
        Background Background3;
        Rectangle health = new Rectangle(100, 10, 200, 20);

        int AlphaValue = 1;
        double FadeInc = 3;
        double FadeDelay = 3;

        bool DoneMoving = true;
        bool doneMoving = true;

        Random Rand = new Random();
        int WallPos;
        int Select;
        int Wall2Pos;

        List<Enemy> objects = new List<Enemy>();
        Timer timer = new Timer(2000);
        Timer RandTimer = new Timer();

        WALL Wall1;
        WALL Wall2;
        WALL Wall3;
        WALL Wall4;
        WALL Wall5;
        WALL Wall6;
        Vector2 Pos1 = new Vector2(0, 100);
        Vector2 Pos2 = new Vector2(800, 100);
        Vector2 Pos3 = new Vector2(1600, 100);
        Vector2 Pos4 = new Vector2(0, 380);
        Vector2 Pos5 = new Vector2(800, 380);
        Vector2 Pos6 = new Vector2(1600, 380);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private SpriteFont font;
        private int score = 0;

        private Texture2D BaseShip, healthBar;
        private float angle = 0;

        private static ContentManager content;

        public static ContentManager GameContent
        {
            get { return content; }
            set { content = value; }
        }

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            content = Content;
            IsMouseVisible = true;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //BaseShipSprite = new Ship();
            Background1 = new Background();
            Background2 = new Background();
            Background3 = new Background();

            Wall1 = new WALL(Pos1);
            Wall2 = new WALL(Pos2);
            Wall3 = new WALL(Pos3);
            Wall4 = new WALL(Pos4);
            Wall5 = new WALL(Pos5);
            Wall6 = new WALL(Pos6);

            RandTimer.Interval = Rand.Next(1000, 2000);

            menu = new Menu(this);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            BaseShipSprite = new Ship(this);

            health.Width = (int)(((float)BaseShipSprite.HP / 100f) * 250);

            menu.LoadContent();

            BaseShip = Content.Load<Texture2D>("Sprites/Base Ship");
            healthBar = Content.Load<Texture2D>("Sprites/HealthBar");
            font = Content.Load<SpriteFont>("myFont");

            Background1.LoadContent(this.Content);
            Background1.Position = new Vector2(0, 0);
            Background2.LoadContent(this.Content);
            Background2.Position = new Vector2(Background2.Position.X + Background2.Size.Width, 0);
            Background3.LoadContent(this.Content);
            Background3.Position = new Vector2(Background3.Position.X + Background3.Size.Width, 0);

            Wall1.LoadContent(this.Content);
            Wall2.LoadContent(this.Content);
            Wall3.LoadContent(this.Content);

            Wall4.LoadContent(this.Content);
            Wall5.LoadContent(this.Content);
            Wall6.LoadContent(this.Content);

            YOUDIED = Content.Load<Texture2D>("Menu/YouDied");

            timer.Elapsed += TimeElapsed;
            RandTimer.Elapsed += RandTimeElapsed;
            Parallel.Invoke(() => timer.Start(), () => RandTimer.Start());

            spriteBatch = new SpriteBatch(GraphicsDevice);
            BaseShipSprite.LoadContent(this.Content);
            graphics.IsFullScreen = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void TimeElapsed(object sender, EventArgs e)
        {
            try
            {
                objects.Add(new Enemy(Content, ObjectType.Box, Rand.Next((int)Wall1.Position.Y + 30, (int)Wall4.Position.Y - 10)));
                Console.WriteLine("Colliding");
                timer.Stop();
                timer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void RandTimeElapsed(object sender, EventArgs e)
        {
            objects.Add(new Enemy(Content, ObjectType.Asteroid, Rand.Next((int)Wall1.Position.Y + 10, (int)Wall4.Position.Y -10)));
            RandTimer.Stop();
            RandTimer.Interval = Rand.Next(1000, 2000);
            RandTimer.Start();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param> 
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (gameState)
            {
                case GameStates.MainMenu:
                    spriteBatch.Begin();
                    menu.Update(gameTime);
                    spriteBatch.End();
                    break;
                case GameStates.GamePlaying:
                    WallMove(gameTime);
                    WallScroll();
                    BaseShipSprite.Update(gameTime);
                    CheckGameObjectCollision();
                    CheckBulletCollision();
                    Wall1.Update(gameTime, Vector2.Zero, Vector2.Zero);
                    Wall2.Update(gameTime, Vector2.Zero, Vector2.Zero);
                    Wall3.Update(gameTime, Vector2.Zero, Vector2.Zero);
                    Wall4.Update(gameTime, Vector2.Zero, Vector2.Zero);
                    Wall5.Update(gameTime, Vector2.Zero, Vector2.Zero);
                    Wall6.Update(gameTime, Vector2.Zero, Vector2.Zero);
                    if (health.Width > 0) score++;

                    for (int i = 0; i < objects.Count; i++ )
                    {
                        objects[i].Update(gameTime);
                    }

                    //Console.WriteLine(CheckBulletCollision());

                    if (health.Width <= 0)
                    {
                        FadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    if (health.Width >= 250)
                    {
                        health.Width = 250;
                    }
                    FadeIn();
                    break;
                default:
                    break;
            }

            base.Draw(gameTime);
        }
        private void NewGameState(object sender, EventArgs args)
        {
            switch (gameState)
            {
                case GameStates.MainMenu:
                    if (menu == null) menu = new Menu(this);
                    menu.LoadContent();
                    break;
            }
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTimeFunTime">Provides a snapshot of timing values.</param>shipBoundingBox
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (gameState)
            {
                case GameStates.MainMenu:
                    spriteBatch.Begin();
                    menu.Draw(spriteBatch);
                    spriteBatch.End();

                    break;
                case GameStates.GamePlaying:

                    spriteBatch.Begin();
                    //background
                    Background1.Draw(this.spriteBatch);
                    Background2.Draw(this.spriteBatch);
                    Background3.Draw(this.spriteBatch);

                    //spriteBatch.Draw(BaseShip, new Rectangle(400, 240, 27, 23), Color.White);

                    spriteBatch.End();
                    //walls
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);


                    Wall1.Draw(this.spriteBatch);
                    Wall2.Draw(this.spriteBatch);
                    Wall3.Draw(this.spriteBatch);
                    Wall4.Draw(this.spriteBatch);
                    Wall5.Draw(this.spriteBatch);
                    Wall6.Draw(this.spriteBatch);


                    //Wall0.Draw(this.spriteBatch);

                    spriteBatch.Draw(healthBar, health, Color.White);

                    spriteBatch.End();
                    //everything else
                    spriteBatch.Begin();

                    for (int i = 0; i < objects.Count; i++)
                    {
                        objects[i].Draw(spriteBatch);
                    }

                    BaseShipSprite.Draw(this.spriteBatch);
                    spriteBatch.DrawString(font, "Score: " + score, new Vector2(100, 100), Color.Red);

                    Vector2 location = new Vector2(300, 400);
                    Rectangle sourceRectangle = new Rectangle(0, 0, BaseShip.Width, BaseShip.Height);
                    Vector2 origin = new Vector2(BaseShip.Width / 2, BaseShip.Height * 3);

                    if (health.Width <= 0) spriteBatch.Draw(YOUDIED, new Vector2(25, 100), new Color(255, 255, 255, (byte)MathHelper.Clamp(AlphaValue, 0, 255)));

                    //spriteBatch.Draw(BaseShip, location, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);

                    spriteBatch.End();
                    base.Draw(gameTime);
                    break;
            }
        }
        public void WallMove(GameTime gameTime)
        {
            Vector2 bDirection = new Vector2(0, -1);
            Vector2 cDirection = new Vector2(0, 1);
            Vector2 cSpeed = new Vector2(0, 20);
            Vector2 dSpeed = new Vector2(0, -20);

            if (DoneMoving && doneMoving)
            {
                WallPos = Rand.Next(10, 155);
                Wall2Pos = Rand.Next(220, 455);
                Select = Rand.Next(1, 3);

                /*Console.WriteLine("WallPos " + WallPos);
                Console.WriteLine("Wall2Pos " + Wall2Pos);
                Console.WriteLine("Wall2real " + Wall4.Position.Y);
                Console.WriteLine(Select);*/
                DoneMoving = false;
                doneMoving = false;
            }

            if (!DoneMoving)
            {
                if (Select == 1)
                {
                    if (Wall1.Position.Y <= 10)
                    {
                        DoneMoving = true;
                    }
                    else if (Wall1.Position.Y != WallPos)
                    {
                        Wall1.Position += bDirection * cSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        Wall2.Position += bDirection * cSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        Wall3.Position += bDirection * cSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (Wall1.Position.Y >= WallPos) DoneMoving = true;
                    }
                }
                else if (Select == 2)
                {
                    if (Wall1.Position.Y >= 160)
                    {
                        DoneMoving = true;
                    }
                    else if (Wall1.Position.Y != WallPos)
                    {
                        Wall1.Position += bDirection * dSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        Wall2.Position += bDirection * dSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        Wall3.Position += bDirection * dSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (Wall1.Position.Y >= WallPos) DoneMoving = true;
                    }
                }
            }
            if (!doneMoving)
            {
                if (Select == 1)
                {
                    if (Wall4.Position.Y <= 250)
                    {
                        doneMoving = true;
                    }
                    else if (Wall4.Position.Y != Wall2Pos)
                    {
                        Wall4.Position += bDirection * cSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        Wall5.Position += bDirection * cSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        Wall6.Position += bDirection * cSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (Wall4.Position.Y <= Wall2Pos) doneMoving = true;
                    }
                }
                else if (Select == 2)
                {
                    if (Wall4.Position.Y >= 460)
                    {
                        doneMoving = true;
                    }
                    else if (Wall4.Position.Y != Wall2Pos)
                    {
                        Wall4.Position += bDirection * dSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        Wall5.Position += bDirection * dSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        Wall6.Position += bDirection * dSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (Wall4.Position.Y >= Wall2Pos) doneMoving = true;
                    }
                }
            }
            Vector2 aDirection = new Vector2(-1, 0);
            Vector2 aSpeed = new Vector2(100, 0);

            Vector2 bSpeed = new Vector2(200, 0);

            Background1.Position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Background2.Position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Background3.Position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Wall1.Position += aDirection * bSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Wall2.Position += aDirection * bSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Wall3.Position += aDirection * bSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Wall4.Position += aDirection * bSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Wall5.Position += aDirection * bSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Wall6.Position += aDirection * bSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public void WallScroll()
        {
            if (Background1.Position.X < 800)
            {
                Background1.Position.X = Background2.Position.X + 800;
            }
            if (Background2.Position.X < -800)
            {
                Background2.Position.X = Background3.Position.X + -800;
            }

            if (Background3.Position.X < 800)
            {
                Background3.Position.X = Background1.Position.X + 800;
            }

            if (Wall4.Position.X < 800)
            {
                Wall4.Position.X = Wall5.Position.X + 800;
            }
            if (Wall5.Position.X < -800)
            {
                Wall5.Position.X = Wall6.Position.X + -800;
            }

            if (Wall6.Position.X < 800)
            {
                Wall6.Position.X = Wall4.Position.X + 800;
            }

            if (Wall1.Position.X < 800)
            {
                Wall1.Position.X = Wall2.Position.X + 800;
            }
            if (Wall2.Position.X < -800)
            {
                Wall2.Position.X = Wall3.Position.X + -800;
            }

            if (Wall3.Position.X < 800)
            {
                Wall3.Position.X = Wall1.Position.X + 800;
            }
        }
        private void OnGameStateChange()
        {
            if (GameStateChanged != null)
            {
                GameStateChanged(this, EventArgs.Empty);
            }
        }

        private void CheckGameObjectCollision()
        {

            //if (BaseShipSprite.SpriteBoundingBox.Intersects(Wall0.SpriteBoundingBox))
            //BaseShipSprite.Position.Y = Wall0.spriteBoundingBox.Bottom;
            //Console.WriteLine(Wall0.SpriteBoundingBox);

            for (int i = 0; i < objects.Count; i++)
            {
                bool a = BaseShipSprite.SpriteBoundingBox.Intersects(objects[i].spriteBoundingBox) && BaseShipSprite.Position.X <= objects[i].spriteBoundingBox.X;
                bool b = BaseShipSprite.SpriteBoundingBox.Intersects(objects[i].spriteBoundingBox) && BaseShipSprite.Position.Y <= objects[i].spriteBoundingBox.Y;
                bool c = BaseShipSprite.SpriteBoundingBox.Intersects(objects[i].spriteBoundingBox) && BaseShipSprite.Position.X >= objects[i].spriteBoundingBox.X;
                bool d = BaseShipSprite.SpriteBoundingBox.Intersects(objects[i].spriteBoundingBox) && BaseShipSprite.Position.Y <= objects[i].spriteBoundingBox.Y;
                if (a || b || c || d)//left
                {
                    //BaseShipSprite.Position.X = i.SpriteBoundingBox.Left - BaseShipSprite.SpriteBoundingBox.Width;
                    health.Width -= 10;
                    objects.Remove(objects[i]);
                }
            }


            ///walls
            //top section of walls

            if (BaseShipSprite.SpriteBoundingBox.Intersects(Wall1.SpriteBoundingBox)) //&& BaseShipSprite.Position.Y >= Wall4.SpriteBoundingBox.Y)
            {
                BaseShipSprite.Position.Y = Wall1.SpriteBoundingBox.Bottom; //- BaseShipSprite.SpriteBoundingBox.Height;
                health.Width--;
            }

            if (BaseShipSprite.SpriteBoundingBox.Intersects(Wall2.SpriteBoundingBox))// && BaseShipSprite.Position.Y >= Wall4.SpriteBoundingBox.Y)
            {
                BaseShipSprite.Position.Y = Wall2.SpriteBoundingBox.Bottom; //- BaseShipSprite.SpriteBoundingBox.Height;
                health.Width--;
            }

            if (BaseShipSprite.SpriteBoundingBox.Intersects(Wall3.SpriteBoundingBox))// && BaseShipSprite.Position.Y >= Wall4.SpriteBoundingBox.Y)
            {
                BaseShipSprite.Position.Y = Wall3.SpriteBoundingBox.Bottom; //- BaseShipSprite.SpriteBoundingBox.Height;
                health.Width--;
            }

            //bottom section of walls

            if (BaseShipSprite.SpriteBoundingBox.Intersects(Wall4.SpriteBoundingBox) && BaseShipSprite.Position.Y <= Wall4.SpriteBoundingBox.Y)
            {
                BaseShipSprite.Position.Y = Wall4.SpriteBoundingBox.Top - BaseShipSprite.SpriteBoundingBox.Height;
                health.Width--;
            }



            if (BaseShipSprite.SpriteBoundingBox.Intersects(Wall5.SpriteBoundingBox) && BaseShipSprite.Position.Y <= Wall5.SpriteBoundingBox.Y)
            {
                BaseShipSprite.Position.Y = Wall5.SpriteBoundingBox.Top - BaseShipSprite.SpriteBoundingBox.Height;
                health.Width--;
            }



            if (BaseShipSprite.SpriteBoundingBox.Intersects(Wall6.SpriteBoundingBox) && BaseShipSprite.Position.Y <= Wall6.SpriteBoundingBox.Y)
            {
                BaseShipSprite.Position.Y = Wall6.SpriteBoundingBox.Top - BaseShipSprite.SpriteBoundingBox.Height;
                health.Width--;
            }

        }

        private void FadeIn()
        {
            if (FadeDelay <= 0)
            {
                FadeDelay = .035;
                AlphaValue += (int)FadeInc;
                if (AlphaValue >= 255 || AlphaValue <= 0)
                {
                    FadeInc *= 1;
                }
            }
        }

        private bool CheckBulletCollision()
        {
            for (int i = 0; i < BaseShipSprite.bullets.Count; i++)
            {
                for (int l = 0; l < objects.Count; l++)
                {
                    if (BaseShipSprite.bullets[i].SpriteBoundingBox.Intersects(objects[l].SpriteBoundingBox))
                    {
                        objects.Remove(objects[l]);
                        BaseShipSprite.bullets.Remove(BaseShipSprite.bullets[i]);
                        return true;
                    }
                }
            }
            return false;
        }

    }
}