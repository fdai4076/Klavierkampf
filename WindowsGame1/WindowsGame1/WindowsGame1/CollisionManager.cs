using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace WindowsGame1
{

    public class CollisionManager
    {
        private List<Player> playerList;
        private BoundingBox arenaBounding;
        private BoundingBox groundBounding;
        
        public List<Collision>[] collisions = new List<Collision>[4];
        public List<Player> enemysInFront = new List<Player>();
        public float test;
        private SoundEffect crashEffect;
        private bool mute;
       


        public CollisionManager(BoundingBox arenaBounding, BoundingBox groundBounding,SoundEffect crashEffect)
        {
            playerList = new List<Player>();
            this.arenaBounding = arenaBounding;
            this.groundBounding = groundBounding;
            this.crashEffect = crashEffect;
            
            collisions[0] = new List<Collision>();
            collisions[1] = new List<Collision>();
            collisions[2] = new List<Collision>();
            collisions[3] = new List<Collision>();

        }

        public void setPlayers(List<Player> playerlist)
        {
            playerList.Clear();
            for (int i = 0; i < playerlist.Count; i++)
            {
                playerList.Add(playerlist[i]);
            }

        }

        private void clearCollisions()
        {
            for (int i = 0; i < collisions.Length; i++)
            {
                collisions[i].Clear();
            }
        }

        public bool checkCanRotateRight(Player player, Vector3 modelPos)
        {
            CollisionSphere[] spheres = player.getCollisionSpheres();
            for (int i = 0; i < playerList.Count; i++)
            {
                if (!(player.getPlayerIndex() == playerList[i].getPlayerIndex()))
                {
                    CollisionSphere[] enemySpheres = playerList[i].getCollisionSpheres();
                    for (int x = 0; x < spheres.Length; x++)
                    {
                        for (int y = 0; y < enemySpheres.Length; y++)
                        {
                            if (spheres[x].getSphere().Intersects(enemySpheres[y].getSphere()) &&
                            ((spheres[x].getAngleToModel() > MathHelper.ToRadians(90) && spheres[x].getAngleToModel() <= MathHelper.ToRadians(player.getCornerAngles()[0])) ||
                            (spheres[x].getAngleToModel() > MathHelper.ToRadians(270) && spheres[x].getAngleToModel() <= MathHelper.ToRadians(player.getCornerAngles()[3]))))
                            {
                                return false;

                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool checkCanRotateLeft(Player player, Vector3 modelPos)
        {
            CollisionSphere[] spheres = player.getCollisionSpheres();
            for (int i = 0; i < playerList.Count; i++)
            {
                if (!(player.getPlayerIndex() == playerList[i].getPlayerIndex()))
                {
                    CollisionSphere[] enemySpheres = playerList[i].getCollisionSpheres();
                    for (int x = 0; x < spheres.Length; x++)
                    {
                        for (int y = 0; y < enemySpheres.Length; y++)
                        {
                            if (spheres[x].getSphere().Intersects(enemySpheres[y].getSphere()) &&
                            ((spheres[x].getAngleToModel() > MathHelper.ToRadians(player.getCornerAngles()[1]) && spheres[x].getAngleToModel() <= MathHelper.ToRadians(90)) ||
                            (spheres[x].getAngleToModel() > MathHelper.ToRadians(player.getCornerAngles()[2]) && spheres[x].getAngleToModel() <= MathHelper.ToRadians(270))))
                            {
                                return false;

                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool canWalkForward(Player player)
        {
            CollisionSphere[] spheres = player.getCollisionSpheres();
            for (int i = 0; i < playerList.Count; i++)
            {
                if (!(player.getPlayerIndex() == playerList[i].getPlayerIndex()))
                {
                    CollisionSphere[] enemySpheres = playerList[i].getCollisionSpheres();
                    for (int x = 0; x < spheres.Length; x++)
                    {
                        for (int y = 0; y < enemySpheres.Length; y++)
                        {
                            if ((spheres[x].getSphere().Intersects(enemySpheres[y].getSphere())) && spheres[x].getDirectionIndex() == 0)
                            {
                                if (playerList[i].getMass() > player.getPower())
                                    return false;

                                if ((playerList[i].getDirectionId() != 4 && playerList[i].getDirectionId() == enemySpheres[y].getDirectionIndex()) && playerList[i].getPower() == player.getPower() && playerList[i].getMass() == player.getMass())
                                    return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool canWalkBackward(Player player)
        {
            CollisionSphere[] spheres = player.getCollisionSpheres();
            for (int i = 0; i < playerList.Count; i++)
            {
                if (!(player.getPlayerIndex() == playerList[i].getPlayerIndex()))
                {
                    CollisionSphere[] enemySpheres = playerList[i].getCollisionSpheres();
                    for (int x = 0; x < spheres.Length; x++)
                    {
                        for (int y = 0; y < enemySpheres.Length; y++)
                        {
                            if ((spheres[x].getSphere().Intersects(enemySpheres[y].getSphere())) && spheres[x].getDirectionIndex() == 2)
                            {
                                if (playerList[i].getMass() > player.getPower())
                                    return false;

                                if ((playerList[i].getDirectionId() != 4 && playerList[i].getDirectionId() == enemySpheres[y].getDirectionIndex()) && playerList[i].getPower() == player.getPower() && playerList[i].getMass() == player.getMass())
                                    return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool canFall(Player player)
        {
            CollisionSphere[] spheres = player.getCollisionSpheres();
            foreach (CollisionSphere currentSphere in spheres)
            {
                if (currentSphere.getSphere().Intersects(arenaBounding))
                {
                    return false;
                }

            }
            return true;
        }

        public bool outOfGame(Player player)
        {
            CollisionSphere[] spheres = player.getCollisionSpheres();
            foreach (CollisionSphere currentSphere in spheres)
            {
                if (currentSphere.getSphere().Intersects(groundBounding))
                {
                    for (int i = 0; i < playerList.Count; i++)
                    {
                        if (playerList[i].getPlayerIndex() == player.getPlayerIndex())
                        {
                            playerList.RemoveAt(i);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public int checkPlayerAlive()
        {
            return playerList.Count;
        }

        public int winner()
        {
            return playerList[0].getModelId();
        }

        public int winnerIndex()
        {
            return playerList[0].getPlayerIndex();
        }


        public Vector3 calculateCollisions(Player player)
        {


            enemysInFront.Clear();
            
            checkPlayerCollisionAt(player, player.getDirectionId());
            float allEnemyMass = getAllEnemyMass(enemysInFront);
             Vector3 directionVector = calculateVector(player, allEnemyMass);
            hitAllEnemys(enemysInFront, directionVector, player.getDirectionId(), player.getcurrentDashPower(), player.getCurrentSpeed());

             Vector3 enemyCollisionVectors = getEnemyCollisionVectors(player.getPlayerIndex());


             directionVector = player.getCurrentSpeed() * directionVector + enemyCollisionVectors;
            return  directionVector;
        }

        private float getAllEnemyMass(List<Player> enemysInFront)
        {
            float allEnemyMasses = 0f;
            foreach (Player enemy in enemysInFront)
            {
                
                allEnemyMasses += enemy.getMass();
            }
            return allEnemyMasses;
        }

       
            


        private void checkPlayerCollisionAt(Player player, int playerDirectionId)
        {

          
            CollisionSphere[] playerCollisionSpheres = player.getCollisionSpheres();
            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i].getPlayerIndex() != player.getPlayerIndex())
                {
                    CollisionSphere[] enemyCollisionSpheres = playerList[i].getCollisionSpheres();

                    for (int x = 0; x < playerCollisionSpheres.Length; x++)
                    {
                        if (playerCollisionSpheres[x].getDirectionIndex() == playerDirectionId)
                        {
                            for (int y = 0; y < enemyCollisionSpheres.Length; y++ )
                            {
                                if (playerCollisionSpheres[x].getSphere().Intersects(enemyCollisionSpheres[y].getSphere()))
                                {
                                    if (!enemysInFront.Contains(playerList[i]))
                                    {
                                        checkPlayerCollisionAt(playerList[i], (enemyCollisionSpheres[y].getDirectionIndex() + 2) % 4);
                                        enemysInFront.Add(playerList[i]);
                                        if (!mute)
                                        {
                                            crashEffect.Play();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
           
        }

        private Vector3 getEnemyCollisionVectors(int playerIndex)
        {
            Vector3 enemyDirections = new Vector3(0, 0, 0);
            
            for(int i = 0; i < collisions[playerIndex].Count; i++)
            {
                Collision currentCollision = collisions[playerIndex][i];
                
                enemyDirections.X = (float)(currentCollision.getSpeed() * currentCollision.getDirectionVector().X); 
                enemyDirections.Z = (float)(currentCollision.getSpeed() * currentCollision.getDirectionVector().Z);
                collisions[playerIndex].Remove(currentCollision);

                test = currentCollision.getDashPower() / 0.1f;
                if ((currentCollision.getDashPower() / 0.01f) > 0)
                {
                    enemyDirections.X *= (currentCollision.getDashPower()/0.01f);
                    enemyDirections.Z *= (currentCollision.getDashPower() / 0.01f);
                }
            }
            
            return enemyDirections;
        }

        private Vector3 calculateVector(Player player, float allEnemyMass)
        {
            float playerSpeed = player.getCurrentSpeed();
            float playerPower = player.getPower();
            float playerDashPower = player.getcurrentDashPower();
            float playerRotationY = player.getRotationY();

            Vector3 vector = new Vector3(0, 0, 0);
            if ((playerPower + playerDashPower) > allEnemyMass)
            {
                vector.X = (float)(((playerPower + playerDashPower - allEnemyMass) / (playerPower + playerDashPower)) * Math.Sin(playerRotationY));
                vector.Z = (float)(((playerPower + playerDashPower - allEnemyMass) / (playerPower + playerDashPower)) * Math.Cos(playerRotationY));
            } 
            return vector;
        }

        private void hitAllEnemys(List<Player> enemyList, Vector3 directionVector, int playerDirectionId, float playerDashPower, float playerSpeed)
        {
            foreach(Player enemy in enemyList)
            {
                collisions[enemy.getPlayerIndex()].Add(new Collision(playerDirectionId,playerSpeed, directionVector, playerDashPower));
            }
        }
                
        public int checkItemPickedUp(BoundingSphere itemSphere)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                CollisionSphere[] spheres = playerList[i].getCollisionSpheres();
                for (int k = 0; k < spheres.Length; k++)
                {
                    if (spheres[k].getSphere().Intersects(itemSphere))
                    {
                        return playerList[i].getPlayerIndex();
                    }
                }
            }
            return 4;
        }
        public void Update(bool mute)
        {
            this.mute = mute;
        }

    }
     

}

