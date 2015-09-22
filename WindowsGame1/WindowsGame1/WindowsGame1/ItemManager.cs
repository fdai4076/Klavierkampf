using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private TimeSpan activationTime;
        private TimeSpan effectTime;

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
            position.X = r.Next(-13, 13) + 0.5f;
            position.Y = 3;
            position.Z = r.Next(-13, 13) + 0.5f;
            itemIndex = r.Next(0, 4);
            Vector3 spherePosition = new Vector3(position.X, 1.2f, position.Z);
            collisionSphere = new CollisionSphere(spherePosition, itemIndex);

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

        public void update(GameTime gameTime)
        {
            rotationy += 0.025f;
            if (!pickedUp)
            {
                pickerIndex = collisionManager.itemControll();

                if (pickerIndex != 4)
                {
                    activationTime = gameTime.TotalGameTime;
                    pickedUp = true;
                }
            }

            if (pickedUp && (activationTime + effectTime) >= gameTime.TotalGameTime)
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
    }
}

