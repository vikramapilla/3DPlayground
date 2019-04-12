using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace DodgeBall
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        Ball ball;
        Enemy enemy;
        Coin coin;

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

        Random random = new Random();
         

        float aspectRatio;
        float angle = 0f;
        int timeToSpawn = 0;
        int timeToSpawnCheck = 0;

        static bool spawnFlag = false;
        static bool randFlag = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            ball = new Ball();
            enemy = new Enemy();
            coin = new Coin();

            ball.initialize(Content);
            enemy.initialize(Content);
            coin.initialize(Content);

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

            if (randFlag)
            {
                timeToSpawn = random.Next(60, 120);
                randFlag = false;
            }
            timeToSpawnCheck++;

            if(timeToSpawnCheck > timeToSpawn)
            {
                timeToSpawnCheck = 0;
                spawnFlag = true;
            }

            if (spawnFlag)
            {
                coin.spawnCoin();
                spawnFlag = false;
                randFlag = true;
            }
            coin.IsCollision(ball.ballModel, playerMatrix);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkCyan);

            // TODO: Add your drawing code here
            ball.Draw(cameraPosition, aspectRatio);
            enemy.Draw();
            coin.Draw();

            DrawModel(playGround, Vector3.Zero, false);
            DrawModel(playGroundWall, Vector3.Zero, false);
            
            


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

    }

}
