using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{
    public class CollisionSphere
    {
        private BoundingSphere sphere;
        private Vector3 posToModel;
        private double angleToModel;
        private double radius;
        private int directionIndex;

        public CollisionSphere (Vector3 pos, int directionIndex)
        {
            
            this.posToModel = pos;
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
