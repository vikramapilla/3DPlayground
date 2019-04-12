using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace DodgeBall
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;

        Texture2D gameOverSprite;
        Texture2D restartSprite;
        Texture2D creditSprite;

        static Ball ball;
        static Enemy enemy;
        static Coin coin;
        static Button button;

        public  Vector3 cameraPosition = new Vector3(0, 8, -14);
        public  Vector3 cameraLookAtVector = Vector3.Zero;
        public  Vector3 cameraUpVector = Vector3.UnitZ;
        public  float fieldOfView = MathHelper.PiOver4;
        public  float nearClipPlane = 1;
        public  float farClipPlane = 100;

        public static Matrix viewMatrix;
        public static Matrix projectionMatrix;

        Model playGround, playGroundWall;

        Song backGroundMusic;
        static SoundEffect coinSound, enemySound, wallSound, gameOverSound;

        static Rectangle playerSquare = new Rectangle(0, 0, 1, 1);
        static Rectangle rightWall = new Rectangle(-6, 0, 1, 12);
        static Rectangle leftWall = new Rectangle(6, 0, 1, 12);

        static Random random = new Random();


        static float aspectRatio;
        static float angle = 0f;
        static float ratio = 0.25f;
        static int timeToSpawn = 0;
        static int timeToSpawnCheck = 0;
        static int score = 0;

        static bool spawnFlag = false;
        static bool randFlag = false;
        static bool playerMovementFlag = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            ball = new Ball();
            enemy = new Enemy();
            coin = new Coin();
            button = new Button();

            ball.initialize(Content);
            enemy.initialize(Content);
            coin.initialize(Content);
            button.initialize(GraphicsDevice.Viewport.Width/2);



            aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;

            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraLookAtVector, cameraUpVector);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("Fonts/Score");
            gameOverSprite = Content.Load<Texture2D>("Image/GameOver");
            restartSprite = Content.Load<Texture2D>("Image/Restart");
            creditSprite = Content.Load<Texture2D>("Image/Credits");

            playGround = Content.Load<Model>("Models/GameZoneGroundLevel_00");
            playGroundWall = Content.Load<Model>("Models/GameZoneWall");

            backGroundMusic = Content.Load<Song>("Audio/BackgroundMusic_01");
            coinSound = Content.Load<SoundEffect>("Audio/CoinExplosion");
            enemySound = Content.Load<SoundEffect>("Audio/EnemyExplosion");
            wallSound = Content.Load<SoundEffect>("Audio/PlayerExplosion");
            gameOverSound = Content.Load<SoundEffect>("Audio/GameOver");
            MediaPlayer.Play(backGroundMusic);
            MediaPlayer.Volume = 0.25f;

        }

        protected override void UnloadContent()
        {

        }


        private static void reset()
        {
            angle = 0f;
            ratio = 0.25f;
            timeToSpawn = 0;
            timeToSpawnCheck = 0;
            score = 0;

            spawnFlag = false;
            randFlag = false;
            playerMovementFlag = true;
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
            

            isWallCollision();
            

            Matrix playerMatrix = Matrix.CreateTranslation(ball.playerPosition);
            Matrix coinMatrix = Matrix.CreateTranslation(new Vector3(5.5f, 0, 5.5f));

            if (enemy.IsCollision(ball.ballModel, playerMatrix))
            {
                ball.playerPosition = Vector3.Zero;
                enemySound.Play();
                gameOverSound.Play();
                MediaPlayer.Stop();
                playerMovementFlag = false;
                this.IsMouseVisible = true;
            }

            enemy.enemyMovement();
            getScore(playerMatrix);

            if (button.isButtonClicked())
            {
                reset();
                this.IsMouseVisible = false;
                MediaPlayer.Play(backGroundMusic);
            }

            base.Update(gameTime);
        }

        private static void getScore(Matrix playerMatrix)
        {
            if (randFlag)
            {
                timeToSpawn = random.Next(90, 180);
                randFlag = false;
            }
            timeToSpawnCheck++;

            if (timeToSpawnCheck > timeToSpawn)
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

            if (coin.IsCollision(ball.ballModel, playerMatrix))
            {
                score++;
                coinSound.Play();
            }
        }

        private static void isWallCollision()
        {
            if (playerSquare.Intersects(rightWall))
            {
                ball.playerPosition.X += 0.75f;
                wallSound.Play(0.45f, 0.5f, 0.5f);
            }
            if (playerSquare.Intersects(leftWall))
            {
                ball.playerPosition.X -= 0.75f;
                wallSound.Play(0.45f, 0.5f, 0.5f);
            }

            //Problem with Z axis collision, that's why co-ordinates are used
            if (ball.playerPosition.Z >= 6)
            {
                ball.playerPosition.Z -= 0.75f;
                wallSound.Play(0.45f, 0.5f, 0.5f);
            }
            if (ball.playerPosition.Z <= -6)
            {
                ball.playerPosition.Z += 0.75f;
                wallSound.Play(0.45f, 0.5f, 0.5f);
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            if (playerMovementFlag)
            {
                ball.Draw();
            }
            enemy.Draw();
            coin.Draw();

            DrawModel(playGround, Matrix.CreateTranslation(Vector3.Zero));
            DrawModel(playGroundWall, Matrix.CreateTranslation(Vector3.Zero));
            
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(50, 50), Color.ForestGreen);

            if (!playerMovementFlag)
            {
                Vector2 screenCenter = new Vector2(GraphicsDevice.Viewport.Bounds.Width / 2, GraphicsDevice.Viewport.Bounds.Height / 2);
                Vector2 textureCenter = new Vector2(gameOverSprite.Width / 2, gameOverSprite.Height / 2);
                if (ratio < 0.65f)
                {
                    ratio += 0.001f;
                }
                spriteBatch.Draw(gameOverSprite, screenCenter, null, Color.White, 0f, textureCenter, 1f * ratio, SpriteEffects.None, 0f);
                spriteBatch.Draw(restartSprite, button.getRectangle(), Color.Thistle);
                spriteBatch.Draw(creditSprite, new Rectangle(GraphicsDevice.Viewport.Bounds.Width-175, 50, 200, 150), Color.Thistle);
            }
            spriteBatch.End();
            base.Draw(gameTime);
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
