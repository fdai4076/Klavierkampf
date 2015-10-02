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

        // gibt den Radius der CollisionSphere zurück
        public double getRadius()
        {
            return radius;
        }

        //setzt den Radius einer CollisionSphere
        public void setRadius(double radius)
        {
            this.radius = radius;
        }

        //gibt den Winkel zwischen Model und CollisionSphere zurück
        public double getAngleToModel()
        {
            return angleToModel;
        }

        //setzt den Winkel zwischen CollisionSphere und Model fest
        public void setAngleToModel(double angleToModel)
        {
            this.angleToModel = angleToModel;
        }

        //gibt das Center Position der CollisonSphere zurück
        public Vector3 getCenterPos()
        {
            return sphere.Center;
        }

        //setzt die Center Position der CollisonSphere
        public void setCenterPos(Vector3 centerPos)
        {
            sphere.Center = centerPos;
        }

        //gibt die Position der CollisionSphere zum Model zurück
        public Vector3 getPosToModel()
        {
            return posToModel;
        }
        
        //gibt die Sphere zurück
        public BoundingSphere getSphere()
        {
            return sphere;
        }

        //gibt den Richtungsindex zurück
        public int getDirectionIndex()
        {
            return directionIndex;
        }
    }
}
