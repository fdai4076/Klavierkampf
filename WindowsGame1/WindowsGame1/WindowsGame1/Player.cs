using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{

    public class Player
    {
        public Model model;
        private Model sphereModel;
        public Vector3 position;
        private bool[] itemActive;
        private int playerindex;
        public float rotationy;
        public double faktorz;
        public double faktory;
        public float speed;
        public CollisionSphere[] sphere;


        public Player(Vector3 spawn, int playerindex, Model model, CollisionSphere[] sphere, Model boundingSphere)
        {

            this.position = spawn;
            this.playerindex = playerindex;
            this.model = model;
            this.sphereModel = boundingSphere;

            this.sphere = sphere;

            for (int i = 0; i < this.sphere.Length; i++)
            {
                this.sphere[i].setCenterPos(new Vector3(spawn.X + this.sphere[i].getPosToModel().X, 1.2f, spawn.Z + this.sphere[i].getPosToModel().Z));
                this.sphere[i].setAngleToModel(getAngle2Dim(this.sphere[i].getCenterPos(), this.position));
                this.sphere[i].setRadius(Math.Sqrt(Math.Pow(position.X - sphere[i].getCenterPos().X,2) + Math.Pow(position.Z - sphere[i].getCenterPos().Z,2)));
            }
            
           
            rotationy = 0.0f;
            speed = 0.1f;

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

        public void Update(bool canWalk)
        {
            
            if (playerindex == 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    rotationy += 0.01f;
                 
                    for (int i = 0; i < sphere.Length; i++)
                    {
                        double radius = sphere[i].getRadius();
                        sphere[i].setCenterPos(new Vector3(
                            (float)(position.X + (Math.Sin(rotationy+sphere[i].getAngleToModel()) * radius)),
                            sphere[i].getCenterPos().Y,
                            (float)(position.Z + (Math.Cos(rotationy+sphere[i].getAngleToModel()) * radius))));
                    }

                    
                    
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    rotationy -= 0.01f;
                    // faktorz += 0.01f;
                    // faktory -= 0.01f;
                    for (int i = 0; i < sphere.Length; i++)
                    {
                        float radius = (Math.Abs(sphere[i].getCenterPos().X - position.X) + Math.Abs(sphere[i].getCenterPos().Z - position.Z));
                        sphere[i].setCenterPos(new Vector3(
                            (float)(position.X + Math.Sin(sphere[i].getAngleToModel() + rotationy) * radius),
                            sphere[i].getCenterPos().Y,
                            (float)(position.Z + Math.Cos(sphere[i].getAngleToModel() + rotationy) * radius)));
                    }

                }

                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    if (canWalk)
                    {
                      
                        position.Z -= (float)(speed * Math.Cos(rotationy));
                        position.X -= (float)(speed * Math.Sin(rotationy));
                        for (int i = 0; i < sphere.Length; i++)
                        {
                            
                            sphere[i].setCenterPos(new Vector3(
                            (float)(sphere[i].getCenterPos().X - speed*Math.Sin(rotationy)),
                            sphere[i].getCenterPos().Y,
                            (float)(sphere[i].getCenterPos().Z - speed*Math.Cos(rotationy))));

                        }
                    }

                }
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    if (canWalk)
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
             
            /*
                    if (playerindex == 1)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Left))
                        {
                            rotationy += 0.01f;
                            //faktorz -= 0.01f;
                            //faktory += 0.01f;
                            for (int i = 0; i < sphere.Length; i++)
                            {
                                float radius = (Math.Abs(sphere[i].Center.X - position.X) + Math.Abs(sphere[i].Center.Z - position.Z));
                                sphere[i].Center.X = (float)(position.X + (Math.Sin(rotationy) * radius));
                                sphere[i].Center.Z = (float)(position.Z + (Math.Cos(rotationy) * radius));
                            }
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Up))
                        {
                
                               // position.Z -= (float)Math.Sin((float)faktorz);
                               // position.X -= (float)Math.Sin((float)faktory);
                               position.Z -= (float)(speed * Math.Cos(rotationy));
                               position.X -= (float)(speed * Math.Sin(rotationy));
                                for (int i = 0; i < sphere.Length; i++)
                                {
                                    //sphere[i].Center = sphere[i].Center - new Vector3((float)Math.Sin((float)faktory), 0, (float)Math.Sin((float)faktorz));
                                    sphere[i].Center.X -= (float)(speed * Math.Sin(rotationy));
                                    sphere[i].Center.Z -= (float)(speed * Math.Cos(rotationy));
                                }
                
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Down))
                        {
                
                               // position.Z -= (float)Math.Sin((float)faktorz);
                               // position.X -= (float)Math.Sin((float)faktory);
                            position.Z += (float)(speed * Math.Cos(rotationy));
                            position.X += (float)(speed * Math.Sin(rotationy));
                                for (int i = 0; i < sphere.Length; i++)
                                {
                                   // sphere[i].Center = sphere[i].Center + new Vector3((float)Math.Sin((float)faktory), 0, (float)Math.Sin((float)faktorz));
                                    sphere[i].Center.X += (float)(speed * Math.Sin(rotationy));
                                    sphere[i].Center.Z += (float)(speed * Math.Cos(rotationy));
                                }
                
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Right))
                        {
                            rotationy -= 0.01f;
                           // faktorz += 0.01f;
                           // faktory -= 0.01f;
               
                            for (int i = 0; i < sphere.Length; i++)
                            {
                                double radius = Math.Sqrt(Math.Pow(position.X - sphere[i].Center.X, 2) + Math.Pow(position.Z - sphere[i].Center.Z, 2));
                                float angle = MathHelper.ToRadians(180) - rotationy;
                                sphere[i].Center.X = (float)(position.X + Math.Cos((angle)) * radius);
                                sphere[i].Center.Z = (float)(position.Z + Math.Sin((angle)) * radius);
                    
                            }
                
                        }
                    }
             * /

                    /*if (playerindex == 2)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.J))
                        {
                            rotationy += 0.01f;
                            faktorz -= 0.01f;
                            faktory += 0.01f;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.L))
                        {
                            rotationy -= 0.01f;
                            faktorz += 0.01f;
                            faktory -= 0.01f;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.I))
                        {
                            position.Z -= (float)Math.Sin((float)faktorz);
                            position.X -= (float)Math.Sin((float)faktory);
                            bound.Center = bound.Center - new Vector3((float)Math.Sin((float)faktory), 0, (float)Math.Sin((float)faktorz));

                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.K))
                        {
                            position.Z += (float)Math.Sin((float)faktorz);
                            position.X += (float)Math.Sin((float)faktory);
                            bound.Center = bound.Center + new Vector3((float)Math.Sin((float)faktory), 0, (float)Math.Sin((float)faktorz));
                        }
                    }

                    if (playerindex == 3)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                        {
                            rotationy += 0.01f;
                            faktorz -= 0.01f;
                            faktory += 0.01f;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
                        {
                            position.Z -= (float)Math.Sin((float)faktorz);
                            position.X -= (float)Math.Sin((float)faktory);
                            bound.Center = bound.Center - new Vector3((float)Math.Sin((float)faktory), 0, (float)Math.Sin((float)faktorz));
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad5))
                        {
                            position.Z += (float)Math.Sin((float)faktorz);
                            position.X += (float)Math.Sin((float)faktory);
                            bound.Center = bound.Center + new Vector3((float)Math.Sin((float)faktory), 0, (float)Math.Sin((float)faktorz));
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
                        {
                            rotationy -= 0.01f;
                            faktorz += 0.01f;
                            faktory -= 0.01f;
                        }
                    }
                     */
        }



        /*public BoundingSphere getBoundingSphere()
        {
            return bound;
        }
         */


        public double getAngle2Dim(Vector3 spherePos, Vector3 modelPos)
        {
            Vector2 sphereVector = new Vector2(spherePos.X - modelPos.X, spherePos.Z - modelPos.Z);
            double angle = (Vector2.Dot(new Vector2(1, 0), sphereVector)) / (Math.Sqrt(1) * Math.Sqrt(Math.Pow(sphereVector.X, 2) + Math.Pow(sphereVector.Y, 2)));
            if (spherePos.Z > 0)
            {
                return (MathHelper.ToRadians(360) - Math.Acos(angle));
            }
            return Math.Acos(angle);
        }

        public String getDrawingThings()
        {
            return sphere[1].getCenterPos().ToString();

            

            
            //return sphere[0].getAngleToModel().ToString();
            // return /* (position.X + Math.Cos(135) * */ (Math.Abs(sphere[0].getCenterPos().X - position.X) +  Math.Abs(sphere[0].getCenterPos().Z - position.Z)).ToString();
        }
    }
}
