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
        int enemyDirectionId;
        float enemySpeed;


        public Collision(float enemySpeed, float enemyRotation, float enemypower, float enemyMass, int enemyDirectionId)
        {

            this.enemyRotation = enemyRotation;
            this.enemypower = enemypower;
            this.enemyMass = enemyMass;
            this.enemyDirectionId = enemyDirectionId;
            this.enemySpeed = enemySpeed;

        }


        public float getEnemyPower()
        {
            return enemypower;
        }

        public int getEnemyDirection()
        {
            return enemyDirectionId;
        }


        public float getEnemyMass()
        {
            return enemyMass;
        }

        public float getEnemyRotation()
        {
            return enemyRotation;
        }

        public float getEnemySpeed()
        {
            return enemySpeed;
        }
    }

}
