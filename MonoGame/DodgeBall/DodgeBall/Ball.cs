using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace DodgeBall
{
    class Ball
    {
        public Model ballModel;

        public Vector3 playerPosition;

        public void initialize(ContentManager contentManager)
        {
            ballModel = contentManager.Load<Model>("Models/3DBall");
            playerPosition = new Vector3(0, 0, 0);
        }

        public void Draw()
        {

            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                playerPosition += new Vector3(0, 0, 0.1f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                playerPosition -= new Vector3(0, 0, 0.1f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                playerPosition -= new Vector3(0.1f, 0, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                playerPosition += new Vector3(0.1f, 0, 0);
            }

            Game1.DrawModel(ballModel, Matrix.CreateTranslation(playerPosition));

        }


    }
}
