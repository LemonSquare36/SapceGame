using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceGame
{
    public struct Button
    {
        private enum ButtonType { Rectangle };

        private float diameter { get; set; }
        private float windowWidth { get; set; }
        private float windowHeight { get; set; }
        private ButtonType type { get; set; }
        private Texture2D button0 { get; set; }

        private event EventHandler buttonPressed;

        public event EventHandler ButtonPressed
        {
            add { buttonPressed += value; }
            remove { buttonPressed -= value; }
        }

        public Vector2 Position
        {
            get
            {
                switch (type)
                {
                    case ButtonType.Rectangle:
                        return new Vector2(Collision.Location.X, Collision.Location.Y);
                    default:
                        return Vector2.Zero;
                }
            }
        }

        public Texture2D Texture
        {
            get { return button0; }
        }

        private int bNum;

        public int ButtonNum
        {
            get { return bNum; }
        }

        private MouseState mouseState;

        /// <summary>
        /// Backing store for Collision.
        /// </summary>
        private Rectangle collision;

        /// <summary>
        /// Rectangle structure.
        /// </summary>
        public Rectangle Collision
        {
            get { return collision; }
            set { collision = value; }
        }

        /// <summary>
        /// Backing Stores for textures.
        /// </summary>
        private Texture2D button1, button2;

        /// <summary>
        /// Set Unpressed Button Texture.
        /// </summary>
        public Texture2D UnpressedButton
        {
            set { button1 = value; }
        }

        /// <summary>
        /// Set Hovered Button Texture.
        /// </summary>
        public Texture2D HoveredButton
        {
            set { button2 = value; }
        }

        /// <summary>
        /// Backing Store for Center of circle.
        /// </summary>
        private Vector2 center;

        /// <summary>
        /// Center of circle.
        /// </summary>
        public Vector2 Center
        {
            get { return center; }
            set { center = value; }
        }

        /// <summary>
        /// Creates a new button for the menu.
        /// </summary>
        /// <param name="position">Position of top left corner.</param>
        /// <param name="width">Width of button in pixels.</param>
        /// <param name="height">Height of button in pixels.</param>
        /// <param name="buttonNum">Number button uses to identify.</param>
        /// <param name="mouse">Mouse state for detection.</param>
        /// <param name="buttonNorm">Ordinary button state.</param>
        /// <param name="buttonHov">Hovered button state.</param>
        public Button(Vector2 position, int width, int height, int buttonNum, MouseState mouse, Texture2D buttonNorm, Texture2D buttonHov, float windowWidth, float windowHeight)
            : this()
        {
            center = Vector2.Zero;
            collision = new Rectangle((int)position.X, (int)position.Y, width, height);
            mouseState = mouse;
            button1 = buttonNorm;
            button2 = buttonHov;
            bNum = buttonNum;
            type = ButtonType.Rectangle;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
        }
        
        public void Update(MouseState mouse)
        {
            mouseState = mouse;
            switch (type)
            {
                case ButtonType.Rectangle:
                    if (Collision.Contains(mouseState.X, mouseState.Y))
                    {
                        button0 = button2;
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            OnButtonPressed();
                        }
                    }
                    else
                    {
                        button0 = button1;
                    }
                    break;

                default:
                    break;
            }
        }

        private void OnButtonPressed()
        {
            if (buttonPressed != null)
            {
                buttonPressed(this, EventArgs.Empty);
            }
        }
    }
}
