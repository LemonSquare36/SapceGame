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
    enum MenuType { MainMenu, Options }

    class Menu
    {
        Main main;
        public MenuType type = MenuType.MainMenu;
        MouseState mouse;
        Button start;
        Texture2D  background, StartP, StartUP;


        public Menu(Main main)
        {
            this.main = main;
        }
        public void LoadContent()
        {
            background = Main.GameContent.Load<Texture2D>("Sprites/maxresdefault");
            StartP = Main.GameContent.Load<Texture2D>("Menu/PlayButtonP");
            StartUP = Main.GameContent.Load<Texture2D>("Menu/PlayButtonUP");
            mouse = Mouse.GetState();

            switch (type)
            {
                case MenuType.MainMenu:
                    start = new Button(new Vector2(650, 400), 100, 50, 1, mouse, StartUP, StartP, 800, 480);
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
                    break;

                default:
                    break;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            switch (type)
            {
                case MenuType.MainMenu:
                    spriteBatch.Draw(start.Texture, start.Position, Color.White);
                    break;
                    
                default:
                    break;
            }
        }
    }
}
