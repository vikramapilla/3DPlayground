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

        public void Draw(Vector3 cameraPosition, float aspectRatio)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                playerPosition += new Vector3(0, 0, 0.05f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                playerPosition -= new Vector3(0, 0, 0.05f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                playerPosition -= new Vector3(0.05f, 0, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                playerPosition += new Vector3(0.05f, 0, 0);
            }

            //Console.WriteLine("Y = {0}", playerPosition.Z);

            foreach (ModelMesh mesh in ballModel.Meshes)
            {
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.World = Matrix.CreateTranslation(playerPosition); ;

                    Vector3 cameraLookAtVector = Vector3.Zero;
                    Vector3 cameraUpVector = Vector3.UnitZ;

                    effect.View = Matrix.CreateLookAt(cameraPosition, cameraLookAtVector, cameraUpVector);

                    float fieldOfView = MathHelper.PiOver4;
                    float nearClipPlane = 1;
                    float farClipPlane = 100;

                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
                }

                mesh.Draw();
            }
        }


        
    }
}
