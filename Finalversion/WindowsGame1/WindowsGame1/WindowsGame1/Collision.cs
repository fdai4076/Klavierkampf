﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    public class Collision
    {
        private Vector3 directionVector;
        private float dashPower;
        private float speed;
        private int directionId;

        public Collision(int directionId, float speed, Vector3 directionVector, float dashPower)
        {
            this.directionVector = directionVector;
            this.dashPower = dashPower;
            this.speed = speed;
            this.directionId = directionId;
        }

        //gibt den Richtungsvektor zurück
        public Vector3 getDirectionVector()
        {
            return directionVector;
        }

        //gibt die DashPower zurück
        public float getDashPower()
        {
            return dashPower;
        }

        //gibt den Speed zurück
        public float getSpeed()
        {
            return speed;
        }

        //gibt die RichtungId zurück
        public int getDirectionId()
        {
            return directionId;
        }       
    }
}