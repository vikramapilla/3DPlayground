using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace DodgeBall
{
    class Coin
    {
        Model coinModel;

        Vector3[] coins = new Vector3[200];
        bool[] coinsDraw = new bool[200];

        Random random = new Random();
        
        static float boundary = 5.5f;
        static int coinIndex = 4;

        public void initialize(ContentManager contentManager)
        {
            coinModel = contentManager.Load<Model>("Models/Coins_electric");

            coins[0] = new Vector3(5.5f, 0, 5.5f);
            coins[1] = new Vector3(-5.5f, 0, 5.5f);
            coins[2] = new Vector3(5.5f, 0, -5.5f);
            coins[3] = new Vector3(-5.5f, 0, -5.5f);

            for (int i = 0; i < coinIndex; i++)
            {
                coinsDraw[i] = true;
            }
        }

        public void Draw()
        {
            for (int i = 0; i < coinIndex; i++)
            {
                if (coinsDraw[i])
                {
                    Game1.DrawModel(coinModel, getWorldMatrix(coins[i]));

                }
            }
        }


        private static Matrix getWorldMatrix(Vector3 position)
        {
            Matrix worldMatrix = Matrix.CreateTranslation(position);
            return worldMatrix;
        }


        public void spawnCoin()
        {
            if (coinIndex < 200)
            {
                float x = random.Next(0, 11) - boundary;
                float z = random.Next(0, 11) - boundary;
                coins[coinIndex] = new Vector3(x, 0, z);
                coinsDraw[coinIndex] = true;
                coinIndex++;
            }

        }

        public bool IsCollision(Model ballModel, Matrix ballWorldMatrix)
        {
            for (int i = 0; i < coinIndex; i++)
            {
                if (coinsDraw[i])
                {
                    for (int coinMeshIndex = 0; coinMeshIndex < coinModel.Meshes.Count; coinMeshIndex++)
                    {
                        BoundingSphere coinSphere = coinModel.Meshes[coinMeshIndex].BoundingSphere;
                        coinSphere = coinSphere.Transform(getWorldMatrix(coins[i]));

                        for (int ballMeshIndex = 0; ballMeshIndex < ballModel.Meshes.Count; ballMeshIndex++)
                        {
                            BoundingSphere ballSphere = ballModel.Meshes[ballMeshIndex].BoundingSphere;
                            ballSphere = ballSphere.Transform(ballWorldMatrix);

                            if (coinSphere.Intersects(ballSphere))
                            {
                                coinsDraw[i] = false;
                                return true;
                            }

                        }
                    }
                }
                
            }
            return false;

        }


    }
}
