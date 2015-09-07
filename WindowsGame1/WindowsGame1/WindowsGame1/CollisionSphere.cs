using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{
    public class CollisionSphere
    {
        BoundingSphere sphere;
        int id;
        Vector3 posToModel;
        double angleToModel;
        double radius;
        int directionIndex;

        public CollisionSphere (Vector3 pos, int directionIndex)
        {
            
            posToModel = pos;
            sphere.Radius = 0.2f;
            this.directionIndex = directionIndex;
            
        }

        public double getRadius()
        {
            return radius;
        }

        public void setRadius(double radius)
        {
            this.radius = radius;
        }

        public double getAngleToModel()
        {
            return angleToModel;
        }

        public void setAngleToModel(double angleToModel)
        {
            this.angleToModel = angleToModel;
        }

        public int getId()
        {
            return id;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public Vector3 getCenterPos()
        {
            return sphere.Center;
        }

        public void setCenterPos(Vector3 centerPos)
        {
            sphere.Center = centerPos;
        }

        public Vector3 getPosToModel()
        {
            return posToModel;
        }

        public BoundingSphere getSphere()
        {
            return sphere;
        }

        public int getDirectionIndex()
        {
            return directionIndex;
        }

    }
}
