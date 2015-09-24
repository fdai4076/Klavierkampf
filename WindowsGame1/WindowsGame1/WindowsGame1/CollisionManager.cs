using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{

    public class CollisionManager
    {
        private List<Player> playerList;
        private BoundingBox arenaBounding;
        private BoundingBox groundBounding;
        
        public List<Collision>[] collisions = new List<Collision>[4];
        public List<Player> enemysInFront = new List<Player>();
        public int test;
       


        public CollisionManager(BoundingBox arenaBounding, BoundingBox groundBounding)
        {
            playerList = new List<Player>();
            this.arenaBounding = arenaBounding;
            this.groundBounding = groundBounding;
            
            collisions[0] = new List<Collision>();
            collisions[1] = new List<Collision>();
            collisions[2] = new List<Collision>();
            collisions[3] = new List<Collision>();

        }

        /*    public List<Collision>[] checkCollision(Player player)
            {
                clearCollisions();
                CollisionSphere[] playerSpheres = player.getCollisionSpheres();
                alreadyCollideWithEnemy.Clear();
                for (int i = 0; i < playerList.Count; i++)
                {
                    if (player.getPlayerIndex() != playerList[i].getPlayerIndex())
                    {
                        CollisionSphere[] enemySpheres = playerList[i].getCollisionSpheres();
                        for (int x = 0; x < playerSpheres.Length; x++)
                        {
                            for (int y = 0; y < enemySpheres.Length; y++)
                            {
                                if (!(alreadyCollideWithEnemy.Contains(playerList[i].getPlayerIndex())))
                                {
                                    if (playerSpheres[x].getSphere().Intersects(enemySpheres[y].getSphere()))
                                    {
                                        collisions[playerSpheres[x].getDirectionIndex()].Add(new Collision(playerList[i].getCurrentSpeed(), playerList[i].getRotationY(), playerList[i].getPower(), playerList[i].getMass(), playerList[i].getDirectionId()));
                                        alreadyCollideWithEnemy.Add(playerList[i].getPlayerIndex());
                                    }
                                }
                            }
                        }
                    }
                }
                return collisions;
            }
         */

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
                    return true;
                }

            }
            return false;
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

     /*   public Vector3 calculateCollisions(Player player)
        {
            Vector3 collisionVector = new Vector3(0, 0, 0);
            List<int> alreadyCollideWithPlayer = new List<int>();
            float enemyMasses = 0f;
            CollisionSphere[] playerCollisionSpheres = player.getCollisionSpheres();
            for (int i = 0; i < playerList.Count; i++)
            {
                if (player.getPlayerIndex() != playerList[i].getPlayerIndex())
                {
                    CollisionSphere[] enemyCollisionSpheres = playerList[i].getCollisionSpheres();
                    for (int x = 0; x < playerCollisionSpheres.Length; x++)
                    {
                        for (int y = 0; y < enemyCollisionSpheres.Length; y++)
                        {
                            if (!alreadyCollideWithPlayer.Contains(playerList[i].getPlayerIndex()))
                            {
                                if (playerCollisionSpheres[x].getSphere().Intersects(enemyCollisionSpheres[y].getSphere()))
                                {
                                    alreadyCollideWithPlayer.Add(playerList[i].getPlayerIndex());

                                    float enemyPower = playerList[i].getPower();
                                    float enemyMass = playerList[i].getMass();
                                    float enemyRotation = playerList[i].getRotationY();
                                    float enemySpeed = playerList[i].getCurrentSpeed();
                                    float enemyDashPower = playerList[i].getcurrentDashPower();
                                    float playerMass = player.getMass();


                                    if (playerCollisionSpheres[x].getDirectionIndex() == player.getDirectionId())
                                    {
                                        enemyMasses += enemyMass;
                                    }

                                    if (enemyPower > playerMass)
                                    {

                                        if (((playerCollisionSpheres[x].getDirectionIndex() + 2) % 4) == enemyCollisionSpheres[y].getDirectionIndex())
                                        {
                                            enemySpeed -= player.getCurrentSpeed();
                                        }
                                        collisionVector.X += (float)(enemySpeed * ((enemyPower + enemyDashPower - playerMass) / enemyPower) * Math.Sin(enemyRotation));
                                        collisionVector.Z += (float)(enemySpeed * ((enemyPower + enemyDashPower - playerMass) / enemyPower) * Math.Cos(enemyRotation));

                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (player.getPower() > enemyMasses)
            {
                float playerPower = player.getPower();
                float playerDashPower = player.getcurrentDashPower();
                float playerSpeed = player.getCurrentSpeed();
                float playerRotation = player.getRotationY();

                collisionVector.X += (float)(playerSpeed * ((playerPower - enemyMasses) / playerPower) * Math.Sin(playerRotation));
                collisionVector.Z += (float)(playerSpeed * ((playerPower - enemyMasses) / playerPower) * Math.Cos(playerRotation));
            }
            return collisionVector;
        } */

        public Vector3 calculateCollisions(Player player)
        {


            enemysInFront.Clear();
            checkPlayerCollisionAt(player, player.getDirectionId());
            float allEnemyMass = getAllEnemyMass(enemysInFront);
             Vector3 directionVector = calculateVector(player, allEnemyMass);
            //hitAllEnemys(enemysInFront, directionVector, player.getcurrentDashPower(), player.getCurrentSpeed());

          //  Vector3 enemyCollisionVectors = getEnemyCollisionVectors(player.getPlayerIndex());
           // directionVector += enemyCollisionVectors;


            test = (enemysInFront.Count);
            return directionVector;
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
                                        //checkPlayerCollisionAt(playerList[i], (enemyCollisionSpheres[y].getDirectionIndex() + 2) % 4);
                                        enemysInFront.Add(playerList[i]);
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
                enemyDirections += (collisions[playerIndex][i].getDirectionVector() * (collisions[playerIndex][i].getDashPower() / 0.1f));
                collisions[playerIndex].Remove(collisions[playerIndex][i]);
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
                vector.X = playerSpeed * (float)(((playerPower + playerDashPower - allEnemyMass) / (playerPower + playerDashPower)) * Math.Sin(playerRotationY));
                vector.Z = playerSpeed * (float)(((playerPower + playerDashPower - allEnemyMass) / (playerPower + playerDashPower)) * Math.Cos(playerRotationY));
            } 
            return vector;
        }

        private void hitAllEnemys(List<Player> enemyList, Vector3 directionVector, float playerDashPower, float playerSpeed)
        {
            foreach(Player enemy in enemyList)
            {
                collisions[enemy.getPlayerIndex()].Add(new Collision(playerSpeed, directionVector, playerDashPower));
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
     

    }
     

}

