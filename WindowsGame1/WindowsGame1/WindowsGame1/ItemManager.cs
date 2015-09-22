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

        private Model[] itemModel;

        private float rotationy;
        private TimeSpan activationTime;
        private TimeSpan effectTime;

        private bool pickedUp; 

        public ItemManager(Model[] itemModel)
        {
            r = new Random();
            this.itemModel = itemModel;
            rotationy = 0.1f;
            pickedUp = false;
        }

        public void spawnItem()
        {


            position.X = r.Next(-12, 12);
            position.Z = r.Next(-12, 12);
            collisionSphere = new CollisionSphere(position, itemIndex);

        }

        public void update()
        {
            rotationy += 0.025f;
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

    }
}
