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
        Button Continue;
        Texture2D ContinueP, ContinueUP;
        int BulletStart = 0;
        MouseState mouse;
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
        bool GameRunning = true;

        Random Rand = new Random();
        int WallPos;
        int Select;
        int Wall2Pos;

        List<HealthPacks> healthPacks = new List<HealthPacks>();
        List<PowerUp> powerUps = new List<PowerUp>();
        List<Enemy> objects = new List<Enemy>();
        Timer RandTimer2 = new Timer();
        Timer RandTimer = new Timer();
        Timer CTimer = new Timer();

        WALL Wall1;
        WALL Wall2;
        WALL Wall3;
        WALL Wall4;
        WALL Wall5;
        WALL Wall6;
        Vector2 Pos1 = new Vector2(0, 100);
        Vector2 Pos2 = new Vector2(0, 100);
        Vector2 Pos3 = new Vector2(1600, 100);
        Vector2 Pos4 = new Vector2(0, 380);
        Vector2 Pos5 = new Vector2(0, 380);
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
            CTimer.Interval = Rand.Next(4000, 6000);
            RandTimer2.Interval = Rand.Next(10000, 20000);

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
            ContinueP = Content.Load<Texture2D>("Menu/ContinuePressed");
            ContinueUP = Content.Load<Texture2D>("Menu/Continue");
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

            RandTimer.Elapsed += RandTimeElapsed;
            CTimer.Elapsed += CTimeElapsed;
            RandTimer2.Elapsed += RandTime2Elasped;
            Parallel.Invoke(() => CTimer.Start(), () => RandTimer.Start(), () => RandTimer2.Start());
            Continue = new Button(new Vector2(300, 400), 200, 50, 4, mouse, ContinueUP, ContinueP, 800, 480);
            Continue.ButtonPressed += ButtonPressed;
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
        private void RandTimeElapsed(object sender, EventArgs e)
        {
            objects.Add(new Enemy(Content, ObjectType.Asteroid, Rand.Next((int)Wall1.Position.Y + 10, (int)Wall4.Position.Y - 10), BaseShipSprite));
            RandTimer.Stop();
            RandTimer.Interval = Rand.Next(500, 1000);
            RandTimer.Start();
        }
        private void CTimeElapsed(object sender, EventArgs e)
        {
            objects.Add(new Enemy(Content, ObjectType.Cannon, Rand.Next((int)Wall1.Position.Y + 10, (int)Wall4.Position.Y - 10), BaseShipSprite));
            CTimer.Stop();
            CTimer.Interval = Rand.Next(4000, 6000);
            CTimer.Start();
        }
        private void RandTime2Elasped(object sender, EventArgs e)
        {
            healthPacks.Add(new HealthPacks(Content, PackType.HealthPack, Rand.Next((int)Wall1.Position.Y + 10, (int)Wall4.Position.Y - 10)));
            powerUps.Add(new PowerUp(Content, PowerUpType.DoubleShot, Rand.Next((int)Wall1.Position.Y + 10, (int)Wall4.Position.Y - 10)));
            RandTimer2.Stop();
            RandTimer2.Interval = Rand.Next(10000, 20000);
            RandTimer2.Start();
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param> 
        protected override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();

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
                    CheckEnemyCollision();
                    CheckBulletCollision();
                    CheckPowerUpCollision();
                    CheckPackCollision();
                    CheckCannonballCollision();
                    Wall1.Update(gameTime, Vector2.Zero, Vector2.Zero);
                    Wall2.Update(gameTime, Vector2.Zero, Vector2.Zero);
                    Wall3.Update(gameTime, Vector2.Zero, Vector2.Zero);
                    Wall4.Update(gameTime, Vector2.Zero, Vector2.Zero);
                    Wall5.Update(gameTime, Vector2.Zero, Vector2.Zero);
                    Wall6.Update(gameTime, Vector2.Zero, Vector2.Zero);
                    if (health.Width > 0) score++;

                    for (int i = 0; i < objects.Count; i++)
                    {
                        objects[i].Update(gameTime);
                    }

                    for (int p = 0; p < powerUps.Count; p++)
                    {
                        powerUps[p].Update(gameTime);
                    }

                    for (int h = 0; h < healthPacks.Count; h++)
                    {
                        healthPacks[h].Update(gameTime);
                    }

                    //Console.WriteLine(CheckBulletCollision());

                    if (health.Width <= 0)
                    {
                        FadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                        GameRunning = false;
                    }

                    if (health.Width >= 250)
                    {
                        health.Width = 250;
                    }

                    if (GameRunning) BaseShipSprite.Update(gameTime);
                    else Continue.Update(mouse);

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

                    for (int p = 0; p < powerUps.Count; p++)
                    {
                        powerUps[p].Draw(spriteBatch);
                    }
                    for (int h = 0; h < healthPacks.Count; h++)
                    {
                        healthPacks[h].Draw(spriteBatch);
                    }
                    if (health.Width <= 0)
                    {
                        spriteBatch.Draw(Continue.Texture, Continue.Position, Color.White);
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

        private void CheckEnemyCollision()
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

        private void CheckPowerUpCollision()
        {

            for (int p = 0; p < powerUps.Count; p++)
            {
                bool A = BaseShipSprite.SpriteBoundingBox.Intersects(powerUps[p].spriteBoundingBox) && BaseShipSprite.Position.X <= powerUps[p].spriteBoundingBox.X;
                bool B = BaseShipSprite.SpriteBoundingBox.Intersects(powerUps[p].spriteBoundingBox) && BaseShipSprite.Position.Y <= powerUps[p].spriteBoundingBox.Y;
                bool C = BaseShipSprite.SpriteBoundingBox.Intersects(powerUps[p].spriteBoundingBox) && BaseShipSprite.Position.X >= powerUps[p].spriteBoundingBox.X;
                bool D = BaseShipSprite.SpriteBoundingBox.Intersects(powerUps[p].spriteBoundingBox) && BaseShipSprite.Position.Y <= powerUps[p].spriteBoundingBox.Y;

                if (A || B || C || D)
                {
                    powerUps.Remove(powerUps[p]);
                    //BaseShipSprite.timer.Interval = 150;
                    BaseShipSprite.AddPowerup(sPowerUpType.DoubleShot);
                }
            }
        }
        private void CheckPackCollision()
        {

            for (int h = 0; h < healthPacks.Count; h++)
            {
                bool Z = BaseShipSprite.SpriteBoundingBox.Intersects(healthPacks[h].spriteBoundingBox) && BaseShipSprite.Position.X <= healthPacks[h].spriteBoundingBox.X;
                bool Y = BaseShipSprite.SpriteBoundingBox.Intersects(healthPacks[h].spriteBoundingBox) && BaseShipSprite.Position.Y <= healthPacks[h].spriteBoundingBox.Y;
                bool X = BaseShipSprite.SpriteBoundingBox.Intersects(healthPacks[h].spriteBoundingBox) && BaseShipSprite.Position.X >= healthPacks[h].spriteBoundingBox.X;
                bool W = BaseShipSprite.SpriteBoundingBox.Intersects(healthPacks[h].spriteBoundingBox) && BaseShipSprite.Position.Y <= healthPacks[h].spriteBoundingBox.Y;

                if (Z || Y || X || W)
                {
                    health.Width += 15;
                    healthPacks.Remove(healthPacks[h]);
                }
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
                        objects[l].BulletStart = objects[l].BulletStart + 1;
                        BaseShipSprite.bullets.Remove(BaseShipSprite.bullets[i]);
                        if (objects[l].BulletStart >= objects[l].BulletValue)
                        {
                            objects.Remove(objects[l]);
                            return true;
                        }

                        return true;
                    }
                }
            }
            return false;
        }
        private bool CheckCannonballCollision()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                for (int l = 0; l < objects[i].CannonBullet.Count; l++)
                {
                    if (BaseShipSprite.SpriteBoundingBox.Intersects(objects[i].CannonBullet[l].SpriteBoundingBox))
                    {
                        health.Width -= 15;
                        objects[l].BulletStart = objects[l].BulletStart + 1;
                        if (objects[i].CannonBullet[l].cannonBallStart >= objects[i].CannonBullet[l].cannonBallValue)
                        {
                            objects[i].CannonBullet.Remove(objects[i].CannonBullet[l]);
                            return true;
                        }

                        return true;
                    }
                }
            }
            return false;
        }
        private void ButtonPressed(object sender, EventArgs e)
        {
            switch (((Button)sender).ButtonNum)
            {
                case 4:
                    menu.type = MenuType.Highscores;
                    GameState = GameStates.MainMenu;
                    menu.LoadContent();
                    menu.HighScores.recordScore(score, "");
                    menu.HighScores.retrieveScores();
                    break;
            }
        }
    }
}