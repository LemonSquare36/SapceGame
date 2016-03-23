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
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        Ship BaseShipSprite;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private SpriteFont font;
        private int score = 0;

        private Texture2D BaseShip;
        private float angle = 0;

        Sprite Background1;
        Sprite Background2;

        private static ContentManager content;
        public static ContentManager GameContent
        {
            get { return content; }
            set { content = value; }
        }
        //private AnimatedSprite animatedSprite;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            content = Content;
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
            BaseShipSprite = new Ship();
            Background1 = new Sprite();
            Background2 = new Sprite();
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

            BaseShip = Content.Load<Texture2D>("Sprites/Base Ship");
            font = Content.Load<SpriteFont>("myFont");

            Background1.LoadContent(this.Content, "Sprites/maxresdefault");
            Background1.Position = new Vector2(0, 0);
            Background1.LoadContent(this.Content, "Sprites/maxresdefault");
            Background1.Position = new Vector2(Background2.Position.X + Background2.Size.Width, 0);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            BaseShipSprite.LoadContent(this.Content);



            //Texture2D texture = Content.Load<Texture2D>("SmileyWalk");
            //animatedSprite = new AnimatedSprite(texture, 4, 4);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            BaseShipSprite.Update(gameTime);

            score++;
            angle += 0.02f;
            // animatedSprite.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTimeFunTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //animatedSprite.Draw(spriteBatch, new Vector2(400, 200));

            spriteBatch.Begin();

            spriteBatch.Draw(BaseShip, new Rectangle(400, 240, 27, 23), Color.White);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(100, 100), Color.Black);

            BaseShipSprite.Draw(this.spriteBatch);

            Vector2 location = new Vector2(300, 400);
            Rectangle sourceRectangle = new Rectangle(0, 0, BaseShip.Width, BaseShip.Height);
            Vector2 origin = new Vector2(BaseShip.Width / 2, BaseShip.Height * 3);

            spriteBatch.Draw(BaseShip, location, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
