using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    public class CharacterManager
    {
        private Model[] modelle;
        private CollisionSphere[] sofaBounding;
        private CollisionSphere[] stuhlBounding;
        private CollisionSphere[] klavierBounding;
        private CollisionSphere[] kleiderschrankBounding;
        private CollisionSphere[] kuehlschrankBounding;
        private int id;

        private Moebel klavier;
        private Moebel kleiderschrank;
        private Moebel sofa;
        private Moebel kuehlschrank;

        public struct Moebel
        {
            public Model model;
            public int modelId;
            public CollisionSphere[] spheres;
            public float speed;
            public float rotationSpeed;
            public float dashpower;
            public int dashCountdown;
            public float power;
            public float mass;
            public float yPosition;
            public float[] angle;

        };

        public CharacterManager(Model[] modelle)
        {

            this.modelle = modelle;

            this.stuhlBounding = new CollisionSphere[] {
                new CollisionSphere (new Vector3(- 0.2f, 0, -0.2f), 0),
                new CollisionSphere (new Vector3(0.2f, 0, -0.2f), 0),
                new CollisionSphere (new Vector3(-0.2f, 0, 0.2f),2),
                new CollisionSphere (new Vector3(0.2f, 0, 0.2f),2)
            };

            this.kuehlschrankBounding = new CollisionSphere[] {
                new CollisionSphere (new Vector3(-0.4f, 0, -0.4f),0),
                new CollisionSphere (new Vector3(0,0, -0.4f),0),
                new CollisionSphere (new Vector3(0.4f, 0, -0.4f),0),
                new CollisionSphere (new Vector3(-0.4f, 0, 0),3),
                new CollisionSphere (new Vector3(0.4f, 0,0 ),1),
                new CollisionSphere (new Vector3(-0.4f, 0, 0.4f),2),
                new CollisionSphere (new Vector3(0,0,0.4f),2),
                new CollisionSphere (new Vector3(0.4f,0,0.4f),2)
            };

            this.sofaBounding = new CollisionSphere[] {
                new CollisionSphere (new Vector3(-2.2f, 0, -0.8f),0),
                new CollisionSphere (new Vector3(-1.8f, 0, -0.8f),0),
                new CollisionSphere (new Vector3(-1.4f, 0, -0.8f),0),
                new CollisionSphere (new Vector3(-1f, 0, -0.8f),0),
                new CollisionSphere (new Vector3(-0.6f,0, -0.8f),0),
                new CollisionSphere (new Vector3(-0.2f, 0, -0.8f),0),
                new CollisionSphere (new Vector3(0.2f, 0, -0.8f),0),
                new CollisionSphere (new Vector3(0.6f, 0, -0.8f),0),
                new CollisionSphere (new Vector3 (1f, 0, -0.8f),0),
                new CollisionSphere (new Vector3(1.4f, 0, -0.8f),0),
                new CollisionSphere (new Vector3 (1.8f, 0, -0.8f),0),
                new CollisionSphere (new Vector3 (2.2f, 0, -0.8f),0),
                new CollisionSphere (new Vector3 (-2.2f, 0, -0.4f),3),
                new CollisionSphere (new Vector3 (2.2f, 0, -0.4f),1),
                new CollisionSphere (new Vector3 (-2.2f, 0, 0),3),
                new CollisionSphere (new Vector3 (2.2f, 0, 0),1),
                new CollisionSphere (new Vector3 (-2.2f, 0, 0.4f),3),
                new CollisionSphere (new Vector3 (2.2f, 0, 0.4f),1),
                new CollisionSphere (new Vector3 (-2.2f, 0, 0.8f),2),
                new CollisionSphere (new Vector3 (-1.8f, 0, 0.8f),2),
                new CollisionSphere (new Vector3 (-1.4f, 0, 0.8f),2),
                new CollisionSphere (new Vector3 (-1f, 0, 0.8f),2),
                new CollisionSphere (new Vector3 (-0.6f, 0, 0.8f),2),
                new CollisionSphere (new Vector3 (-0.2f, 0, 0.8f),2),
                new CollisionSphere (new Vector3 (0.2f, 0, 0.8f),2),
                new CollisionSphere (new Vector3 (0.6f, 0, 0.8f),2),
                new CollisionSphere (new Vector3 (1f, 0, 0.8f),2),
                new CollisionSphere (new Vector3 (1.4f, 0, 0.8f),2),
                new CollisionSphere (new Vector3 (1.8f, 0, 0.8f),2),
                new CollisionSphere (new Vector3 (2.2f, 0, 0.8f),2)
            };

            this.kleiderschrankBounding = new CollisionSphere[] {
                new CollisionSphere (new Vector3(-1f, 0, -0.4f),0),
                new CollisionSphere (new Vector3(-0.6f, 0, -0.4f),0),
                new CollisionSphere (new Vector3(-0.2f, 0, -0.4f),0),
                new CollisionSphere (new Vector3(0.2f, 0, -0.4f),0),
                new CollisionSphere (new Vector3 (0.6f, 0, -0.4f),0),
                new CollisionSphere (new Vector3 (1f, 0, -0.4f),0),
                new CollisionSphere (new Vector3 (-1f, 0, 0),3),
                new CollisionSphere (new Vector3(1f, 0 ,0),1),
                new CollisionSphere (new Vector3(-1f, 0, 0.4f),2),
                new CollisionSphere(new Vector3(-0.6f, 0, 0.4f),2),
                new CollisionSphere (new Vector3(-0.2f, 0, 0.4f),2),
                new CollisionSphere (new Vector3(0.2f, 0, 0.4f),2),
                new CollisionSphere (new Vector3(0.6f, 0, 0.4f),2),
                new CollisionSphere (new Vector3 (1f, 0, 0.4f),2)
            };

            this.klavierBounding = new CollisionSphere[]
            {
                new CollisionSphere (new Vector3 (-1.4f, 0, -0.4f),0),
                new CollisionSphere (new Vector3 (-1f, 0, -0.4f),0),
                new CollisionSphere (new Vector3 (-0.6f, 0, -0.4f),0),
                new CollisionSphere (new Vector3 (-0.2f, 0, -0.4f),0),
                new CollisionSphere (new Vector3 (0.2f, 0, -0.4f),0),
                new CollisionSphere (new Vector3 (0.6f, 0, -0.4f),0),
                new CollisionSphere (new Vector3 (1f, 0, -0.4f),0),
                new CollisionSphere (new Vector3 (1.4f, 0, -0.4f),0),
                new CollisionSphere (new Vector3 (-1.4f, 0, 0),3),
                new CollisionSphere (new Vector3 (1.4f, 0, 0),1),
                new CollisionSphere (new Vector3 (-1.4f, 0, 0.4f),2),
                new CollisionSphere (new Vector3 (-1f, 0, 0.4f),2),
                new CollisionSphere (new Vector3 (-0.6f, 0, 0.4f),2),
                new CollisionSphere (new Vector3 (-0.2f, 0, 0.4f),2),
                new CollisionSphere (new Vector3 ( 0.2f, 0, 0.4f),2),
                new CollisionSphere (new Vector3 (0.6f, 0, 0.4f),2),
                new CollisionSphere (new Vector3 (1f , 0, 0.4f),2),
                new CollisionSphere (new Vector3 (1.4f, 0, 0.4f),2)
            };

            klavier.model = modelle[0];
            klavier.modelId = 0;
            klavier.spheres = klavierBounding;
            klavier.speed = 0.200f;
            klavier.rotationSpeed = 0.06f;
            klavier.dashpower = 0.1f;
            klavier.dashCountdown = 7000;
            klavier.power = 0.10f;
            klavier.mass = 0.10f;
            klavier.yPosition = 2.2f;
            klavier.angle = new float[] { 164.0546f, 15.9454f, 195.9454f, 344.0546f };

            kleiderschrank.model = modelle[1];
            kleiderschrank.modelId = 1;
            kleiderschrank.spheres = kleiderschrankBounding;
            kleiderschrank.speed = 0.100f;
            kleiderschrank.rotationSpeed = 0.08f;
            kleiderschrank.dashpower = 0.05f;
            kleiderschrank.dashCountdown = 7000;
            kleiderschrank.power = 0.10f;
            kleiderschrank.mass = 0.05f;
            kleiderschrank.yPosition = 2.8625f;
            kleiderschrank.angle = new float[] { 158.1986f, 21.8014f, 201.8014f, 338.1986f };

            sofa.model = modelle[2];
            sofa.modelId = 2;
            sofa.spheres = sofaBounding;
            sofa.speed = 0.100f;
            sofa.rotationSpeed = 0.08f;
            sofa.dashpower = 0.05f;
            sofa.dashCountdown = 7000;
            sofa.power = 0.10f;
            sofa.mass = 0.05f;
            sofa.yPosition = 1.7425f;
            sofa.angle = new float[] { 160.0169f, 19.9831f, 199.9831f, 340.0169f };

            kuehlschrank.model = modelle[3];
            kuehlschrank.modelId = 3;
            kuehlschrank.spheres = kuehlschrankBounding;
            kuehlschrank.speed = 0.100f;
            kuehlschrank.rotationSpeed = 0.08f;
            kuehlschrank.dashpower = 0.05f;
            kuehlschrank.dashCountdown = 7000;
            kuehlschrank.power = 0.10f;
            kuehlschrank.mass = 0.05f;
            kuehlschrank.yPosition = 2.403f;
            kuehlschrank.angle = new float[] { 135f, 45f, 225f, 315f };

        }

        public Moebel getStruct(int id)
        {

            this.id = id;

            switch (id)
            {
                case 0:
                    return klavier;


                case 1:
                    return kleiderschrank;


                case 2:
                    return sofa;


                case 3:
                    return kuehlschrank;

                default:
                    return klavier;

            }
        }
    }
}
