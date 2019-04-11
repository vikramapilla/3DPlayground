using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DodgeBall
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        Ball ball;
        Enemy enemy;

        public  Vector3 cameraPosition = new Vector3(0, 8, -14);
        public  Vector3 cameraLookAtVector = Vector3.Zero;
        public  Vector3 cameraUpVector = Vector3.UnitZ;
        public  float fieldOfView = MathHelper.PiOver4;
        public  float nearClipPlane = 1;
        public  float farClipPlane = 100;

        public static Matrix viewMatrix;
        public static Matrix projectionMatrix;

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
            enemy = new Enemy();
            ball.initialize(Content);
            enemy.initialize(Content);
            base.Initialize();
            aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;

            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraLookAtVector, cameraUpVector);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
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

            Matrix playerMatrix = Matrix.CreateTranslation(ball.playerPosition);
            Matrix coinMatrix = Matrix.CreateTranslation(new Vector3(5.5f, 0, 5.5f));

            if (enemy.IsCollision(ball.ballModel, playerMatrix))
            {
                ball.playerPosition = Vector3.Zero;
            }

            enemy.enemyMovement();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkCyan);

            // TODO: Add your drawing code here
            ball.Draw(cameraPosition, aspectRatio);
            enemy.Draw();
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
                    
                    effect.View = viewMatrix;
                    
                    effect.Projection = projectionMatrix;
                }

                mesh.Draw();
            }
        }

        public static void DrawModel(Model model, Matrix worldMatrix)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.World = worldMatrix;
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                }

                mesh.Draw();
            }
        }



        private bool IsPickUpCollision(Model model1, Matrix world1, Model model2, Matrix world2)
        {
            for (int meshIndex1 = 0; meshIndex1 < model1.Meshes.Count; meshIndex1++)
            {
                BoundingSphere sphere1 = model1.Meshes[meshIndex1].BoundingSphere;
                sphere1 = sphere1.Transform(world1);

                for (int meshIndex2 = 0; meshIndex2 < model2.Meshes.Count; meshIndex2++)
                {
                    BoundingSphere sphere2 = model2.Meshes[meshIndex2].BoundingSphere;
                    sphere2 = sphere2.Transform(world2);

                    if (sphere1.Intersects(sphere2))
                        return true;
                }
            }
            return false;
        }


    }

}
