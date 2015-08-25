using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model klotz;
        Model klavier;
        Model kleiderschrank;
        Model stuhl;
        Model sofa;
        Model kuehlschrank;
        Model arena;
        Model sphere1;
        Model sphere2;
        Model sphere3;
        Model sphere4;
        Model sphere5;
        Matrix view, projection;
        float roty = 0.0f;
        Player player1;
        Player player2;
        Player player3;
        Player player4;
        Player player5;
        SpriteFont font;
        BoundingSphere[] sofaBounding;
        BoundingSphere[] kleiderschrankBounding;
        BoundingSphere[] kuehlschrankBounding;
        BoundingSphere[] klavierBounding;
        BoundingSphere[] stuhlBounding;
        private BoundingSphere bound;

        enum GameState { logo, start, character, howtoplay, option, ingame, pause, result };
        private GameState gamestate;
        private bool test = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphi4
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1280f / 720f, 0.1f, 1000f);
            view = Matrix.CreateLookAt(new Vector3(1, 40, 0), Vector3.Zero, Vector3.Up);

            base.Initialize();
            
            gamestate = GameState.ingame;
       
            

            sofaBounding = new BoundingSphere[3];
            sofaBounding[0].Center = new Vector3(0, 0.349f, 0);
            sofaBounding[1].Center = new Vector3(-1.3135f, 0.349f, 0);
            sofaBounding[2].Center = new Vector3(1.3135f, 0.349f, 0);
            for (int i = 0; i < sofaBounding.Length; i++)
            {
                sofaBounding[i].Radius = 1.0915f;
            }
           
            kleiderschrankBounding = new BoundingSphere[3];
            kleiderschrankBounding[0].Center = new Vector3(0, -1.2285f, 0);
            kleiderschrankBounding[1].Center = new Vector3(-0.616f, -1.2285f, 0);
            kleiderschrankBounding[2].Center = new Vector3(0.616f, -1.2285f, 0);
            for (int i = 0; i < kleiderschrankBounding.Length; i++)
            {
                kleiderschrankBounding[i].Radius = 0.634f;
            }
          
            kuehlschrankBounding = new BoundingSphere[1];
            kuehlschrankBounding[0].Center = new Vector3(0, -0.853f, 0);
            kuehlschrankBounding[0].Radius = 0.55f;

            klavierBounding = new BoundingSphere[5];
            klavierBounding[0].Center = new Vector3(0, -0.5325f, 0);
            klavierBounding[1].Center = new Vector3(-0.4f, -0.5325f, 0);
            klavierBounding[2].Center = new Vector3(0.4f, -0.5325f, 0);
            klavierBounding[3].Center = new Vector3(-0.9675f, -0.5325f, 0);
            klavierBounding[4].Center = new Vector3(0.9675f, -0.5325f, 0);
            for (int i = 0; i < klavierBounding.Length; i++)
            {
                klavierBounding[i].Radius = 0.5675f;
            }

            stuhlBounding = new BoundingSphere[1];
            stuhlBounding[0].Center = new Vector3(0, -0.59f, 0);
            stuhlBounding[0].Radius = 0.44f;

            //player1 = new Player(new Vector3(0, 1.8625f, -4), 0, kleiderschrank, kleiderschrankBounding, sphere2, null);
            player1 = new Player(new Vector3(0, 1.03f, -4), 0, stuhl, stuhlBounding, sphere5, null);
            player2 = new Player(new Vector3(0, 0.7425f, 4), 1, sofa, sofaBounding, sphere1, null);
            //player3 = new Player(new Vector3(0, 0, 4), 2, kuehlschrank, null);
            //player4 = new Player(new Vector3(-4, 0, 0), 3, klavier, null);
            //player5 = new Player(new Vector3(0, 0, 0), 4, arena, null, null);
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("SpriteFont1");
            klotz = Content.Load<Model>("Grundklotz");
            klavier = Content.Load<Model>("klavier");
            kleiderschrank = Content.Load<Model>("kleiderschrank");
            sofa = Content.Load<Model>("sofa");
            kuehlschrank = Content.Load<Model>("kuehlschrank");
            stuhl = Content.Load<Model>("stuhl");
            arena = Content.Load<Model>("arena");
            sphere1 = Content.Load<Model>("sphere_01");
            sphere2 = Content.Load<Model>("sphere_02");
            sphere3 = Content.Load<Model>("sphere_03");
            sphere4 = Content.Load<Model>("sphere_04");
            sphere5 = Content.Load<Model>("sphere_05");
            // TODO: use this.Content to load your game content here
            

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //GameState

            switch (gamestate)
            {
                case GameState.logo:
                    //Console.WriteLine("Case 1");
                    break;

                case GameState.start:
                    //Console.WriteLine("Case 1");
                    break;

                case GameState.character:
                    //Console.WriteLine("Case 1");
                    break;

                case GameState.howtoplay:
                    //Console.WriteLine("Case 1");
                    break;

                case GameState.option:
                    //Console.WriteLine("Case 1");
                    break;

                case GameState.ingame:
                    for (int i = 0; i < player1.getBound().Length; i++)
                    {
                        for (int j = 0; j < player2.getBound().Length; j++)
                        {
                            if (player1.getBound()[i].Intersects(player2.getBound()[j]))
                            {
                                test = false;
                            }
                        }
                    }

                    player1.Update(test);
                    player2.Update(test);
                    test = true;

                    //player1.Update(player1.getBoundingSphere().Intersects(player2.getBoundingSphere()));
                    //player2.Update(player1.getBoundingSphere().Intersects(player2.getBoundingSphere()));
                    //player3.Update(player1.getBoundingSphere().Intersects(player2.getBoundingSphere()));
                    //player4.Update(player1.getBoundingSphere().Intersects(player2.getBoundingSphere()));

                    break;

                case GameState.pause:
                    //Console.WriteLine("Case 1");
                    break;

                case GameState.result:
                    //Console.WriteLine("Case 1");
                    break;

            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            

            //GameState

            switch (gamestate)
            {
                case GameState.logo:
                    //Console.WriteLine("Case 1");
                    break;

                case GameState.start:
                    //Console.WriteLine("Case 1");
                    break;

                case GameState.character:
                    //Console.WriteLine("Case 1");
                    break;

                case GameState.howtoplay:
                    //Console.WriteLine("Case 1");
                    break;

                case GameState.option:
                    //Console.WriteLine("Case 1");
                    break;

                case GameState.ingame:
                    // TODO: Add your drawing code here
                    /*Matrix world = Matrix.Identity;
                    foreach (ModelMesh mesh in arena.Meshes)
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
                     */
                   // player5.Draw(view, projection);
                    player1.Draw(view, projection);
                    player2.Draw(view, projection);
                    //player3.Draw(view, projection);
                    //player4.Draw(view, projection);

                /*spriteBatch.Begin();
                spriteBatch.DrawString(font, "ca " + sphere1.Meshes[0].BoundingSphere.ToString(), new Vector2(100, 100), Color.White);
                spriteBatch.DrawString(font, "view " + view.ToString(), new Vector2(0, 500), Color.White);
                spriteBatch.DrawString(font, "view " + view.ToString(), new Vector2(-800, 600), Color.White);
                spriteBatch.DrawString(font, "center bs1 " + player1.getBoundingSphere().Center.ToString(), new Vector2(100, 100), Color.White);
                spriteBatch.DrawString(font, "center bs2 " + player2.getBoundingSphere().Center.ToString(), new Vector2(100, 200), Color.White);
                spriteBatch.DrawString(font, "center bs3 " + player3.getBoundingSphere().Center.ToString(), new Vector2(100, 300), Color.White);
                spriteBatch.DrawString(font, "center bs4 " + player4.getBoundingSphere().Center.ToString(), new Vector2(100, 400), Color.White);

                spriteBatch.End();
       */          
                base.Draw(gameTime);
                break;

            case GameState.pause:
                //Console.WriteLine("Case 1");
                break;

            case GameState.result:
                //Console.WriteLine("Case 1");
                break;

        }
         /*
         spriteBatch.Begin();
         spriteBatch.DrawString(font, "rotationy"+player1.rotationy.ToString(), new Vector2(100, 100), Color.White);
         spriteBatch.DrawString(font, "faktory"+player1.faktory.ToString(), new Vector2(100, 200), Color.White);
         spriteBatch.DrawString(font, "faktorz" + player1.faktorz.ToString(), new Vector2(100, 300), Color.White);
         spriteBatch.DrawString(font, "sinz" + ((float)Math.Sin((float)player1.faktory)).ToString(), new Vector2(100, 400), Color.White);
           
         spriteBatch.End();
         */
            }
        }
    }
