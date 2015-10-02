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
        public List<Collision> playerCollisions = new List<Collision>();
        public  Vector3 test;
        private SoundEffect crashEffect;
        private bool mute;
        private float step = 0.1f;
       


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

        public bool canWalk(Player player, int direction)
        {
            List<int>[] enemyList = new List<int>[] {
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>(),
            };

            

            CollisionSphere[] playerSpheres = player.getCollisionSpheres();

            foreach (Player actualPlayer in playerList)
            {
                if (actualPlayer.getPlayerIndex() != player.getPlayerIndex())
                {
                    CollisionSphere[] enemySpheres = actualPlayer.getCollisionSpheres();
                    foreach (CollisionSphere playerSphere in playerSpheres)
                    {
                        foreach (CollisionSphere enemySphere in enemySpheres)
                        {
                            if (playerSphere.getSphere().Intersects(enemySphere.getSphere()))
                            {
                                if (player.getPower() > actualPlayer.getMass())
                                {
                                    enemyList[actualPlayer.getPlayerIndex()].Add(playerSphere.getDirectionIndex());
                                }
                            }
                        }
                    }
                }
            }

            foreach (List<int> list in enemyList)
            {
                if (list.Contains(direction) && !list.Contains(1) && !list.Contains(3))
                {
                    return true;
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

        public void calculateCollisions(Player player, int directionId, float speed, float power, float dashPower, float rotationY)
        {
            int rounds = (int) (Math.Abs(speed) / step);
            Vector3 directionVector = new Vector3(0,0,0);
            for (int i = 0; i < rounds; i++)
            {

                CollisionSphere[] playerCollisionSpheres = player.getCollisionSpheres();
                int playerIndex = player.getPlayerIndex();

                List<Player> playerHit = new List<Player>();
                int directionIdOfEnemy = 4;
                getAllPlayerAt(playerCollisionSpheres, playerIndex, directionId, playerHit, directionIdOfEnemy);
                float enemyMass = getAllMass(playerHit);
                //float newSpeed = getNewSpeed(speed, power, dashPower, enemyMass);
                int dashFaktor = 1;
                if (dashPower > 0)
                {
                    dashFaktor = (int)(dashPower / step)* 2;
                }
                Vector3 actualVector = getDirectionVector(speed, power, dashPower, rotationY, enemyMass);
                if (playerHit.Count > 0)
                {

                    calculateCollisions(playerHit[playerHit.Count - 1], directionIdOfEnemy, speed*dashFaktor, power, dashPower, rotationY);
                }
                directionVector += actualVector;
                player.moveCollisionSpheres(actualVector);
            }
            test = directionVector;
            player.movePlayer(directionVector);
        }

        private float getNewSpeed(float speed, float power, float dashPower, float enemyMass)
        {
            return (speed * ((power + dashPower - enemyMass) / (power + dashPower)));
        }


        private void getAllPlayerAt(CollisionSphere[] playerCollisionSpheres, int playerIndex, int directionId, List<Player> playerHit, int lastIndexOfEnemy)
        {
            foreach (Player player in playerList)
            {
                if (player.getPlayerIndex() != playerIndex)
                {
                    CollisionSphere[] enemyCollisionSpheres = player.getCollisionSpheres();

                    foreach (CollisionSphere playerSphere in playerCollisionSpheres)
                    {
                        if (playerSphere.getDirectionIndex() == directionId)
                        {
                            foreach (CollisionSphere enemySphere in enemyCollisionSpheres)
                            {
                                if (playerSphere.getSphere().Intersects(enemySphere.getSphere()))
                                {
                                    if (!playerHit.Contains(player))
                                    {
                                        playerHit.Add(player);
                                        lastIndexOfEnemy = ((getDirectionId(playerCollisionSpheres, enemyCollisionSpheres) + 2) % 4);
                                        getAllPlayerAt(player.getCollisionSpheres(), player.getPlayerIndex(), lastIndexOfEnemy, playerHit, lastIndexOfEnemy);
                                    }

                                }

                            }
                        }
                    }
                }
            }

        }

        private int getDirectionId(CollisionSphere[] playerSphere, CollisionSphere[] enemySphere)
        {
            List<int> enemySpheresId = new List<int>();
            foreach (CollisionSphere currentPlayerSphere in playerSphere)
            {
                foreach (CollisionSphere currentEnemySphere in enemySphere)
                {
                    if (currentPlayerSphere.getSphere().Intersects(currentEnemySphere.getSphere()))
                    {
                        enemySpheresId.Add(currentEnemySphere.getDirectionIndex());
                    }

                }
            }
            if (enemySpheresId.Contains(1))
            {
                return 1;
            }
            if (enemySpheresId.Contains(3))
            {
                return 3;
            }
            if (enemySpheresId.Contains(0))
            {
                return 0;
            }
            else
            {
                return 2;
            }

        }


        private float getAllMass(List<Player> playerHit)
        {
            float allMass = 0f;
            foreach (Player enemy in playerHit)
            {
                allMass += enemy.getMass();
            }
            return allMass;
        }

        private Vector3 getDirectionVector(float speed, float power, float dashPower, float rotationY, float enemyMass)
        {
            Vector3 directionVector = new Vector3(0, 0, 0);
            if (power + dashPower > enemyMass)
            {
                directionVector.X = (float)(speed * ((power + dashPower - enemyMass) / (power + dashPower)) * Math.Sin(rotationY));
                directionVector.Z = (float)(speed * ((power + dashPower - enemyMass) / (power + dashPower)) * Math.Cos(rotationY));
            }
            return directionVector;
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

