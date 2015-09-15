using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{

    public class CollisionManager
    {
        private List<Player> players;
        private BoundingBox arenaBounding;
        private BoundingBox groundBounding;
        private List<Collision>[] collisions = new List<Collision>[4];

        public CollisionManager(BoundingBox arenaBounding, BoundingBox groundBounding)
        {
            players = new List<Player>();
            this.arenaBounding = arenaBounding;
            this.groundBounding = groundBounding;
            collisions[0] = new List<Collision>();
            collisions[1] = new List<Collision>();
            collisions[2] = new List<Collision>();
            collisions[3] = new List<Collision>();

        }

        public List<Collision>[] checkCollision(Player player)
        {
            clearCollisions();
            CollisionSphere[] playerSpheres = player.getCollisionSpheres();
            for (int i = 0; i < players.Count; i++)
            {
                if (!(players[i].getPlayerIndex() == player.getPlayerIndex()))
                {
                    CollisionSphere[] enemySpheres = players[i].getCollisionSpheres();

                    for (int x = 0; x < playerSpheres.Length; x++)
                    {
                        for (int y = 0; y < enemySpheres.Length; y++)
                        {
                            if (playerSpheres[x].getSphere().Intersects(enemySpheres[y].getSphere()))
                            {
                                float enemyPower = 0;
                                if (players[i].getDirectionId() == 2 || players[i].getDirectionId() == 0)
                                {
                                    if (players[i].isDashing())
                                    {

                                        if (players[i].getCurrentSpeed() > 0)
                                        {
                                            enemyPower = players[i].power - player.getMass();
                                        }
                                        else
                                        {
                                            enemyPower = (players[i].power - player.getMass()) * (-1);
                                        }

                                    }
                                    else
                                    {
                                        if (players[i].power > player.getMass())
                                            enemyPower = players[i].getCurrentSpeed();
                                    }
                                }




                                collisions[playerSpheres[x].getDirectionIndex()].Add(new Collision(players[i].rotationy, enemyPower, players[i].getMass(), players[i].getPlayerIndex()));
                            }
                        }
                    }
                }
            }
            return collisions;
        }

        public void setPlayers(Player[] playerarray)
        {
            for (int i = 0; i < playerarray.Length; i++)
            {
                this.players.Add(playerarray[i]);
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
            for (int i = 0; i < players.Count; i++)
            {
                if (!(player.getPlayerIndex() == players[i].getPlayerIndex()))
                {
                    CollisionSphere[] enemySpheres = players[i].getCollisionSpheres();
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
            for (int i = 0; i < players.Count; i++)
            {
                if (!(player.getPlayerIndex() == players[i].getPlayerIndex()))
                {
                    CollisionSphere[] enemySpheres = players[i].getCollisionSpheres();
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
            for (int i = 0; i < players.Count; i++)
            {
                if (!(player.getPlayerIndex() == players[i].getPlayerIndex()))
                {
                    CollisionSphere[] enemySpheres = players[i].getCollisionSpheres();
                    for (int x = 0; x < spheres.Length; x++)
                    {
                        for (int y = 0; y < enemySpheres.Length; y++)
                        {
                            if ((spheres[x].getSphere().Intersects(enemySpheres[y].getSphere())) && spheres[x].getDirectionIndex() == 0)
                            {
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
            for (int i = 0; i < players.Count; i++)
            {
                if (!(player.getPlayerIndex() == players[i].getPlayerIndex()))
                {
                    CollisionSphere[] enemySpheres = players[i].getCollisionSpheres();
                    for (int x = 0; x < spheres.Length; x++)
                    {
                        for (int y = 0; y < enemySpheres.Length; y++)
                        {
                            if ((spheres[x].getSphere().Intersects(enemySpheres[y].getSphere())) && spheres[x].getDirectionIndex() == 2)
                            {
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
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (players[i].getPlayerIndex() == player.getPlayerIndex())
                        {
                            players.RemoveAt(i);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public int checkPlayerAlive()
        {
            return players.Count;
        }

        public int winner()
        {
            return players[0].modelId;
        }
    }
}
