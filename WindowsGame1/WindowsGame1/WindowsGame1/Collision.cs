using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    public class Collision
    {
        float enemyRotation;
        float power;

        public Collision( float enemyRotation, float power)
        {
            
            this.enemyRotation = enemyRotation;
            this.power = power;
        }


        public float getPower()
        {
            return power;
        }

        public float getEnemyDirection()
        {
            return enemyRotation;
        }
    }
}
