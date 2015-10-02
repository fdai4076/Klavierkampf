using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

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
        private bool canMove;
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
        private ItemManager itemManager;
        private int modelId;
        private float rotationSpeed;
        private float dashPower;
        private double dashCountdown;
        private TimeSpan dashTime;
        public float currentDashPower;
        private float speedEffect;
        private float powerEffect;
        private int movingEffect;
        private SoundEffect dashEffect;

        public Player(Vector3 spawn, float spawnrotation, int playerindex, CollisionManager collisionManager, CharacterManager.Moebel data, Model sphereModel, ItemManager itemManager, SoundEffect dashEffect)
        {
            this.position = spawn;
            position.Y = data.yPosition;
            this.rotationy = spawnrotation;
            this.playerindex = playerindex;
            this.collisionManager = collisionManager;
            this.itemManager = itemManager;

            this.model = data.model;
            this.modelId = data.modelId;
            this.dashEffect = dashEffect;
            canMove = true;
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
                this.sphere[i].setCenterPos(new Vector3(position.X + this.sphere[i].getPosToModel().X, 1.1f, position.Z + this.sphere[i].getPosToModel().Z));
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

        //zeichnet die Spieler und die BoundingSpheres
        public void Draw(Matrix view, Matrix projection)
        {/*
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
             }*/

            Matrix world = Matrix.Identity * Matrix.CreateRotationY(rotationy) * Matrix.CreateTranslation(position);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect basic in mesh.Effects)
                {
                    basic.World = world;
                    basic.View = view;
                    basic.Projection = projection;
                    basic.EnableDefaultLighting();
                    //basic.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    //basic.Alpha = 0.5f;
                }
                mesh.Draw();
            }
        }


        public void Update(GameTime gameTime,bool mute)
        {
            //ruft den möglichen Effekt eines Items auf den Spieler ab
            speedEffect = itemManager.getItemEffect(playerindex).speed;
            powerEffect = itemManager.getItemEffect(playerindex).power;
            movingEffect = itemManager.getItemEffect(playerindex).moving;

            //Abfrage ob der Spieler noch auf der Tunierfläche ist
            if (isAlive)
            {
                directionId = 4;
                currentSpeed = 0;
                currentDashPower = 0f;
                ItemManager.ItemsEffect itemEffect = itemManager.getItemEffect(playerindex);

                //wenn der Spieler die Tunierfläche verlässt fällt er
                if (collisionManager.canFall(this))
                {
                    position.Y -= 0.1f;
                    for (int i = 0; i < sphere.Length; i++)
                    {
                        sphere[i].setCenterPos(new Vector3(sphere[i].getCenterPos().X, sphere[i].getCenterPos().Y - 0.1f, sphere[i].getCenterPos().Z));
                    }
                    canMove = false;

                    if (collisionManager.outOfGame(this))
                        isAlive = false;
                }

                //abfrage ob der Spieler sich bewegen kann
                if (canMove)
                {
                    //Steuerung und Dash für Player1
                    if (playerindex == 0)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.A) || GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
                        {
                            rotateLeft();
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.D) || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
                        {
                            rotateRight();
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.W) || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
                        {

                            walkForward();
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.S) || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
                        {

                            walkBackward();
                        }

                        if ((Keyboard.GetState().IsKeyDown(Keys.E) && !dashing) || (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && !dashing))
                        {
                            if (currentSpeed == 0 && movingEffect == -1)
                            {
                                walkBackward();
                            }
                            if (currentSpeed == 0)
                            {
                                walkForward();
                            }
                            dash(gameTime, mute);
                        }
                    }

                    //Steuerung und Dash für Player2
                    if (playerindex == 1)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.H) || GamePad.GetState(PlayerIndex.Two).DPad.Left == ButtonState.Pressed)
                        {
                            rotateLeft();
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.K) || GamePad.GetState(PlayerIndex.Two).DPad.Right == ButtonState.Pressed)
                        {
                            rotateRight();
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.U) || GamePad.GetState(PlayerIndex.Two).DPad.Up == ButtonState.Pressed)
                        {
                            walkForward();
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.J) || GamePad.GetState(PlayerIndex.Two).DPad.Down == ButtonState.Pressed)
                        {
                            walkBackward();
                        }
                        if ((Keyboard.GetState().IsKeyDown(Keys.I) && !dashing) || (GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed && !dashing))
                        {
                            if (currentSpeed == 0 && movingEffect == -1)
                            {
                                walkBackward();
                            }
                            if (currentSpeed == 0)
                            {
                                walkForward();
                            }
                            dash(gameTime, mute);
                        }
                    }

                    //Steuerung und Dash für Player3
                    if (playerindex == 2)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Left) || GamePad.GetState(PlayerIndex.Three).DPad.Left == ButtonState.Pressed)
                        {
                            rotateLeft();
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.Right) || GamePad.GetState(PlayerIndex.Three).DPad.Right == ButtonState.Pressed)
                        {
                            rotateRight();
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.Three).DPad.Up == ButtonState.Pressed)
                        {
                            walkForward();
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.Three).DPad.Down == ButtonState.Pressed)
                        {
                            walkBackward();
                        }
                        if ((Keyboard.GetState().IsKeyDown(Keys.RightControl) && !dashing) || (GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed && !dashing))
                        {
                            if (currentSpeed == 0 && movingEffect == -1)
                            {
                                walkBackward();
                            }
                            if (currentSpeed == 0)
                            {
                                walkForward();
                            }
                            dash(gameTime, mute);
                        }
                    }

                    //Steuerung und Dash für Player4
                    if (playerindex == 3)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad4) || GamePad.GetState(PlayerIndex.Four).DPad.Left == ButtonState.Pressed)
                        {
                            rotateLeft();
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad6) || GamePad.GetState(PlayerIndex.Four).DPad.Right == ButtonState.Pressed)
                        {
                            rotateRight();
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad8) || GamePad.GetState(PlayerIndex.Four).DPad.Up == ButtonState.Pressed)
                        {
                            walkForward();
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad5) || GamePad.GetState(PlayerIndex.Four).DPad.Down == ButtonState.Pressed)
                        {
                            walkBackward();
                        }

                        if ((Keyboard.GetState().IsKeyDown(Keys.NumPad9) && !dashing) || (GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed && !dashing))
                        {
                            if (currentSpeed == 0 && movingEffect == -1)
                            {
                                walkBackward();
                            }
                            if (currentSpeed == 0)
                            {
                                walkForward();
                            }
                            dash(gameTime, mute);
                        }
                    }
                }

                //überprüft Kollisonen und überprüft, ob der Spieler dashen kann
                collisionManager.calculateCollisions(this, directionId, currentSpeed, power, currentDashPower, rotationy);
                checkCanDash(gameTime);
            }
        }

        //gibt den Winkel zwischen der Sphere und dem Model zurück
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

        //gibt den Player Index zurück
        public int getPlayerIndex()
        {
            return playerindex;
        }

        //gibt ein Array der CollisionSpheres zurück
        public CollisionSphere[] getCollisionSpheres()
        {
            return sphere;
        }

        //gibt die Masse zurück
        public float getMass()
        {
            return mass;
        }

    /*    private void movePlayer()
        {
            collisionManager.calculateCollisions(this,directionId,currentSpeed,power,currentDashPower,rotationy);
            position += collisionVector;
         
           for (int i = 0; i < sphere.Length; i++)
            {
                double radius = sphere[i].getRadius();
                sphere[i].setCenterPos(new Vector3(
                    (float)(position.X + (Math.Cos(sphere[i].getAngleToModel() + rotationy) * radius)),
                    sphere[i].getCenterPos().Y,
                    (float)(position.Z + (-Math.Sin(sphere[i].getAngleToModel() + rotationy) * radius))));
            }
            
        }*/

        //bewegt die Collision SPheres
        public void movePlayer(Vector3 directionVector)
        {
            position += directionVector;

           for (int i = 0; i < sphere.Length; i++)
            {
                double radius = sphere[i].getRadius();
                sphere[i].setCenterPos(new Vector3(
                    (float)(position.X + (Math.Cos(sphere[i].getAngleToModel() + rotationy) * radius)),
                    sphere[i].getCenterPos().Y,
                    (float)(position.Z + (-Math.Sin(sphere[i].getAngleToModel() + rotationy) * radius))));
            } 
        }


        public void moveCollisionSpheres(Vector3 collisionVector)
        {
            for (int i = 0; i < sphere.Length; i++)
            {
                sphere[i].setCenterPos(new Vector3(
                (float)(sphere[i].getCenterPos().X + collisionVector.X),
                sphere[i].getCenterPos().Y,
                (float)(sphere[i].getCenterPos().Z + collisionVector.Z)));
            }
        }

        //lässt den Player links rotieren
        public void rotateLeft()
        {
            if (movingEffect == -1)
            {
                directionId = 1;
                if (collisionManager.checkCanRotateRight(this, position))
                {
                    rotationy += (rotationSpeed * movingEffect);
                }
            }
            else
            {
                directionId = 3;
                if (collisionManager.checkCanRotateLeft(this, position))
                {
                    rotationy += (rotationSpeed * movingEffect);
                }
            }
        }

        //lässt den Player rechts rotieren
        public void rotateRight()
        {
            if (movingEffect == -1)
            {
                directionId = 3;
                if (collisionManager.checkCanRotateLeft(this, position))
                {
                    rotationy -= (rotationSpeed * movingEffect);
                }
            }
            else
            {
                directionId = 1;
                if (collisionManager.checkCanRotateRight(this, position))
                {
                    rotationy -= (rotationSpeed * movingEffect);
                }
            }
        }

        //lässt den Player rückwärts laufen
        public void walkBackward()
        {
            if (movingEffect == -1)
            {
                directionId = 0;
                if (collisionManager.canWalk(this, directionId))
                {
                    currentSpeed += (speed * speedEffect * movingEffect);
                }
            }
            else
            {
                directionId = 2;
                if (collisionManager.canWalk(this, directionId))
                {
                    currentSpeed += (speed * speedEffect * movingEffect);
                }
            }
        }

        //lässt den Player vorwärts laufen
        public void walkForward()
        {
            if (movingEffect == -1)
            {
                directionId = 2;
                if (collisionManager.canWalk(this, directionId))
                {
                    currentSpeed -= (speed * speedEffect * movingEffect);

                }
            }
            else
            {
                directionId = 0;
                if (collisionManager.canWalk(this, directionId))
                {
                    currentSpeed -= (speed * speedEffect * movingEffect);

                }
            }
        }

        //dash Effekt
        public void dash(GameTime gameTime, bool mute)
        {
            currentSpeed *= 5f;
            currentDashPower = dashPower;
            dashing = true;
            dashTime = gameTime.TotalGameTime;
            if (!mute)
            {
                dashEffect.Play();
            }
        }

        //überprüft ob der Player dashen kann
        public void checkCanDash(GameTime gameTime)
        {
            if (dashTime + TimeSpan.FromMilliseconds(dashCountdown) <= gameTime.TotalGameTime)
            {
                dashing = false;
            }
        }

        //gibt die Zeit zum nächsten möglichen Dash zurück
        public TimeSpan getRestDashTime(GameTime gameTime)
        {
            return (dashTime + TimeSpan.FromMilliseconds(dashCountdown)-gameTime.TotalGameTime);
        }

        //gibt die Winkel der CollisionSpheres in der Ecke zurück
        public float[] getCornerAngles()
        {
            return cornerAngles;
        }
        
        //gibt die Richtungs ID zurück
        public int getDirectionId()
        {
            return directionId;
        }

        //gibt den aktuellen Speed zurück
        public float getCurrentSpeed()
        {
            return currentSpeed;
        }

        //gibt zurück ob der Spieler dasht
        public bool isDashing()
        {
            return dashing;
        }

        //gibt die Power zurück
        public float getPower()
        {
            return power;
        }

        //gibt die Model Id zurück
        public int getModelId()
        {
            return modelId;
        }

        // gibt die y Rotation zurück
        public float getRotationY()
        {
            return rotationy;
        }

        //gibt die position zurück
        public Vector3 getPosition()
        {
            return position;
        }

        //gibt die aktuelle DashPower zurück
        public float getcurrentDashPower()
        {
            return currentDashPower;
        }
    }
}
