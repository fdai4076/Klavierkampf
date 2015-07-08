using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

public class Character
{

    private Model mesh;
    //private Audio sound;
    //private Boundingbox; 
    private int geschwindigkeit;
    private float gravity;


    public Character(Model mesh, int geschwindigkeit, float gravity)
	{
        this.mesh = mesh;
        this.geschwindigkeit = geschwindigkeit;
        this.gravity = gravity;
	}

    public Model getMesh()
    {
        return mesh;
    }

    public int getGeschwindigkeit()
    {
        return geschwindigkeit;
    }

    public float getGravity()
    {
        return gravity;
    }

}
