using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3DPlayground
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        //Camera Declaration
        Vector3 cameraPosition;
        Vector3 cameraTarget;

        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        //Game Objects
        Model playerModel;

        //Position
        Vector3 playerPosition;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            base.Initialize();

            //Camera Setup

            cameraTarget = new Vector3(0f, 0f, 0f);
            cameraPosition = new Vector3(0f, 0f, 10f);

            worldMatrix = Matrix.CreateWorld(cameraPosition, Vector3.Forward, Vector3.Up);
            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(45f), 
                               graphics.GraphicsDevice.Viewport.AspectRatio,
                               0.1f, 10f);

            worldMatrix = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.UnitY);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);
        }

        protected override void LoadContent()
        {
            playerModel = Content.Load<Model>("3DBall");
            playerPosition = new Vector3(0f, 0f, 0f);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                cameraPosition.X += 0.1f;
                cameraTarget.X += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                cameraPosition.X -= 0.1f;
                cameraTarget.X -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                cameraPosition.Y += 0.1f;
                cameraTarget.Y += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                cameraPosition.Y -= 0.1f;
                cameraTarget.Y -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                cameraPosition.Z += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                cameraPosition.Z -= 0.1f;
            }
            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);

            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                playerPosition += new Vector3(0, 0.01f, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                playerPosition -= new Vector3(0, 0.01f, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                playerPosition += new Vector3(0.01f, 0, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                playerPosition -= new Vector3(0.01f, 0, 0);
            }
            worldMatrix = Matrix.CreateTranslation(playerPosition);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            DrawModel(playerModel, worldMatrix, viewMatrix, projectionMatrix);

            base.Draw(gameTime);
        }


        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }



    }
}
