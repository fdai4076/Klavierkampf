﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{

    public class Item
    {

  

        private Model itemModel;
        private Vector3 position;

        private float rotationy;
        private int itemIndex;
        private TimeSpan activeTime; 

        private CollisionSphere collisonSphere;

        public Item(Model itemModel, Vector3 position, int itemIndex, TimeSpan activeTime)
        {
            this.itemModel = itemModel;
            this.position = position;
            this.itemIndex = itemIndex;
            this.activeTime = activeTime;

            collisonSphere = new CollisionSphere(position,itemIndex);

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
