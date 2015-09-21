using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WindowsGame1
{
    public class Player
    {
       
        private Model model;
        private Model sphereModel;
        private Vector3 position;
        
        private float[] cornerAngles;
        private bool[] itemActive;
        private bool isAlive;
        private int directionId;
        private int playerindex;
        private float rotationy;
        private float speed;
        private float currentSpeed;
        private float power;
        private float mass;
        private CollisionSphere[] sphere;
        private bool dashing;
        private CollisionManager collisionManager;
        private int modelId;
        private float rotationSpeed;
        private float dashPower;
        private double dashCountdown;
        private TimeSpan dashTime;
        public float currentDashPower;
        


        public Player(Vector3 spawn, float spawnrotation, int playerindex, CollisionManager collisionManager, CharacterManager.Moebel data,Model sphereModel)
        {
            this.position = spawn;
            position.Y= data.yPosition;
            this.rotationy = spawnrotation;
            this.playerindex = playerindex;
            this.collisionManager = collisionManager;

            this.model = data.model;
            this.modelId = data.modelId;
            sphere = new CollisionSphere[data.spheres.Length];
            
            
            for (int i = 0; i < data.spheres.Length; i++)
            {
                sphere[i] = new CollisionSphere(data.spheres[i].getPosToModel(), data.spheres[i].getDirectionIndex());
            }

            this.speed = data.speed;
            this.rotationSpeed = data.rotationSpeed;
            this.dashPower = data.dashpower;
            this.dashCountdown = data.dashCountdown;
            this.power = data.power;
            this.mass = data.mass;  
            this.cornerAngles = data.angle;
            this.sphereModel = sphereModel;
            
            for (int i = 0; i < this.sphere.Length; i++)
            {
                this.sphere[i].setCenterPos(new Vector3(position.X + this.sphere[i].getPosToModel().X, 1.2f, position.Z + this.sphere[i].getPosToModel().Z));
                this.sphere[i].setAngleToModel(getAngle2Dim(this.sphere[i].getCenterPos(), this.position));
                this.sphere[i].setRadius(Math.Sqrt(Math.Pow(position.X - sphere[i].getCenterPos().X, 2) + Math.Pow(position.Z - sphere[i].getCenterPos().Z, 2)));
                double radius = sphere[i].getRadius();
                this.sphere[i].setCenterPos(new Vector3(
                               (float)(position.X + (Math.Cos(sphere[i].getAngleToModel() + rotationy) * radius)),
                               sphere[i].getCenterPos().Y,
                               (float)(position.Z + (-Math.Sin(sphere[i].getAngleToModel() + rotationy) * radius))));
            }
            
            dashing = false;
            directionId = 4;
            isAlive = true;
            dashTime = new TimeSpan();
            
            
            
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

        public void Update(GameTime gameTime)
        {
            if (isAlive)
            {
                directionId = 4;
                currentSpeed = 0;
                currentDashPower = 0f;

                if (!collisionManager.canFall(this))
                {
                    position.Y -= 0.1f;
                    for (int i = 0; i < sphere.Length; i++)
                    {
                        sphere[i].setCenterPos(new Vector3(sphere[i].getCenterPos().X, sphere[i].getCenterPos().Y - 0.1f, sphere[i].getCenterPos().Z));
                    }

                    if (collisionManager.outOfGame(this))
                        isAlive = false;
                }



                if (playerindex == 0)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.A) || GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
                    {
                        directionId = 3;
                        if (collisionManager.checkCanRotateLeft(this, position))
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
                            if ((Keyboard.GetState().IsKeyDown(Keys.Tab) && !dashing) || (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && !dashing))
                            {
                                currentSpeed *= 5f;
                                currentDashPower = dashPower;
                                dashing = true;
                                dashTime = gameTime.TotalGameTime;

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
                    if (Keyboard.GetState().IsKeyDown(Keys.J) || GamePad.GetState(PlayerIndex.Three).DPad.Left == ButtonState.Pressed)
                    {
                        directionId = 3;
                        if (collisionManager.checkCanRotateLeft(this, position))
                        {
                            rotationy += 0.01f;
                        }
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.L) || GamePad.GetState(PlayerIndex.Three).DPad.Right == ButtonState.Pressed)
                    {
                        directionId = 1;
                        rotationy -= 0.01f;

                        if (collisionManager.checkCanRotateRight(this, position))
                        {
                            rotationy -= 0.01f;
                        }
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.I) || GamePad.GetState(PlayerIndex.Three).DPad.Up == ButtonState.Pressed)
                    {
                        directionId = 0;
                        if (collisionManager.canWalkForward(this))
                        {

                            currentSpeed -= speed;
                            if ((Keyboard.GetState().IsKeyDown(Keys.Space) && !dashing) || (GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed && !dashing))
                            {
                                currentSpeed *= 5f;
                                power += dashPower;
                                dashing = false;
                                dashTime = gameTime.TotalGameTime;
                            }
                        }
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.K) || GamePad.GetState(PlayerIndex.Three).DPad.Down == ButtonState.Pressed)
                    {
                        directionId = 2;
                        if (collisionManager.canWalkBackward(this))
                        {

                            currentSpeed += speed;

                        }
                    }
                }

                if (playerindex == 2)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Left) || GamePad.GetState(PlayerIndex.Two).DPad.Left == ButtonState.Pressed)
                    {
                        directionId = 3;
                        if (collisionManager.checkCanRotateLeft(this, position))
                        {
                            rotationy += 0.01f;
                        }
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Right) || GamePad.GetState(PlayerIndex.Two).DPad.Right == ButtonState.Pressed)
                    {
                        directionId = 1;

                        if (collisionManager.checkCanRotateRight(this, position))
                        {
                            rotationy -= 0.01f;
                        }
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.Two).DPad.Up == ButtonState.Pressed)
                    {
                        directionId = 0;
                        if (collisionManager.canWalkForward(this))
                        {

                            currentSpeed -= speed;
                            if ((Keyboard.GetState().IsKeyDown(Keys.RightControl) && !dashing) || (GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed && !dashing))
                            {
                                currentSpeed *= 5f;
                                power += dashPower;
                                dashing = false;
                                dashTime = gameTime.TotalGameTime;
                            }
                        }
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.Two).DPad.Down == ButtonState.Pressed)
                    {
                        directionId = 2;
                        if (collisionManager.canWalkBackward(this))
                        {
                            currentSpeed += speed;
                        }

                    }
                }


                if (playerindex == 3)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.NumPad4) || GamePad.GetState(PlayerIndex.Four).DPad.Left == ButtonState.Pressed)
                    {
                        directionId = 3;
                        if (collisionManager.checkCanRotateLeft(this, position))
                        {
                            rotationy += 0.01f;
                        }

                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.NumPad6) || GamePad.GetState(PlayerIndex.Four).DPad.Right == ButtonState.Pressed)
                    {
                        directionId = 1;
                        if (collisionManager.checkCanRotateRight(this, position))
                        {
                            rotationy -= 0.01f;
                        }
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.NumPad8) || GamePad.GetState(PlayerIndex.Four).DPad.Up == ButtonState.Pressed)
                    {
                        directionId = 0;
                        if (collisionManager.canWalkForward(this))
                        {

                            currentSpeed -= speed;
                            if ((Keyboard.GetState().IsKeyDown(Keys.NumPad0) && !dashing) || (GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed && !dashing))
                            {
                                currentSpeed *= 5f;
                                power += dashPower;
                                dashing = false;
                                dashTime = gameTime.TotalGameTime;
                            }
                        }
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.NumPad5) || GamePad.GetState(PlayerIndex.Four).DPad.Down == ButtonState.Pressed)
                    {
                        directionId = 2;
                        if (collisionManager.canWalkBackward(this))
                        {

                            currentSpeed += speed;
                        }


                    }
                }
                
                movePlayer();
                checkCanDash(gameTime);
            }
        }

     /*   public void calculateCollisions()
        {
            allEnemyMass = 0;
            List<Collision>[] collisions = collisionManager.checkCollision(this);
            for (int i = 0; i < collisions.Length; i++)
            {
                foreach (Collision currentCollision in collisions[i])
                {

                    if (directionId == i)
                    {
                        allEnemyMass += currentCollision.getEnemyMass();
                    }
                    if (currentCollision.getEnemyDirection() != directionId)
                    {
                        if (currentCollision.getEnemyPower() > mass)
                        {
                            changePosition.X += (float)(currentCollision.getEnemySpeed() *
                                ((currentCollision.getEnemyPower() - mass) / currentCollision.getEnemyPower()) *
                                Math.Sin(currentCollision.getEnemyRotation()));

                            changePosition.Z += (float)(currentCollision.getEnemySpeed() *
                              ((currentCollision.getEnemyPower() - mass) / currentCollision.getEnemyPower()) *
                              Math.Cos(currentCollision.getEnemyRotation()));
                        }
                    }
                    else
                    {
                        if (currentCollision.getEnemyDirection() != 4 && currentCollision.getEnemySpeed() > currentSpeed)
                        {
                            changePosition.X += (float)((currentCollision.getEnemySpeed() - currentSpeed) *
                                ((currentCollision.getEnemyPower() - mass) / currentCollision.getEnemyPower()) *
                                Math.Sin(currentCollision.getEnemyRotation()));

                            changePosition.Z += (float)((currentCollision.getEnemySpeed() - currentSpeed) *
                                ((currentCollision.getEnemyPower() - mass) / currentCollision.getEnemyPower()) *
                                Math.Cos(currentCollision.getEnemyRotation()));
                        }
                    }
                }
            }
            
            if (power > allEnemyMass)
            {
                changePosition.X += (float)(currentSpeed * ((power - allEnemyMass) / power) * Math.Sin(rotationy));
                changePosition.Z += (float)(currentSpeed * ((power - allEnemyMass) / power) * Math.Cos(rotationy));
            }
        }
        */
        public double getAngle2Dim(Vector3 spherePos, Vector3 modelPos)
        {
            Vector2 sphereVector = new Vector2(spherePos.X - modelPos.X, spherePos.Z - modelPos.Z);
            double angle = (Vector2.Dot(new Vector2(1, 0), sphereVector)) / (Math.Sqrt(1) * Math.Sqrt(Math.Pow(sphereVector.X, 2) + Math.Pow(sphereVector.Y, 2)));
            if (spherePos.Z - position.Z > 0)
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
            Vector3 collisionVector = collisionManager.calculateCollisions(this);
            position += collisionVector;
            for (int i = 0; i < sphere.Length; i++)
            {

                sphere[i].setCenterPos(new Vector3(
                (float)(sphere[i].getCenterPos().X + collisionVector.X),
                sphere[i].getCenterPos().Y,
                (float)(sphere[i].getCenterPos().Z + collisionVector.Z)));
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

        public void checkCanDash(GameTime gameTime)
        {
            if (dashTime + TimeSpan.FromMilliseconds(dashCountdown) <= gameTime.TotalGameTime)
            {
                dashing = false;
            }

           
        }


      
        public TimeSpan getDashTime()
        {
           return (dashTime + TimeSpan.FromMilliseconds(dashCountdown));
            
        }

        public float[] getCornerAngles()
        {
            return cornerAngles;
        }

        public int getDirectionId()
        {
            return directionId;
        }

        public float getCurrentSpeed()
        {
            return currentSpeed;
        }

       
        public bool isDashing()
        {
            return dashing;
        }

        public float getPower()
        {
            return power;
        }

        public int getModelId()
        {
            return modelId;
        }

        public float getRotationY()
        {
            return rotationy;
        }

        public Vector3 getPosition()
        {
            return position;
        }

        public float getcurrentDashPower()
        {
            return currentDashPower;
        }

    }
}
