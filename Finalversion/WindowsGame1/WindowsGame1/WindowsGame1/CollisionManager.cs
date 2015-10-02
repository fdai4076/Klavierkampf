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
        private List<Player> playerList;  // Liste aller teilnehmenden Spieler
        private BoundingBox arenaBounding; // Kollisionsbox der Arena
        private BoundingBox groundBounding; // Kollisionsbox des Bodens
        private SoundEffectInstance soundEffectInstance;

        // public List<Collision>[] collisions = new List<Collision>[4];
        // public List<Collision> playerCollisions = new List<Collision>();
        // public  Vector3 test;
        private SoundEffect crashEffect; // Soundeffect für Kollisionen
        private bool mute; // bool ob der Ton gemutet ist
        private float step = 0.01f; // Berechnungsschritt für die Kollision



        public CollisionManager(BoundingBox arenaBounding, BoundingBox groundBounding, SoundEffect crashEffect)
        {
            playerList = new List<Player>();
            this.arenaBounding = arenaBounding;
            this.groundBounding = groundBounding;
            this.crashEffect = crashEffect;
            soundEffectInstance = crashEffect.CreateInstance();
            soundEffectInstance.Volume = 1f;

        }
        // fügt Spieler in die Playerlist ein
        public void setPlayers(List<Player> playerlist)
        {
       
            playerList = playerlist;
        }

       
        // überprüft ob der Charakter (player) sich an der angegebenen Position (modelPos) nach Rechts drehen kann.
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
        // überprüft ob der Charakter (player) an der angegebenen Position (modelPos) sich nach Links drehen kann
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
        // überprüft ob der Charakter (player) in die angegebene Richtung (direction) laufen kann
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
                                if (actualPlayer.getMass() >= (player.getPower() + player.getcurrentDashPower()))
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
                if (list.Contains(direction))
                {
                    return false;
                }
            }

            return true;
        }











      

        // überprüft ob der Charakter (player) nach unten fallen kann.
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
        // gibt zurück wieviele Spieler noch am Leben sind.
        public int checkPlayerAlive()
        {
            return playerList.Count;
        }
        // gibt den Gewinner zurück.
        public int winner()
        {
            return playerList[0].getModelId();
        }
        // Gibt den PlayerIndex des Gewinners zurück.
        public int winnerIndex()
        {
            return playerList[0].getPlayerIndex();
        }
        // Berechnet die Kollisionen mit anderen Spielern und gibt einen Richtungsvektor zur Bewegung zurück.
        public void calculateCollisions(Player player, int directionId, float speed, float power, float dashPower, float rotationY)
        {

            int rounds = (int)(Math.Abs(speed) / step);

            Vector3 directionVector = new Vector3(0, 0, 0);
            speed = speed / Math.Abs(speed / step);
            for (int i = 0; i < rounds; i++)
            {

                CollisionSphere[] playerCollisionSpheres = player.getCollisionSpheres();
                int playerIndex = player.getPlayerIndex();

                List<Player> playerHit = new List<Player>();
                int directionIdOfEnemy = 4;
                getAllPlayerAt(playerCollisionSpheres, playerIndex, directionId, playerHit, directionIdOfEnemy);
                float enemyMass = getAllMass(playerHit);

                float dashFaktor = 1;
                if (dashPower > 0)
                {
                    dashFaktor = dashPower * 5000;
                }
                Vector3 actualVector = getDirectionVector(speed, power, dashPower, rotationY, enemyMass);
                if (enemyMass >= (power + dashPower))
                {
                    playerHit.Clear();
                }
                if (playerHit.Count > 0)
                {

                    calculateCollisions(playerHit[playerHit.Count - 1], directionIdOfEnemy, speed * dashFaktor, power, dashPower, rotationY);
                }

                directionVector += actualVector;
                player.moveCollisionSpheres(actualVector);
            }

            player.movePlayer(directionVector);
        }



        // Überprüft ob Charaktere die ICH treffe noch andere Charakter hinter sich haben und gibt alle zurück.
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
                                        if (!mute)
                                        {
                                            soundEffectInstance.Play();
                                        }
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
        // Berechnet welche CollisionSpheres des Gegners (enemySphere) vom Spieler (playerSphere) getroffen wird und gibt die DirectionId der getroffenen Seite zurück.
        // 0 = vorne, 1 = links, 2 = hinten , 3 = rechts.
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

        // gibt mir die Masse aller Spieler die ich direkt oder indirekt treffe zurück.
        private float getAllMass(List<Player> playerHit)
        {
            float allMass = 0f;
            foreach (Player enemy in playerHit)
            {
                allMass += enemy.getMass();
            }
            return allMass;
        }
        // Liefert den Richtungsvektor zurück.
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












        // überprüft ob ein Item eingesammelt wurde und liefert den PlayerIndex zurück. 4 bedeutet dass das Item nicht eingesammelt wurde.
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

        // mutet oder entmutet den Sound
        public void Update(bool mute)
        {
            this.mute = mute;
        }

    }


}

