﻿using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WindowsGame1
{
    public class Player
    {
        public Model model;
        private Model sphereModel;
        public Vector3 position;
        private bool[] itemActive;
        private int directionId;
        private int playerindex;
        public float rotationy;
        public double faktorz;
        public double faktory;
        public float speed;
        private float currentSpeed;
        public float power;
        public float mass;
        public CollisionSphere[] sphere;
        private bool dashing;
        public CollisionManager collisionManager;
        private CharacterManager characterManager;

        public Player(Vector3 spawn, float spawnrotation, int playerindex, Model boundingSphere, CollisionManager collisionManager, CharacterManager characterManager)
        {
            this.characterManager = characterManager;
            this.position = spawn;
            this.playerindex = playerindex;
            this.model = characterManager.gibStruct(playerindex).model;
            this.sphereModel = boundingSphere;
            this.sphere = characterManager.gibStruct(playerindex).spheres;
            this.rotationy = spawnrotation;
            this.collisionManager = collisionManager;

            for (int i = 0; i < this.sphere.Length; i++)
            {
                this.sphere[i].setCenterPos(new Vector3(spawn.X + this.sphere[i].getPosToModel().X, 1.2f, spawn.Z + this.sphere[i].getPosToModel().Z));
                this.sphere[i].setAngleToModel(getAngle2Dim(this.sphere[i].getCenterPos(), this.position));
                this.sphere[i].setRadius(Math.Sqrt(Math.Pow(position.X - sphere[i].getCenterPos().X,2) + Math.Pow(position.Z - sphere[i].getCenterPos().Z,2)));
                double radius = sphere[i].getRadius();
                sphere[i].setCenterPos(new Vector3(
                               (float)(position.X + (Math.Cos(sphere[i].getAngleToModel() + rotationy) * radius)),
                               sphere[i].getCenterPos().Y,
                               (float)(position.Z + (-Math.Sin(sphere[i].getAngleToModel() + rotationy) * radius))));
            }          
            speed = 0.1f;
            mass = 0.05f;
            dashing = false;
            power = 1f;
            directionId = 5;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            for (int i = 0; i < sphere.Length; i++)
            {
                Matrix World = Matrix.Identity * Matrix.CreateTranslation(sphere[i].getCenterPos());
                foreach (ModelMesh sphereMesh in sphereModel.Meshes)
                {
                    foreach (BasicEffect effect in sphereMesh.Effects)
                    {
                        effect.World = World;
                        effect.View = view;
                        effect.Projection = projection;
                        effect.EnableDefaultLighting();
                    }
                    sphereMesh.Draw();
                }
            }

            Matrix world = Matrix.Identity * Matrix.CreateRotationY(rotationy) * Matrix.CreateTranslation(position);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect basic in mesh.Effects)
                {
                    basic.World = world;
                    basic.View = view;
                    basic.Projection = projection;
                    basic.EnableDefaultLighting();
                    basic.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    basic.Alpha = 0.5f;
                }
                mesh.Draw();
            }
        }

        public void Update(bool canFall)
        {
            speed = 0.1f;
            directionId = 4;
            currentSpeed = 0f;
           
            if (!canFall)
            {
                position.Y -= 0.1f;
                for (int i = 0; i < sphere.Length; i++)
                {
                    sphere[i].setCenterPos(new Vector3(sphere[i].getCenterPos().X, sphere[i].getCenterPos().Y - 0.1f, sphere[i].getCenterPos().Z));
                }
            }
            if (playerindex == 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A) || GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
                {
                    directionId = 3;
                    if(collisionManager.checkCanRotateLeft(this, position))
                    {
                    rotationy += 0.01f; 
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D) || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
                {
                    directionId = 1;
                    if (collisionManager.checkCanRotateRight(this, position))
                    {
                        rotationy -= 0.01f;

                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.W) || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
                {
                    directionId = 0;
                    if (collisionManager.canWalkForward(this))
                    {
                        currentSpeed -= speed;
                        if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && !dashing)
                        {
                            currentSpeed *= 50f;
                        }
                    }
                      
                }

                if (Keyboard.GetState().IsKeyDown(Keys.S) || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
                {
                    directionId = 2;
                    if (collisionManager.canWalkBackward(this))
                    {
                        currentSpeed += speed;
                    }
                }
                     
            }

            if (playerindex == 1)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    directionId = 3;
                    rotationy += 0.01f;

                    for (int i = 0; i < sphere.Length; i++)
                    {
                        double radius = sphere[i].getRadius();
                        sphere[i].setCenterPos(new Vector3(
                            (float)(position.X + (Math.Cos(sphere[i].getAngleToModel() + rotationy) * radius)),
                            sphere[i].getCenterPos().Y,
                            (float)(position.Z + (-Math.Sin(sphere[i].getAngleToModel() + rotationy) * radius))));
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    directionId = 1;
                    rotationy -= 0.01f;

                    for (int i = 0; i < sphere.Length; i++)
                    {
                        double radius = sphere[i].getRadius();
                        sphere[i].setCenterPos(new Vector3(
                            (float)(position.X + (Math.Cos(rotationy + sphere[i].getAngleToModel()) * radius)),
                            sphere[i].getCenterPos().Y,
                            (float)(position.Z + (-Math.Sin(rotationy + sphere[i].getAngleToModel()) * radius))));
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    directionId = 0;
                    if (collisionManager.canWalkForward(this))
                    {
                        position.Z -= (float)(speed * Math.Cos(rotationy));
                        position.X -= (float)(speed * Math.Sin(rotationy));
                        for (int i = 0; i < sphere.Length; i++)
                        {
                            sphere[i].setCenterPos(new Vector3(
                            (float)(sphere[i].getCenterPos().X - speed * Math.Sin(rotationy)),
                            sphere[i].getCenterPos().Y,
                            (float)(sphere[i].getCenterPos().Z - speed * Math.Cos(rotationy))));
                        }
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    directionId = 2;
                    if (collisionManager.canWalkBackward(this))
                    {
                        position.Z += (float)(speed * Math.Cos(rotationy));
                        position.X += (float)(speed * Math.Sin(rotationy));
                        for (int i = 0; i < sphere.Length; i++)
                        {
                            sphere[i].setCenterPos(new Vector3(
                            (float)(sphere[i].getCenterPos().X + speed * Math.Sin(rotationy)),
                            sphere[i].getCenterPos().Y,
                            (float)(sphere[i].getCenterPos().Z + speed * Math.Cos(rotationy))));
                        }
                    }
                }
            }
             
            if (playerindex == 2)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.J))
                {
                    directionId = 3;
                    rotationy += 0.01f;

                    for (int i = 0; i < sphere.Length; i++)
                    {
                        double radius = sphere[i].getRadius();
                        sphere[i].setCenterPos(new Vector3(
                            (float)(position.X + (Math.Cos(sphere[i].getAngleToModel() + rotationy) * radius)),
                            sphere[i].getCenterPos().Y,
                            (float)(position.Z + (-Math.Sin(sphere[i].getAngleToModel() + rotationy) * radius))));
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.L))
                {
                    directionId = 1;
                    rotationy -= 0.01f;

                    for (int i = 0; i < sphere.Length; i++)
                    {
                        double radius = sphere[i].getRadius();
                        sphere[i].setCenterPos(new Vector3(
                            (float)(position.X + (Math.Cos(rotationy + sphere[i].getAngleToModel()) * radius)),
                            sphere[i].getCenterPos().Y,
                            (float)(position.Z + (-Math.Sin(rotationy + sphere[i].getAngleToModel()) * radius))));
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.I))
                {
                    directionId = 0;
                    if (collisionManager.canWalkForward(this))
                    {
                        position.Z -= (float)(speed * Math.Cos(rotationy));
                        position.X -= (float)(speed * Math.Sin(rotationy));
                        for (int i = 0; i < sphere.Length; i++)
                        {
                            sphere[i].setCenterPos(new Vector3(
                            (float)(sphere[i].getCenterPos().X - speed * Math.Sin(rotationy)),
                            sphere[i].getCenterPos().Y,
                            (float)(sphere[i].getCenterPos().Z - speed * Math.Cos(rotationy))));
                        }
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.K))
                {
                    directionId = 2;
                    if (collisionManager.canWalkBackward(this))
                    {
                        position.Z += (float)(speed * Math.Cos(rotationy));
                        position.X += (float)(speed * Math.Sin(rotationy));
                        for (int i = 0; i < sphere.Length; i++)
                        {
                            sphere[i].setCenterPos(new Vector3(
                            (float)(sphere[i].getCenterPos().X + speed * Math.Sin(rotationy)),
                            sphere[i].getCenterPos().Y,
                            (float)(sphere[i].getCenterPos().Z + speed * Math.Cos(rotationy))));
                        }
                    }
                }
            }

            if (playerindex == 3)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                {
                    directionId = 3;
                    rotationy += 0.01f;

                    for (int i = 0; i < sphere.Length; i++)
                    {
                        double radius = sphere[i].getRadius();
                        sphere[i].setCenterPos(new Vector3(
                            (float)(position.X + (Math.Cos(sphere[i].getAngleToModel() + rotationy) * radius)),
                            sphere[i].getCenterPos().Y,
                            (float)(position.Z + (-Math.Sin(sphere[i].getAngleToModel() + rotationy) * radius))));
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
                {
                    directionId = 1;
                    rotationy -= 0.01f;

                    for (int i = 0; i < sphere.Length; i++)
                    {
                        double radius = sphere[i].getRadius();
                        sphere[i].setCenterPos(new Vector3(
                            (float)(position.X + (Math.Cos(rotationy + sphere[i].getAngleToModel()) * radius)),
                            sphere[i].getCenterPos().Y,
                            (float)(position.Z + (-Math.Sin(rotationy + sphere[i].getAngleToModel()) * radius))));
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
                {
                    directionId = 0;
                    if (collisionManager.canWalkForward(this))
                    {
                        position.Z -= (float)(speed * Math.Cos(rotationy));
                        position.X -= (float)(speed * Math.Sin(rotationy));
                        for (int i = 0; i < sphere.Length; i++)
                        {
                            sphere[i].setCenterPos(new Vector3(
                            (float)(sphere[i].getCenterPos().X - speed * Math.Sin(rotationy)),
                            sphere[i].getCenterPos().Y,
                            (float)(sphere[i].getCenterPos().Z - speed * Math.Cos(rotationy))));
                        }
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.NumPad5))
                {
                    directionId = 2;
                    if (collisionManager.canWalkBackward(this))
                    {
                        position.Z += (float)(speed * Math.Cos(rotationy));
                        position.X += (float)(speed * Math.Sin(rotationy));
                        for (int i = 0; i < sphere.Length; i++)
                        {
                            sphere[i].setCenterPos(new Vector3(
                            (float)(sphere[i].getCenterPos().X + speed * Math.Sin(rotationy)),
                            sphere[i].getCenterPos().Y,
                            (float)(sphere[i].getCenterPos().Z + speed * Math.Cos(rotationy))));
                        }
                    }
                }
            }
            calculateCollisions();
        }

        public void calculateCollisions()
        {
            List<int> collisionsWithPlayer = new List<int>();
            List<Collision>[] collisions = collisionManager.checkCollision(this);
            if (directionId == 0 || directionId == 2)
            {
                foreach (Collision currentCollision in collisions[directionId])
                {
                    if (!(collisionsWithPlayer.Contains(currentCollision.getEnemyPlayerId())))
                    {
                        float changeSpeed = speed - currentCollision.getEnemyMass();
                        currentSpeed -= changeSpeed;
                        if (currentSpeed < 0)
                        {
                            currentSpeed = 0;
                        }
                    }
                }
            }

            movePlayer();
            collisionsWithPlayer.Clear();

            for (int i = 0; i < collisions.Length; i++)
            {
                foreach (Collision currentCollision in collisions[i])
                {
                    if (!(collisionsWithPlayer.Contains(currentCollision.getEnemyPlayerId())))
                    {
                        collisionsWithPlayer.Add(currentCollision.getEnemyPlayerId());
                        float enemyPower = currentCollision.getEnemyPower() - mass;
                        if (enemyPower > 0)
                        {
                            movePlayer(enemyPower, currentCollision.getEnemyRotation());
                        }
                    }
                }
            }
        }


                        





        public double getAngle2Dim(Vector3 spherePos, Vector3 modelPos)
        {
            Vector2 sphereVector = new Vector2(spherePos.X - modelPos.X, spherePos.Z - modelPos.Z);
            double angle = (Vector2.Dot(new Vector2(1, 0), sphereVector)) / (Math.Sqrt(1) * Math.Sqrt(Math.Pow(sphereVector.X, 2) + Math.Pow(sphereVector.Y, 2)));
            if (spherePos.Z-position.Z > 0)
            {
                return (MathHelper.ToRadians(360) - Math.Acos(angle));
            }
            return Math.Acos(angle);
        }

        public int getPlayerIndex()
        {
            return playerindex;
        }

        public CollisionSphere[] getCollisionSpheres()
        {
            return sphere;
        }

        public float getMass()
        {
            return mass;
        }

        private void movePlayer()
        {
            position.Z += (float)(currentSpeed * Math.Cos(rotationy));
            position.X += (float)(currentSpeed * Math.Sin(rotationy));
            for (int i = 0; i < sphere.Length; i++)
            {
                sphere[i].setCenterPos(new Vector3(
                (float)(sphere[i].getCenterPos().X + currentSpeed * Math.Sin(rotationy)),
                sphere[i].getCenterPos().Y,
                (float)(sphere[i].getCenterPos().Z + currentSpeed * Math.Cos(rotationy))));
            }
            for (int i = 0; i < sphere.Length; i++)
            {
                double radius = sphere[i].getRadius();
                sphere[i].setCenterPos(new Vector3(
                    (float)(position.X + (Math.Cos(sphere[i].getAngleToModel() + rotationy) * radius)),
                    sphere[i].getCenterPos().Y,
                    (float)(position.Z + (-Math.Sin(sphere[i].getAngleToModel() + rotationy) * radius))));
            }



        }

        private void movePlayer(float power, float rotation)
        {
            position.Z += (float)(power * Math.Cos(rotation));
            position.X += (float)(power * Math.Sin(-rotation));
            for (int i = 0; i < sphere.Length; i++)
            {
                sphere[i].setCenterPos(new Vector3(
                (float)(sphere[i].getCenterPos().X + power * Math.Sin(rotation)),
                sphere[i].getCenterPos().Y,
                (float)(sphere[i].getCenterPos().Z + power * Math.Cos(-rotation))));
            }
        }



       

    }
}
