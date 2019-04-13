using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DodgeBall
{
    class Ball
    {
        static Model ballModel;

        static Vector3 playerPosition;
        static float angle = 0f;
        static Matrix rotMatrix;

        public void initialize(ContentManager contentManager)
        {
            ballModel = loadModel(contentManager);
            playerPosition = new Vector3(0, 0, 0);
            rotMatrix = Matrix.CreateRotationY(0);
        }

        private static Model loadModel(ContentManager contentManager)
        {
            Model enemyModel = contentManager.Load<Model>("Models/3DBall");
            return enemyModel;
        }

        public Vector3 getPos()
        {
            return playerPosition;
        }

        public void setPos(Vector3 vec)
        {
            playerPosition = vec;
        }

        public Model getModel()
        {
            return ballModel;
        }

        public Vector3 Move(Vector3 cameraPosition)
        {
            Vector3 directionVec = (playerPosition - new Vector3(cameraPosition.X, 0, cameraPosition.Z)) / 30;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                playerPosition += directionVec;
                cameraPosition += directionVec;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                playerPosition -= directionVec;
                cameraPosition -= directionVec;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                rotMatrix = Matrix.CreateRotationY(-0.02f);
                cameraPosition = Vector3.Transform(cameraPosition, getCamMatrix());
                angle -= 0.02f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                rotMatrix = Matrix.CreateRotationY(0.02f);
                cameraPosition = Vector3.Transform(cameraPosition, getCamMatrix());
                angle += 0.02f;
            }

            return cameraPosition;
        }

        public void Draw()
        {
            Game1.DrawModel(ballModel, getPlayerMatrix());
        }

        private static Matrix getPlayerMatrix()
        {
            return Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(playerPosition);
        }

        private static Matrix getCamMatrix()
        {
            return Matrix.CreateTranslation(-playerPosition) * rotMatrix * Matrix.CreateTranslation(playerPosition);
        }
    }
}
