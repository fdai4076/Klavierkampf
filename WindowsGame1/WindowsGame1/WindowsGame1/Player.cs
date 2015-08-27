using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class Player
{
    public Model model;
    private Model sphereModel;
    private Texture2D textur;
    private Vector3 position;
    private Vector3 aktposition;
    private bool[] itemActive;
    private int playerindex;
    public float rotationy;
    public double faktorz;
    public double faktory;
    public float speed;
    //private BoundingSphere bound;
    private BoundingSphere[] sphere;

    public Player(Vector3 spawn, int playerindex, Model model, BoundingSphere[] sphere, Model sphereModel, Texture2D textur)
    {
        this.position = spawn;
        this.playerindex = playerindex;
        this.model = model;
        this.textur = textur;
        this.sphereModel = sphereModel;

        this.sphere = sphere;

            for (int i = 0; i < this.sphere.Length; i++)
            {
                this.sphere[i].Center = this.sphere[i].Center + spawn;
            }
        
        //bound = model.Meshes[0].BoundingSphere;
        //bound.Radius = 0.5f;
        rotationy = 0.0f;
        faktory = 0.0;
        faktorz = 1.0;
        speed = 0.1f;
        
    }

    public void Draw(Matrix view, Matrix projection)
    {
        for (int i = 0; i < sphere.Length; i++)
        {
            Matrix World = Matrix.Identity * Matrix.CreateTranslation(sphere[i].Center);
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

        Matrix world = Matrix.Identity * Matrix.CreateRotationY(rotationy) *Matrix.CreateTranslation(position);

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
                //faktorz -= 0.01f;
               // faktory += 0.01f;
                for (int i = 0; i < sphere.Length; i++)
                {
                    float radius = (Math.Abs(sphere[i].Center.X - position.X) + Math.Abs(sphere[i].Center.Z - position.Z));
                    sphere[i].Center.X = (float)(position.X + (Math.Sin(rotationy) * radius));
                    sphere[i].Center.Z = (float)(position.Z + (Math.Cos(rotationy) * radius));
                }
                


            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                rotationy -= 0.01f;
               // faktorz += 0.01f;
               // faktory -= 0.01f;
                for (int i = 0; i < sphere.Length; i++)
                {
                    float radius = (Math.Abs(sphere[i].Center.X - position.X) + Math.Abs(sphere[i].Center.Z - position.Z));
                    sphere[i].Center.X = (float)(position.X - (Math.Sin(rotationy) * radius));
                    sphere[i].Center.Z = (float)(position.Z - (Math.Cos(rotationy) * radius));
                }
                
            }
      
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if(canWalk)
                {
                   // position.Z -= (float)Math.Sin((float)faktorz)/100;
                   // position.X -= (float)Math.Sin((float)faktory)/100;
                    position.Z -= (float)(speed * Math.Cos(rotationy));
                    position.X -= (float)(speed * Math.Sin(rotationy));
                    for (int i = 0; i < sphere.Length; i++)
                    {
                        //sphere[i].Center = sphere[i].Center - new Vector3((float)Math.Sin((float)faktory)/100, 0, (float)Math.Sin((float)faktorz)/100);
                            sphere[i].Center.X -= (float)(speed * Math.Sin(rotationy));
                            sphere[i].Center.Z -= (float)(speed * Math.Cos(rotationy));
                        
                    }
                }

            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (canWalk)
                {
                    //position.Z += (float)Math.Sin((float)faktorz)/100;
                   // position.X += (float)Math.Sin((float)faktory)/100;
                    position.Z += (float)(speed * Math.Cos(rotationy));
                    position.X += (float)(speed * Math.Sin(rotationy));
                    for (int i = 0; i < sphere.Length; i++)
                    {
                       // sphere[i].Center = sphere[i].Center + new Vector3((float)Math.Sin((float)faktory)/100, 0, (float)Math.Sin((float)faktorz)/100);
                        sphere[i].Center.X += (float)(speed * Math.Sin(rotationy));
                        sphere[i].Center.Z += (float)(speed * Math.Cos(rotationy));
                    }
                }
            }
        }

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
                    Console.WriteLine(radius);
                    sphere[i].Center.X = (float)(position.X + (Math.Cos(rotationy)* radius));
                    sphere[i].Center.Z = (float)(position.Z + (Math.Sin(rotationy) * radius));
                }
                
            }
        }

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

    public BoundingSphere[] getBound()
    {
        return sphere;
    }

    /*public BoundingSphere getBoundingSphere()
    {
        return bound;
    }
     */
}
