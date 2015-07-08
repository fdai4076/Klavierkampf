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
    private float rotationy;

    public Player(Vector3 spawn, int playerindex, Model model, Texture2D textur)
    {
        this.position = spawn;
        this.playerindex = playerindex;
        this.model = model;
        this.textur = textur;
        rotationy = 0.0f;
    }

    public void Draw(Matrix view, Matrix projection)
    {
        Matrix world = Matrix.Identity * Matrix.CreateTranslation(position) * Matrix.CreateRotationY(MathHelper.ToRadians(rotationy)) * Matrix.CreateTranslation(-position);
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
                rotationy += 0.1f;
                //position.X += 
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                position.X += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                position.Z -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                position.Z += 1;
            }
        }

        if (playerindex == 1)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                position.X -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                position.Z -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                position.Z += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                position.X += 1;
            }
        }
    }
}
