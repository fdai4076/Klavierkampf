using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class Player
{
    private Model model;
    private Texture2D textur;
    private Vector3 position;
    private Vector3 aktposition;
    private bool[] itemActive;
    private int playerindex;
    public float rotationy;
    public double faktorz;
    public double faktory;

    public Player(Vector3 spawn, int playerindex, Model model, Texture2D textur)
    {
        this.position = spawn;
        this.playerindex = playerindex;
        this.model = model;
        this.textur = textur;
        rotationy = 0.0f;
        faktory = 0.0;
        faktorz = 1.0;
    }

    public void Draw(Matrix view, Matrix projection)
    {
        Matrix world = Matrix.Identity * Matrix.CreateRotationY(rotationy)*Matrix.CreateTranslation(position)  ;// * Matrix.CreateTranslation(-position);
        
        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (BasicEffect basic in mesh.Effects)
            {
                basic.World = world;
                basic.View = view;
                basic.Projection = projection;
                basic.EnableDefaultLighting();
            }
            mesh.Draw();
        }
    }

    public void Update()
    {
        if (playerindex == 0)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                rotationy += 0.01f;
                faktorz -= 0.01f;
                faktory += 0.01f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                rotationy -= 0.01f;
                faktorz += 0.01f;
                faktory -= 0.01f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                position.Z -= (float)Math.Sin((float)faktorz);
                position.X -= (float)Math.Sin((float)faktory); 
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                position.Z += (float)Math.Sin((float)faktorz);
                position.X += (float)Math.Sin((float)faktory);
            }
        }

        if (playerindex == 1)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                rotationy += 0.01f;
                faktorz -= 0.01f;
                faktory += 0.01f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                position.Z -= (float)Math.Sin((float)faktorz);
                position.X -= (float)Math.Sin((float)faktory); 
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                position.Z += (float)Math.Sin((float)faktorz);
                position.X += (float)Math.Sin((float)faktory);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                rotationy -= 0.01f;
                faktorz += 0.01f;
                faktory -= 0.01f;
            }
        }
    }

    
}
