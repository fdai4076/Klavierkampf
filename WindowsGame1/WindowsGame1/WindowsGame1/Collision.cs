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
        float enemypower;
        float enemyMass;
        int enemyPlayerId;

        public Collision( float enemyRotation, float enemypower, float enemyMass, int enemyPlayerId)
        {
            
            this.enemyRotation = enemyRotation;
            this.enemypower = enemypower;
            this.enemyPlayerId = enemyPlayerId;
        }


        public float getEnemyPower()
        {
            return enemypower;
        }

        public float getEnemyDirection()
        {
            return enemyRotation;
        }

        public int getEnemyPlayerId()
        {
            return enemyPlayerId;
        }

        public float getEnemyMass()
        {
            return enemyMass;
        }

        public float getEnemyRotation()
        {
            return enemyRotation;
        }
    }
}
