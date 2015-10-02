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

        private Model itemSchatten;

        public struct ItemsEffect
        {
            public float speed;
            public float power;
            public int moving;
        };

        public ItemManager(Model[] itemModel, CollisionManager collisionManager,Model itemSchatten)
        {
            r = new Random();
            this.itemModel = itemModel;
            rotationy = 0.1f;
            pickedUp = false;
            effectTime = new TimeSpan(0, 0, 10);
            this.collisionManager = collisionManager;
            this.itemSchatten = itemSchatten;
            spawnItem();
        }
        
        //erstellt Zufallswerte für die SpawnPosition des Items und erstellt die passende collisionSphere
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

        //weist dem struct itemsEffect Werte zu, die der Player später nutzt um die Bewegung zu verändern
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

        //lässt das Item rotieren, außerdem legt es eine neue Position fest, wenn das Item eingesammelt wurde,
        //damit angezeigt werden kann welches item eingesammelt wurde, wenn die effectTime des Items vorbei ist, wird ein
        // neues gespawnt
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

        //zeichnet das Item an der zufälligen festgelegten stelle und den Schatten direkt darunter
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
                if (!pickedUp)
                {
                    Matrix world2 = Matrix.Identity * Matrix.CreateTranslation(new Vector3(position.X, 1.4f, position.Z));
                    foreach (ModelMesh mesh in itemSchatten.Meshes)
                    {
                        foreach (BasicEffect basic in mesh.Effects)
                        {
                            basic.World = world2;
                            basic.View = view;
                            basic.Projection = projection;
                            basic.EnableDefaultLighting();
                        }
                        mesh.Draw();
                    }
                }
            }
        
        //gibt die Sphere des Items zurück
        public CollisionSphere getItemSphere()
        {
            return collisionSphere;
        }

        //gibt die restliche Zeit, in der das Item einen Effekt hat, zurück
        public int getRestTime(GameTime gameTime)
        {
            TimeSpan time = ((activationTime + effectTime) - gameTime.TotalGameTime);
            return time.Seconds;
        }

        // gibt den Index des Items zurück
        public int getItemIndex()
        {
            return itemIndex;
        }

        //gibt zurück ob das Item schon eingesammelt wurde
        public bool getPickedUp()
        {
            return pickedUp;
        }

        //gibt zurück welche Player einen Effekt des Items abbekommen
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

        public void resetItem()
        {
            activationTime = new TimeSpan(0,0,0);
        }
    }
}

