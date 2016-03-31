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
        Texture2D StartP, StartUP;
    }
}
