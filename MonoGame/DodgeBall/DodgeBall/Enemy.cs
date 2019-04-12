using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DodgeBall
{
    class Enemy
    {
        static Model[] enemy = new Model[4];
        static Vector3[] enemyPosition = new Vector3[4];

        static float boundary = 3.5f;
        static float angle = 0f;
        static bool[] sideFlag = new bool[4];

        Random random = new Random();

        public void initialize(ContentManager contentManager)
        {
            for (int i = 0; i < enemy.Length; i++)
            {
                enemy[i] = loadEnemyModel(contentManager);
            }

            enemyPosition[0] = new Vector3(0f, 0, 3.5f);
            enemyPosition[1] = new Vector3(-3.5f, 0, 0f);
            enemyPosition[2] = new Vector3(0f, 0, -3.5f);
            enemyPosition[3] = new Vector3(3.5f, 0, 0f);

            for (int i=0; i<sideFlag.Length; i++)
            {
                sideFlag[i] = true;
            }
        }


        private static Model loadEnemyModel(ContentManager contentManager)
        {
            Model enemyModel = contentManager.Load<Model>("Models/Animal");
            return enemyModel;
        }


        public void enemyMovement()
        {
            if(sideFlag[0])
            {
                enemyPosition[0].X += random.Next(10, 20) /100f;
                if (enemyPosition[0].X >= boundary) sideFlag[0] = false;
            }
            else
            {
                enemyPosition[0].X -= random.Next(10, 20) / 100f;
                if (enemyPosition[0].X <= -boundary) sideFlag[0] = true;
            }

            if (sideFlag[1])
            {
                enemyPosition[1].Z += random.Next(10, 20) / 100f;
                if (enemyPosition[1].Z >= boundary) sideFlag[1] = false;
            }
            else
            {
                enemyPosition[1].Z -= random.Next(10, 20) / 100f;
                if (enemyPosition[1].Z <= -boundary) sideFlag[1] = true;
            }

            if (sideFlag[2])
            {
                enemyPosition[2].X += random.Next(10, 20) / 100f;
                if (enemyPosition[2].X >= boundary) sideFlag[2] = false;
            }
            else
            {
                enemyPosition[2].X -= random.Next(10, 20) / 100f;
                if (enemyPosition[2].X <= -boundary) sideFlag[2] = true;
            }

            if (sideFlag[3])
            {
                enemyPosition[3].Z += random.Next(10, 20) / 100f;
                if (enemyPosition[3].Z >= boundary) sideFlag[3] = false;
            }
            else
            {
                enemyPosition[3].Z -= random.Next(10, 20) / 100f;
                if (enemyPosition[3].Z <= -boundary) sideFlag[3] = true;
            }

            angle += 0.075f;

        }


        public void Draw()
        {
            for(int i=0; i<enemy.Length; i++)
            {
                Game1.DrawModel(enemy[i], getWorldMatrix(i));
            }
        }


        private static Matrix getWorldMatrix(int i)
        {
            Matrix worldMatrix = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(enemyPosition[i]);
            return worldMatrix;
        }


        public bool IsCollision(Model ballModel, Matrix ballWorldMatrix)
        {
            for (int i = 0; i < enemy.Length; i++)
            {
                for (int enemyMeshIndex = 0; enemyMeshIndex < enemy[i].Meshes.Count; enemyMeshIndex++)
                {
                    BoundingSphere enemySphere = enemy[i].Meshes[enemyMeshIndex].BoundingSphere;
                    enemySphere = enemySphere.Transform(getWorldMatrix(i));

                    for (int ballMeshIndex = 0; ballMeshIndex < ballModel.Meshes.Count; ballMeshIndex++)
                    {
                        BoundingSphere ballSphere = ballModel.Meshes[ballMeshIndex].BoundingSphere;
                        ballSphere = ballSphere.Transform(ballWorldMatrix);

                        if (enemySphere.Intersects(ballSphere))
                            return true;
                    }
                }
            }
            return false;
            
        }

    }
}
