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
        Model klavier;
        Model kleiderschrank;
        Model stuhl;
        Model sofa;
        Model kuehlschrank;
        Model arena;
        Model ultimatesphere;
        Matrix view, projection;
        Item item;
        Player player1;
        Player player2;
        Player player3;
        Player player4;
        SpriteFont font;
       
        private BoundingBox arenaBounding;
        public CollisionManager collisionManager;
        

        enum GameState { logo, splashMenu, character, howtoplay, option, ingame, pause, result };
        private GameState gamestate;

        Button buttonPauseReturn, buttonPauseMainmenu;
        Button buttonSplahscreenStart, buttonSplashscreenHowtoplay, buttonSplashscreenOption, buttonSplashscreenExit;
        Button buttonOptionBack;
        Button buttonHowtoplayBack;

        Texture2D pausenMenu;
        Texture2D splashscreen, splashsreenButtonBackground;
        Texture2D optionMenu;
        Texture2D howtoplayscreen;

        int screenWidth = 1280, screenHeight = 720;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.ApplyChanges();
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
            view = Matrix.CreateLookAt(new Vector3(0, 20,0.1f), Vector3.Zero, Vector3.Up);

            base.Initialize();
            
            gamestate = GameState.splashMenu;



           

            arenaBounding = new BoundingBox(new Vector3(-12.5f, -1f, -12.5f), new Vector3(12.5f, 1f, 12.5f));

            Model[] modelle = new Model[] { klavier, kleiderschrank, sofa, kuehlschrank };
            CharacterManager characterManager = new CharacterManager(modelle); 

            collisionManager = new CollisionManager();
            player1 = new Player(new Vector3(0, characterManager.gibStruct(0).yPosition, -4), MathHelper.ToRadians(180f), 0, ultimatesphere, collisionManager, characterManager);
            player2 = new Player(new Vector3(0, characterManager.gibStruct(1).yPosition, 4), 0, 1, ultimatesphere, collisionManager, characterManager);
            player3 = new Player(new Vector3(4, characterManager.gibStruct(2).yPosition, 0), MathHelper.ToRadians(90f), 2, ultimatesphere, collisionManager, characterManager);
            player4 = new Player(new Vector3(-4, characterManager.gibStruct(3).yPosition, 0), MathHelper.ToRadians(-90f), 3, ultimatesphere, collisionManager, characterManager);
            collisionManager.setPlayers(new Player[] { player1, player2, player3, player4 });
            
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
            klavier = Content.Load<Model>("klavier");
            kleiderschrank = Content.Load<Model>("kleiderschrank");
            sofa = Content.Load<Model>("sofa");
            kuehlschrank = Content.Load<Model>("kuehlschrank");
            stuhl = Content.Load<Model>("stuhl");
            arena = Content.Load<Model>("arena");
            item = new Item(Content.Load<Model>("itemLangsamer"), new Vector3(0, 3, 0));
            ultimatesphere = Content.Load<Model>("ultimateSphere");
            // TODO: use this.Content to load your game content here

            pausenMenu = Content.Load<Texture2D>("Menu/Pause/PausenMenu");
            splashscreen = Content.Load<Texture2D>("Menu/splashMenu/SplashMenu");
            splashsreenButtonBackground = Content.Load<Texture2D>("Menu/splashMenu/ButtonBackground2");
            optionMenu = Content.Load<Texture2D>("Menu/Option/Background");
            howtoplayscreen = Content.Load<Texture2D>("Menu/Howtoplay/Background");

            buttonPauseReturn = new Button(Content.Load<Texture2D>("Menu/Pause/returnButton"), Content.Load<Texture2D>("Menu/Pause/returnButton2"), graphics.GraphicsDevice);
            buttonPauseReturn.setPosition(new Vector2(515, 300));

            buttonPauseMainmenu = new Button(Content.Load<Texture2D>("Menu/Pause/MainMenu"), Content.Load<Texture2D>("Menu/Pause/MainMenu2"), graphics.GraphicsDevice);
            buttonPauseMainmenu.setPosition(new Vector2(515, 475));

            buttonSplahscreenStart = new Button(Content.Load<Texture2D>("Menu/SplashMenu/StartButton"), Content.Load<Texture2D>("Menu/SplashMenu/StartButton2"), graphics.GraphicsDevice);
            buttonSplahscreenStart.setPosition(new Vector2(565, 250));

            buttonSplashscreenHowtoplay = new Button(Content.Load<Texture2D>("Menu/SplashMenu/Howtoplay"), Content.Load<Texture2D>("Menu/SplashMenu/Howtoplay2"), graphics.GraphicsDevice);
            buttonSplashscreenHowtoplay.setPosition(new Vector2(565, 340));

            buttonSplashscreenExit = new Button(Content.Load<Texture2D>("Menu/SplashMenu/ExitButton"), Content.Load<Texture2D>("Menu/SplashMenu/ExitButton2"), graphics.GraphicsDevice);
            buttonSplashscreenExit.setPosition(new Vector2(565, 520));

            buttonSplashscreenOption = new Button(Content.Load<Texture2D>("Menu/SplashMenu/OptionButton"), Content.Load<Texture2D>("Menu/SplashMenu/OptionButton2"), graphics.GraphicsDevice);
            buttonSplashscreenOption.setPosition(new Vector2(565, 430 ));

            buttonOptionBack = new Button(Content.Load<Texture2D>("Menu/Back"), Content.Load<Texture2D>("Menu/Back2"), graphics.GraphicsDevice);
            buttonOptionBack.setPosition(new Vector2(25, 600));


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

            MouseState mouse = Mouse.GetState();
            //GameState

            switch (gamestate)
            {
                case GameState.logo:
                    
                    break;

                case GameState.splashMenu:
                    
                    IsMouseVisible = true;

                    if (buttonSplahscreenStart.isClicked == true)
                    {
                        gamestate = GameState.ingame;
                        IsMouseVisible = false;
                        buttonSplahscreenStart.isClicked = false;
                    }
                    buttonSplahscreenStart.Update(mouse);

                    if (buttonSplashscreenHowtoplay.isClicked == true)
                    {
                        gamestate = GameState.howtoplay;
                        IsMouseVisible = false;
                        buttonSplashscreenHowtoplay.isClicked = false;
                    }
                    buttonSplashscreenHowtoplay.Update(mouse);

                    if (buttonSplashscreenExit.isClicked == true)
                    {
                        this.Exit();
                    }
                    buttonSplashscreenExit.Update(mouse);

                    if (buttonSplashscreenOption.isClicked == true)
                    {
                        gamestate = GameState.option;
                        IsMouseVisible = false;
                        buttonSplashscreenHowtoplay.isClicked = false;
                    }
                    buttonSplashscreenOption.Update(mouse);
                    
                    break;

                case GameState.character:
                    
                    break;

                case GameState.howtoplay:
              
                    break;

                case GameState.option:

                    IsMouseVisible = true;

                    if (buttonOptionBack.isClicked == true)
                    {
                        gamestate = GameState.splashMenu;
                        IsMouseVisible = false;
                        buttonSplahscreenStart.isClicked = false;
                    }

                    buttonOptionBack.Update(mouse);

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

                    player1.Update(player1.sphere[0].getSphere().Intersects(arenaBounding));
                    player2.Update(player2.sphere[0].getSphere().Intersects(arenaBounding));
                    player3.Update(player3.sphere[0].getSphere().Intersects(arenaBounding));
                    player4.Update(player4.sphere[0].getSphere().Intersects(arenaBounding));

                    item.update();

                    if (Keyboard.GetState().IsKeyDown(Keys.P))
                        gamestate = GameState.pause;

                    break;

                case GameState.pause:
                    
                    IsMouseVisible = true;

                    if (buttonPauseReturn.isClicked == true)
                    {
                        gamestate = GameState.ingame;
                        IsMouseVisible = false; 
                        buttonPauseReturn.isClicked = false;
                    }

                    if (buttonPauseMainmenu.isClicked == true)
                    {
                        gamestate = GameState.splashMenu;
                        buttonPauseMainmenu.isClicked = false;
                    }
                    buttonPauseMainmenu.Update(mouse);
                    buttonPauseReturn.Update(mouse);
                    
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
            spriteBatch.Begin();

            switch (gamestate)
            {
                case GameState.logo:
                    
                    break;

                case GameState.splashMenu:
                   
                    spriteBatch.Draw(splashscreen, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    spriteBatch.Draw(splashsreenButtonBackground, new Rectangle (490,150,splashsreenButtonBackground.Width,splashsreenButtonBackground.Height),Color.White);
                    buttonSplahscreenStart.Draw(spriteBatch);
                    buttonSplashscreenHowtoplay.Draw(spriteBatch);
                    buttonSplashscreenOption.Draw(spriteBatch);
                    buttonSplashscreenExit.Draw(spriteBatch);

                    break;

                case GameState.character:
                    
                    break;

                case GameState.howtoplay:

                    spriteBatch.Draw(howtoplayscreen, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

                    break;

                case GameState.option:

                    spriteBatch.Draw(optionMenu, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    buttonOptionBack.Draw(spriteBatch);

                    break;

                case GameState.ingame:

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

                    item.draw(view, projection);

                    //spriteBatch.Begin();
                    spriteBatch.DrawString(font, "Sphere0 " + player1.sphere[0].getCenterPos().ToString(), new Vector2(100, 100), Color.White);
                    spriteBatch.DrawString(font, "Sphere1 " + player1.sphere[1].getCenterPos().ToString(), new Vector2(100, 150), Color.White);
                    spriteBatch.DrawString(font, "Sphere2 " + player1.sphere[2].getCenterPos().ToString(), new Vector2(100, 200), Color.White);
                    spriteBatch.DrawString(font, "Sphere3 " + player1.sphere[3].getCenterPos().ToString(), new Vector2(100, 250), Color.White);
                    spriteBatch.DrawString(font, "radius0 " + MathHelper.ToDegrees((float)(player1.sphere[0].getAngleToModel())).ToString(), new Vector2(100, 300), Color.White);
                    spriteBatch.DrawString(font, "radius1 " + MathHelper.ToDegrees((float)(player1.sphere[1].getAngleToModel())).ToString(), new Vector2(100, 350), Color.White);
                    spriteBatch.DrawString(font, "radius2 " + MathHelper.ToDegrees((float)(player1.sphere[2].getAngleToModel())).ToString(), new Vector2(100, 400), Color.White);
                    spriteBatch.DrawString(font, "radius3 " + MathHelper.ToDegrees((float)(player1.sphere[3].getAngleToModel())).ToString(), new Vector2(100, 450), Color.White);
                    spriteBatch.DrawString(font, "collision" + (player1.sphere[0].getSphere().Intersects(arenaBounding)).ToString(), new Vector2(100, 500), Color.White);
                    //spriteBatch.End();

                    

                    base.Draw(gameTime);
                break;

            case GameState.pause:
                
                spriteBatch.Draw(pausenMenu, new Rectangle(0,0, screenWidth,screenWidth), Color.White);
                buttonPauseReturn.Draw(spriteBatch);
                buttonPauseMainmenu.Draw(spriteBatch);
                
                break;

            case GameState.result:
                
                break;

            }
            spriteBatch.End();
            //http://xboxforums.create.msdn.com/forums/t/1796.aspx :
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            //
        }
    }
}
