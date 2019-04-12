using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace DodgeBall
{
    class Button
    {

        static Rectangle buttonPostion;
        static MouseState mouseState;

        public void initialize(int x)
        {
            buttonPostion = new Rectangle(x-90, 50, 180, 50);
        }

        public Rectangle getRectangle()
        {
            return buttonPostion;
        }

        public bool isButtonClicked()
        {

            mouseState = Mouse.GetState();

            if(buttonPostion.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed)
            {
                return true;
            }

            return false;
        }

    }
}
