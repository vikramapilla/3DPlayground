using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DodgeBall
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        Ball ball;
        Vector3 cameraPosition = new Vector3(0, 8, -14);

        Model playGround, playGroundWall;
        Model[] electricCoins = new Model[4];

        Rectangle playerSquare = new Rectangle(0, 0, 1, 1);
        Rectangle rightWall = new Rectangle(-6, 0, 1, 12);
        Rectangle leftWall = new Rectangle(6, 0, 1, 12);

        float aspectRatio;
        float angle = 0f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            ball = new Ball();
            ball.initialize(Content);
            base.Initialize();
        }

        protected override void LoadContent()
        {

            playGround = Content.Load<Model>("Models/GameZoneGroundLevel_00");
            playGroundWall = Content.Load<Model>("Models/GameZoneWall");
            
            for(int i=0; i<electricCoins.Length; i++)
            {
                electricCoins[i] = Content.Load<Model>("Models/Coins_electric");
            }
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            Matrix ballWorlMatrix = Matrix.CreateTranslation(ball.playerPosition);
            Matrix wallWorldMatrix = Matrix.CreateTranslation(Vector3.Zero);

            playerSquare.X = (int)ball.playerPosition.X;
            angle += 0.05f;


            if (playerSquare.Intersects(rightWall))
            {
                ball.playerPosition.X += 2;
            }
            if (playerSquare.Intersects(leftWall))
            {
                ball.playerPosition.X -= 2;
            }

            //Problem with Z axis collision, that's why co-ordinates are used
            if (ball.playerPosition.Z >= 6)
            {
                ball.playerPosition.Z -= 2;
            }
            if (ball.playerPosition.Z <= -6)
            {
                ball.playerPosition.Z += 2;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkCyan);

            // TODO: Add your drawing code here
            aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            ball.Draw(cameraPosition, aspectRatio);
            DrawModel(playGround, Vector3.Zero, false);
            DrawModel(playGroundWall, Vector3.Zero, false);
            
            DrawModel(electricCoins[0], new Vector3(5.5f, 0, 5.5f), true);
            DrawModel(electricCoins[1], new Vector3(-5.5f, 0, 5.5f), true);
            DrawModel(electricCoins[2], new Vector3(5.5f, 0, -5.5f), true);
            DrawModel(electricCoins[3], new Vector3(-5.5f, 0, -5.5f), true);


            base.Draw(gameTime);
        }

        public void DrawModel(Model model, Vector3 modelPosition, bool rotation)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    if (rotation)
                    {
                        effect.World = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(modelPosition);
                    }
                    else
                    {
                        effect.World = Matrix.CreateTranslation(modelPosition);
                    }

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
