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
        Model ultimatesphere;
        Matrix view, projection;
        float roty = 0.0f;
        Player player1;
        Player player2;
        Player player3;
        Player player4;
        Player player5;
        SpriteFont font;
        private CollisionSphere[] sofaBounding;
        private CollisionSphere[] stuhlBounding;
        private CollisionSphere[] klavierBounding;
        private CollisionSphere[] kleiderschrankBounding;
        private CollisionSphere[] kuehlschrankBounding;
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
            view = Matrix.CreateLookAt(new Vector3(0, 40,0.1f ), Vector3.Zero, Vector3.Up);

            base.Initialize();
            
            gamestate = GameState.ingame;



            stuhlBounding = new CollisionSphere[] {
                new CollisionSphere (new Vector3(- 0.2f, 0, -0.2f)),
                new CollisionSphere (new Vector3(0.2f, 0, -0.2f)),
                new CollisionSphere (new Vector3(-0.2f, 0, 0.2f)),
                new CollisionSphere (new Vector3(0.2f, 0, 0.2f))
            };

            kuehlschrankBounding = new CollisionSphere[] {
                new CollisionSphere (new Vector3(-0.4f, 0, -0.4f)),
                new CollisionSphere (new Vector3(0,0, -0.4f)),
                new CollisionSphere (new Vector3(0.4f, 0, -0.4f)),
                new CollisionSphere (new Vector3(-0.4f, 0, 0)),
                new CollisionSphere (new Vector3(0.4f, 0,0 )),
                new CollisionSphere (new Vector3(-0.4f, 0, 0.4f)),
                new CollisionSphere (new Vector3(0,0,0.4f)),
                new CollisionSphere (new Vector3(0.4f,0,0.4f))
            };

            sofaBounding = new CollisionSphere[] {
                new CollisionSphere (new Vector3(-2.2f, 0, -0.8f)),
                new CollisionSphere (new Vector3(-1.8f, 0, -0.8f)),
                new CollisionSphere (new Vector3(-1.4f, 0, -0.8f)),
                new CollisionSphere (new Vector3(-1f, 0, -0.8f)),
                new CollisionSphere (new Vector3(-0.6f,0, -0.8f)),
                new CollisionSphere (new Vector3(-0.2f, 0, -0.8f)),
                new CollisionSphere (new Vector3(0.2f, 0, -0.8f)),
                new CollisionSphere (new Vector3(0.6f, 0, -0.8f)),
                new CollisionSphere (new Vector3 (1f, 0, -0.8f)),
                new CollisionSphere (new Vector3(1.4f, 0, -0.8f)),
                new CollisionSphere (new Vector3 (1.8f, 0, -0.8f)),
                new CollisionSphere (new Vector3 (2.2f, 0, -0.8f)),
                new CollisionSphere (new Vector3 (-2.2f, 0, -0.4f)),
                new CollisionSphere (new Vector3 (2.2f, 0, -0.4f)),
                new CollisionSphere (new Vector3 (-2.2f, 0, 0)),
                new CollisionSphere (new Vector3 (2.2f, 0, 0)),
                new CollisionSphere (new Vector3 (-2.2f, 0, 0.4f)),
                new CollisionSphere (new Vector3 (2.2f, 0, 0.4f)),
                new CollisionSphere (new Vector3 (-2.2f, 0, 0.8f)),
                new CollisionSphere (new Vector3 (-1.8f, 0, 0.8f)),
                new CollisionSphere (new Vector3 (-1.4f, 0, 0.8f)),
                new CollisionSphere (new Vector3 (-1f, 0, 0.8f)),
                new CollisionSphere (new Vector3 (-0.6f, 0, 0.8f)),
                new CollisionSphere (new Vector3 (-0.2f, 0, 0.8f)),
                new CollisionSphere (new Vector3 (0.2f, 0, 0.8f)),
                new CollisionSphere (new Vector3 (0.6f, 0, 0.8f)),
                new CollisionSphere (new Vector3 (1f, 0, 0.8f)),
                new CollisionSphere (new Vector3 (1.4f, 0, 0.8f)),
                new CollisionSphere (new Vector3 (1.8f, 0, 0.8f)),
                new CollisionSphere (new Vector3 (2.2f, 0, 0.8f))
            };

            kleiderschrankBounding = new CollisionSphere[] {
                new CollisionSphere (new Vector3(-1f, 0, -0.4f)),
                new CollisionSphere (new Vector3(-0.6f, 0, -0.4f)),
                new CollisionSphere (new Vector3(-0.2f, 0, -0.4f)),
                new CollisionSphere (new Vector3(0.2f, 0, -0.4f)),
                new CollisionSphere (new Vector3 (0.6f, 0, -0.4f)),
                new CollisionSphere (new Vector3 (1f, 0, -0.4f)),
                new CollisionSphere (new Vector3 (-1f, 0, 0)),
                new CollisionSphere (new Vector3(1f, 0 ,0)),
                new CollisionSphere (new Vector3(-1f, 0, 0.4f)),
                new CollisionSphere(new Vector3(-0.6f, 0, 0.4f)),
                new CollisionSphere (new Vector3(-0.2f, 0, 0.4f)),
                new CollisionSphere (new Vector3(0.2f, 0, 0.4f)),
                new CollisionSphere (new Vector3(0.6f, 0, 0.4f)),
                new CollisionSphere (new Vector3 (1f, 0, 0.4f))
            };

            klavierBounding = new CollisionSphere[]
            {
                new CollisionSphere (new Vector3 (-1.4f, 0, -0.4f)),
                new CollisionSphere (new Vector3 (-1f, 0, -0.4f)),
                new CollisionSphere (new Vector3 (-0.6f, 0, -0.4f)),
                new CollisionSphere (new Vector3 (-0.2f, 0, -0.4f)),
                new CollisionSphere (new Vector3 (0.2f, 0, -0.4f)),
                new CollisionSphere (new Vector3 (0.6f, 0, -0.4f)),
                new CollisionSphere (new Vector3 (1f, 0, -0.4f)),
                new CollisionSphere (new Vector3 (1.4f, 0, -0.4f)),
                new CollisionSphere (new Vector3 (-1.4f, 0, 0)),
                new CollisionSphere (new Vector3 (1.4f, 0, 0)),
                new CollisionSphere (new Vector3 (-1.4f, 0, 0.4f)),
                new CollisionSphere (new Vector3 (-1f, 0, 0.4f)),
                new CollisionSphere (new Vector3 (-0.6f, 0, 0.4f)),
                new CollisionSphere (new Vector3 (-0.2f, 0, 0.4f)),
                new CollisionSphere (new Vector3 ( 0.2f, 0, 0.4f)),
                new CollisionSphere (new Vector3 (0.6f, 0, 0.4f)),
                new CollisionSphere (new Vector3 (1f , 0, 0.4f)),
                new CollisionSphere (new Vector3 (1.4f, 0, 0.4f))
            };

            player1 = new Player(new Vector3(0, 2.2425f, -4), 0,sofa, sofaBounding, ultimatesphere);
            player2 = new Player(new Vector3(4, 2.8625f, 0), 1, kleiderschrank, kleiderschrankBounding, ultimatesphere);
            player3 = new Player(new Vector3(-4,2.403f , 0), 2, kuehlschrank, kuehlschrankBounding,ultimatesphere);
            player4 = new Player(new Vector3(0, 2.1f, 4), 3, klavier, klavierBounding,ultimatesphere);
            
            
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
            ultimatesphere = Content.Load<Model>("ultimateSphere");
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
                    
                    break;

                case GameState.start:
                    
                    break;

                case GameState.character:
                    
                    break;

                case GameState.howtoplay:
              
                    break;

                case GameState.option:
                   
                    break;

                case GameState.ingame:
                   /* for (int i = 0; i < player1.getBound().Length; i++)
                    {
                        for (int j = 0; j < player2.getBound().Length; j++)
                        {
                            if (player1.getBound()[i].Intersects(player2.getBound()[j]))
                            {
                                test = false;
                            }
                        }
                    }
                     */

                    player1.Update(test);
                    player2.Update(test);
                    player3.Update(test);
                    player4.Update(test);
                    test = true;

                    break;

                case GameState.pause:
                    
                    break;

                case GameState.result:
                    
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
                    
                    break;

                case GameState.start:
                   
                    break;

                case GameState.character:
                    
                    break;

                case GameState.howtoplay:
                    
                    break;

                case GameState.option:
                    
                    break;

                case GameState.ingame:
                    // TODO: Add your drawing code here
                   Matrix world = Matrix.Identity;
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
                     
                    player1.Draw(view, projection);
                    player2.Draw(view, projection);
                    player3.Draw(view, projection);
                    player4.Draw(view, projection);
                    
                  /*  spriteBatch.Begin();
                    spriteBatch.DrawString(font, "Sphere0 " + player1.sphere[0].getCenterPos().ToString(), new Vector2(100, 100), Color.White);
                    spriteBatch.DrawString(font, "Sphere1 " + player1.sphere[11].getCenterPos().ToString(), new Vector2(100, 150), Color.White);
                    spriteBatch.DrawString(font, "Sphere2 " + player1.sphere[18].getCenterPos().ToString(), new Vector2(100, 200), Color.White);
                    spriteBatch.DrawString(font, "Sphere3 " + player1.sphere[29].getCenterPos().ToString(), new Vector2(100, 250), Color.White);
                    spriteBatch.DrawString(font, "radius0 " + MathHelper.ToDegrees((float)(player1.sphere[0].getAngleToModel()+player1.rotationy)).ToString(), new Vector2(100, 300), Color.White);
                    spriteBatch.DrawString(font, "radius1 " + MathHelper.ToDegrees((float)(player1.sphere[11].getAngleToModel() + player1.rotationy)).ToString(), new Vector2(100, 350), Color.White);
                    spriteBatch.DrawString(font, "radius2 " + MathHelper.ToDegrees((float)(player1.sphere[18].getAngleToModel() + player1.rotationy)).ToString(), new Vector2(100, 400), Color.White);
                    spriteBatch.DrawString(font, "radius3 " + MathHelper.ToDegrees((float)(player1.sphere[29].getAngleToModel() + player1.rotationy)).ToString(), new Vector2(100, 450), Color.White);
                    spriteBatch.End();
                     */
                  
                    base.Draw(gameTime);
                break;

            case GameState.pause:
                
                break;

            case GameState.result:
                
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
