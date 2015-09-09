using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    
    public class CollisionManager
    {
        private Player[] players;
        private List<Collision>[] collisions = new List<Collision>[4];
       
        public CollisionManager()
        {
            players = null;
            collisions[0] = new List<Collision>();
            collisions[1] = new List<Collision>();
            collisions[2] = new List<Collision>();
            collisions[3] = new List<Collision>();

        }

        public List<Collision>[] checkCollision(Player player)
        {
            clearCollisions();
            CollisionSphere[] playerSpheres = player.getCollisionSpheres();
            for (int i = 0; i < players.Length; i++)
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
                                collisions[playerSpheres[x].getDirectionIndex()].Add(new Collision(players[i].rotationy, players[i].power, players[i].getMass(), players[i].getPlayerIndex()));
                            }
                        }
                    }
                }
            }
            return collisions;
        }

        public void setPlayers(Player[] players)
        {
            this.players = players;
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
            for (int i = 0; i < players.Length; i++)
            {
                if (!(player.getPlayerIndex() == players[i].getPlayerIndex()))
                {
                    CollisionSphere[] enemySpheres = players[i].getCollisionSpheres();
                    for (int x = 0; x < spheres.Length; x++)
                    {
                        for (int y = 0; y < enemySpheres.Length; y++)
                        {
                            if (spheres[x].getSphere().Intersects(enemySpheres[y].getSphere()) &&
                            ((spheres[x].getAngleToModel() > MathHelper.ToRadians(90) && spheres[x].getAngleToModel() <= MathHelper.ToRadians(160)) ||
                            (spheres[x].getAngleToModel() > MathHelper.ToRadians(270) && spheres[x].getAngleToModel() <= MathHelper.ToRadians(340))))
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
            for (int i = 0; i < players.Length; i++)
            {
                if (!(player.getPlayerIndex() == players[i].getPlayerIndex()))
                {
                    CollisionSphere[] enemySpheres = players[i].getCollisionSpheres();
                    for (int x = 0; x < spheres.Length; x++)
                    {
                        for (int y = 0; y < enemySpheres.Length; y++)
                        {
                            if (spheres[x].getSphere().Intersects(enemySpheres[y].getSphere()) &&
                            ((spheres[x].getAngleToModel() > MathHelper.ToRadians(20) && spheres[x].getAngleToModel() <= MathHelper.ToRadians(90)) ||
                            (spheres[x].getAngleToModel() > MathHelper.ToRadians(200) && spheres[x].getAngleToModel() <= MathHelper.ToRadians(270))))
                            {
                                return false;

                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool canWalkForward (Player player)
        {
            CollisionSphere[] spheres = player.getCollisionSpheres();
            for (int i = 0; i< players.Length; i++)
            {
                if(!(player.getPlayerIndex() == players[i].getPlayerIndex()))
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
            for (int i = 0; i < players.Length; i++)
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




                    



    }
}
