﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SpaceGame
{
    enum MenuType { MainMenu, Options, Highscores }

    class Menu
    {
        Main main;
        public MenuType type = MenuType.MainMenu;
        MouseState mouse;
        public ScoreBoard HighScores;
        Button start, options, mainMenu;
        Texture2D background, StartP, StartUP, OptionsP, OptionsUP, MainMenuP, MainMenuUP, Title;
        SpriteFont font;


        public Menu(Main main)
        {
            this.main = main;
        }
        public void LoadContent()
        {
            background = Main.GameContent.Load<Texture2D>("Sprites/maxresdefault");
            Title = Main.GameContent.Load<Texture2D>("Menu/SapceGame");
            StartP = Main.GameContent.Load<Texture2D>("Menu/PlayButtonP");
            StartUP = Main.GameContent.Load<Texture2D>("Menu/PlayButtonUP");
            OptionsP = Main.GameContent.Load<Texture2D>("Menu/OptionsP");
            OptionsUP = Main.GameContent.Load<Texture2D>("Menu/OptionsUP");
            MainMenuP = Main.GameContent.Load<Texture2D>("Menu/MainMenuP");
            MainMenuUP = Main.GameContent.Load<Texture2D>("Menu/MainMenuUP");
            HighScores = new ScoreBoard("Scores", "HighScores", 10);
            mouse = Mouse.GetState();
            font = Main.GameContent.Load<SpriteFont>("myFont");

            switch (type)
            {
                case MenuType.MainMenu:
                    start = new Button(new Vector2(650, 400), 100, 50, 1, mouse, StartUP, StartP, 800, 480);
                    options = new Button(new Vector2(50, 400), 150, 50, 2, mouse, OptionsUP, OptionsP, 800, 480);

                    start.ButtonPressed += ButtonPressed;
                    options.ButtonPressed += ButtonPressed;
                    break;
                case MenuType.Options:
                    mainMenu = new Button(new Vector2(300, 400), 200, 50, 3, mouse, MainMenuUP, MainMenuP, 800, 480);
                    mainMenu.ButtonPressed += ButtonPressed;
                    break;
                case MenuType.Highscores:
                    mainMenu = new Button(new Vector2(300, 400), 200, 50, 3, mouse, MainMenuUP, MainMenuP, 800, 480);
                    mainMenu.ButtonPressed += ButtonPressed;
                    HighScores.retrieveScores();
                    break;
                default:
                    break;
            }
        }
        public void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();
            switch (type)
            {
                case MenuType.MainMenu:
                    start.Update(mouse);
                    options.Update(mouse);
                    break;
                case MenuType.Options:
                    mainMenu.Update(mouse);
                    break;
                case MenuType.Highscores:
                    mainMenu.Update(mouse);
                    break;
                default:
                    break;
            }
        }
        private void ButtonPressed(object sender, EventArgs e)
        {
            switch (((Button)sender).ButtonNum)
            {
                case 1:
                    main.GameState = GameStates.GamePlaying;
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(main.soDarude);
                    break;
                case 2:
                    type = MenuType.Options;
                    LoadContent();
                    break;
                case 3:
                    type = MenuType.MainMenu;
                    break;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            try
            {
                switch (type)
                {
                    case MenuType.MainMenu:
                        spriteBatch.Draw(Title, new Vector2(225, 100), Color.White);
                        spriteBatch.Draw(start.Texture, start.Position, Color.White);
                        spriteBatch.Draw(options.Texture, options.Position, Color.White);

                        break;
                    case MenuType.Options:
                        spriteBatch.Draw(mainMenu.Texture, mainMenu.Position, Color.White);

                        break;
                    case MenuType.Highscores:
                        spriteBatch.Draw(mainMenu.Texture, mainMenu.Position, Color.White);
                        for (int i = 0; i < HighScores.HighScores.Length; i++)
                        {
                            spriteBatch.DrawString(font, HighScores.HighScores[i], new Vector2(50, 50 * i), Color.White);
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
