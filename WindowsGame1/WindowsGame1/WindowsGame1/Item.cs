using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{

    class Item
    {
        Model itemModel;
        Vector3 position;

        float rotationy;
        
        enum ItemState {schnell, langsam, umkehren, power}
        private ItemState itemstate;

        public Item(Model ueModel, Vector3 uePosition){

            itemModel = ueModel;
            position = uePosition;
            //itemstate = ueItemState;
            rotationy = 0.1f;
            
        }

        public void update()
        {
            rotationy += 0.025f;
        }

        public void draw(Matrix view, Matrix projection)
        {

            Matrix world = Matrix.Identity * Matrix.CreateRotationY(rotationy) * Matrix.CreateTranslation(position);

            foreach (ModelMesh mesh in itemModel.Meshes)
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
