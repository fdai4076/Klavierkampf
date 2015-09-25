using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace WindowsGame1
{
    public class ItemManager
    {
        private Random r;
        private Vector3 position;
        private CollisionSphere collisionSphere;

        private int itemIndex;
        public int pickerIndex;

        private Model[] itemModel;

        private float rotationy;
        public TimeSpan activationTime;
        public TimeSpan effectTime;

        private bool pickedUp;

        private CollisionManager collisionManager;

        public struct ItemsEffect
        {
            public float speed;
            public float power;
            public int moving;
        };


        public ItemManager(Model[] itemModel, CollisionManager collisionManager)
        {
            r = new Random();
            this.itemModel = itemModel;
            rotationy = 0.1f;
            pickedUp = false;
            effectTime = new TimeSpan(0, 0, 10);
            this.collisionManager = collisionManager;
            spawnItem();
        }

        public void spawnItem()
        {
            position.X = r.Next(-12, 12);
            position.Y = 3;
            position.Z = r.Next(-12, 12);
            itemIndex = r.Next(0, 4);
            Vector3 spherePosition = new Vector3(position.X, 1.2f, position.Z);
            collisionSphere = new CollisionSphere(spherePosition, itemIndex);
            collisionSphere.setCenterPos(new Vector3(position.X, 1.2f, position.Z));
        }

        public ItemsEffect getItemEffect(int playerIndex)
        {
            if (pickedUp == true)
            {

                switch (itemIndex)
                {
                    case 0:
                        if (playerIndex == pickerIndex)
                        {
                            ItemsEffect effect;
                            effect.power = 1;
                            effect.speed = 1;
                            effect.moving = 1;

                            return effect;
                        }
                        else
                        {
                            ItemsEffect effect;
                            effect.power = 1;
                            effect.speed = 0.5f;
                            effect.moving = 1;

                            return effect;
                        }
                        break;

                    case 1:
                        if (playerIndex == pickerIndex)
                        {
                            ItemsEffect effect;
                            effect.power = 1;
                            effect.speed = 1.5f;
                            effect.moving = 1;

                            return effect;
                        }
                        else
                        {
                            ItemsEffect effect;
                            effect.power = 1;
                            effect.speed = 1;
                            effect.moving = 1;

                            return effect;
                        }
                        break;

                    case 2:
                        if (playerIndex == pickerIndex)
                        {
                            ItemsEffect effect;
                            effect.power = 1.5f;
                            effect.speed = 1;
                            effect.moving = 1;

                            return effect;
                        }
                        else
                        {
                            ItemsEffect effect;
                            effect.power = 1;
                            effect.speed = 1;
                            effect.moving = 1;

                            return effect;
                        }
                        break;

                    case 3:
                        if (playerIndex == pickerIndex)
                        {
                            ItemsEffect effect;
                            effect.power = 1;
                            effect.speed = 1;
                            effect.moving = 1;

                            return effect;
                        }
                        else
                        {
                            ItemsEffect effect;
                            effect.power = 1;
                            effect.speed = 1;
                            effect.moving = -1;

                            return effect;
                        }
                        break;

                    default:
                        ItemsEffect effect2;
                        effect2.power = 1;
                        effect2.speed = 1;
                        effect2.moving = 1;

                        return effect2;
                }
            }
            else
            {
                ItemsEffect effect;
                effect.power = 1;
                effect.speed = 1;
                effect.moving = 1;

                return effect;
            }

        }

        public void update(GameTime gameTime, SoundEffect collectItem,bool mute)
        {
            rotationy += 0.025f ;
            if (!pickedUp)
            {
                pickerIndex = collisionManager.checkItemPickedUp(collisionSphere.getSphere());

                if (pickerIndex != 4)
                {
                    activationTime = gameTime.TotalGameTime;
                    pickedUp = true;
                    position.X = 18;
                    position.Y = 10;
                    position.Z = -4;
                    if (!mute)
                    {
                        collectItem.Play();
                    }
                }
            }

            if (pickedUp && gameTime.TotalGameTime>(activationTime + effectTime))
            {
                pickedUp = false;
                spawnItem();
            }
        }

        public void draw(Matrix view, Matrix projection)
        {
            
            
                Matrix world = Matrix.Identity * Matrix.CreateRotationY(rotationy) * Matrix.CreateTranslation(position);

                foreach (ModelMesh mesh in itemModel[itemIndex].Meshes)
                {
                    foreach (BasicEffect basic in mesh.Effects)
                    {
                        basic.World = world;
                        basic.View = view;
                        basic.Projection = projection;
                        basic.EnableDefaultLighting();
                    }
                    mesh.Draw();
                }
            }
        

        public CollisionSphere getItemSphere()
        {
            return collisionSphere;
        }

        public int getRestTime(GameTime gameTime)
        {
            TimeSpan time = ((activationTime + effectTime) - gameTime.TotalGameTime);
            return time.Seconds;
        }

        public int getItemIndex()
        {
            return itemIndex;
        }

        public bool getPickedUp()
        {
            return pickedUp;
        }

        public List<String> getAffectedPlayer(List<Player> playerList)
        {
            List<String> affectedPlayer = new List<String>();
            if (itemIndex == 0 || itemIndex == 3)
            {
                foreach (Player player in playerList)
                {
                    if (player.getPlayerIndex() != pickerIndex)
                    {
                        affectedPlayer.Add((player.getPlayerIndex()+1).ToString());
                    }
                }
            }
            else
            {
                  affectedPlayer.Add((pickerIndex+1).ToString());               
            }
            return affectedPlayer;
        }
    }
}

