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
        int count;
       
        private BoundingBox arenaBounding;
        private BoundingBox groundBounding;
        public CollisionManager collisionManager;
        

        enum GameState { logo, splashMenu, character, howtoplay, ingame, pause, result };
        private GameState gamestate;

        int buttonMiddle;
        Button buttonPauseReturn, buttonPauseMainmenu;
        Button buttonSplahscreenStart, buttonSplashscreenHowtoplay, buttonSplashscreenExit;
        Button buttonCharakterBack, buttonCharakterFor;
        Button player1Back, player1For, player2Back, player2For, player3Back, player3For, player4Back, player4For;
        Button optionMute;

        Texture2D buttonBackground, alroundscreen;
        Texture2D pause;
        Texture2D splashscreen;
        Texture2D [] howtoplayscreen;
        Texture2D logoPicture, logoBesch;
        Texture2D charakterwahl, player1Char, player2Char, player3Char, player4Char;

        Color logoBeschColor;
        bool logoBeschInvisible;

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
            view = Matrix.CreateLookAt(new Vector3(3, 25,25), Vector3.Zero, Vector3.Up);

            base.Initialize();
            
            gamestate = GameState.logo;

            howtoplayscreen = new Texture2D[2];

            logoBeschColor = new Color(255, 255, 255, 255);

            buttonMiddle = 565;
            buttonPauseReturn.setPosition(new Vector2(buttonMiddle, 300));
            buttonPauseMainmenu.setPosition(new Vector2(buttonMiddle, 390));
            buttonSplahscreenStart.setPosition(new Vector2(buttonMiddle, 290));
            buttonSplashscreenHowtoplay.setPosition(new Vector2(buttonMiddle, 380));
            buttonSplashscreenExit.setPosition(new Vector2(buttonMiddle, 470));
            buttonCharakterBack.setPosition(new Vector2(30, 640));
            buttonCharakterFor.setPosition(new Vector2(1100, 640));

            player1Back.setPosition(new Vector2(270, 250));
            player1For.setPosition(new Vector2(455, 250));

            player2Back.setPosition(new Vector2(770, 250));
            player2For.setPosition(new Vector2(955, 250));

            player3Back.setPosition(new Vector2(270, 450));
            player3For.setPosition(new Vector2(455, 450));

            player4Back.setPosition(new Vector2(770, 450));
            player4For.setPosition(new Vector2(955, 450));

            arenaBounding = new BoundingBox(new Vector3(-12.5f, 1f, -12.5f), new Vector3(12.5f, 1f, 12.5f));
            groundBounding = new BoundingBox (new Vector3(-25f, -1f, -25f), new Vector3 (25f, -1f, 25f));

            Model[] modelle = new Model[] { klavier, kleiderschrank, sofa, kuehlschrank };
            CharacterManager characterManager = new CharacterManager(modelle); 

            collisionManager = new CollisionManager(arenaBounding, groundBounding);
            player1 = new Player(new Vector3(0, characterManager.getStruct(0).yPosition, -4), MathHelper.ToRadians(180f), 0, ultimatesphere, collisionManager, characterManager);
            player2 = new Player(new Vector3(0, characterManager.getStruct(1).yPosition, 4), 0, 1, ultimatesphere, collisionManager, characterManager);
            player3 = new Player(new Vector3(4, characterManager.getStruct(2).yPosition, 0), MathHelper.ToRadians(90f), 2, ultimatesphere, collisionManager, characterManager);
            player4 = new Player(new Vector3(-4, characterManager.getStruct(3).yPosition, 0), MathHelper.ToRadians(-90f), 3, ultimatesphere, collisionManager, characterManager);
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

            alroundscreen = Content.Load<Texture2D>("Menu/Logo/Background");
            logoPicture = Content.Load<Texture2D>("Menu/Logo/Logo3");
            logoBesch = Content.Load<Texture2D>("Menu/Logo/LogoBesch");
            splashscreen = Content.Load<Texture2D>("Menu/splashMenu/SplashMenu");
            buttonBackground = Content.Load<Texture2D>("Menu/splashMenu/ButtonBackground2");
            charakterwahl = Content.Load<Texture2D>("Menu/Charakterwahl/Charakterwahl");
            player1Char = Content.Load<Texture2D>("Menu/Charakterwahl/Char0");
            player2Char = Content.Load<Texture2D>("Menu/Charakterwahl/Char0");
            player3Char = Content.Load<Texture2D>("Menu/Charakterwahl/Char0");
            player4Char = Content.Load<Texture2D>("Menu/Charakterwahl/Char0");
            //howtoplayscreen[0] = Content.Load<Texture2D>("Menu/Howtoplay/Background");
            pause = Content.Load<Texture2D>("Menu/Pause/Pause");

            buttonPauseReturn = new Button(Content.Load<Texture2D>("Menu/Pause/ButtonReturn"), Content.Load<Texture2D>("Menu/Pause/ButtonReturn2"), graphics.GraphicsDevice);
            buttonPauseMainmenu = new Button(Content.Load<Texture2D>("Menu/Pause/ButtonMainmenu"), Content.Load<Texture2D>("Menu/Pause/ButtonMainmenu2"), graphics.GraphicsDevice);
            buttonSplahscreenStart = new Button(Content.Load<Texture2D>("Menu/SplashMenu/StartButton"), Content.Load<Texture2D>("Menu/SplashMenu/StartButton2"), graphics.GraphicsDevice);
            buttonSplashscreenHowtoplay = new Button(Content.Load<Texture2D>("Menu/SplashMenu/Howtoplay"), Content.Load<Texture2D>("Menu/SplashMenu/Howtoplay2"), graphics.GraphicsDevice);
            buttonSplashscreenExit = new Button(Content.Load<Texture2D>("Menu/SplashMenu/ExitButton"), Content.Load<Texture2D>("Menu/SplashMenu/ExitButton2"), graphics.GraphicsDevice);
            buttonCharakterBack = new Button(Content.Load<Texture2D>("Menu/Back"), Content.Load<Texture2D>("Menu/Back2"), graphics.GraphicsDevice);
            buttonCharakterFor = new Button(Content.Load<Texture2D>("Menu/For"), Content.Load<Texture2D>("Menu/For2"), graphics.GraphicsDevice);
            player1Back = new Button(Content.Load<Texture2D>("Menu/Charakterwahl/CBack"), Content.Load<Texture2D>("Menu/Charakterwahl/CBack2"), graphics.GraphicsDevice);
            player1For = new Button(Content.Load<Texture2D>("Menu/Charakterwahl/CFor"), Content.Load<Texture2D>("Menu/Charakterwahl/CFor2"), graphics.GraphicsDevice);
            player2Back = new Button(Content.Load<Texture2D>("Menu/Charakterwahl/CBack"), Content.Load<Texture2D>("Menu/Charakterwahl/CBack2"), graphics.GraphicsDevice);
            player2For = new Button(Content.Load<Texture2D>("Menu/Charakterwahl/CFor"), Content.Load<Texture2D>("Menu/Charakterwahl/CFor2"), graphics.GraphicsDevice);
            player3Back = new Button(Content.Load<Texture2D>("Menu/Charakterwahl/CBack"), Content.Load<Texture2D>("Menu/Charakterwahl/CBack2"), graphics.GraphicsDevice);
            player3For = new Button(Content.Load<Texture2D>("Menu/Charakterwahl/CFor"), Content.Load<Texture2D>("Menu/Charakterwahl/CFor2"), graphics.GraphicsDevice);
            player4Back = new Button(Content.Load<Texture2D>("Menu/Charakterwahl/CBack"), Content.Load<Texture2D>("Menu/Charakterwahl/CBack2"), graphics.GraphicsDevice);
            player4For = new Button(Content.Load<Texture2D>("Menu/Charakterwahl/CFor"), Content.Load<Texture2D>("Menu/Charakterwahl/CFor2"), graphics.GraphicsDevice);


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
                    
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                        gamestate = GameState.splashMenu;

                    if (logoBeschColor.A == 255) logoBeschInvisible = false;
                    if (logoBeschColor.A == 0) logoBeschInvisible = true;
                    if (logoBeschInvisible) logoBeschColor.A += 3;
                    else logoBeschColor.A -= 3;

                    break;

                case GameState.splashMenu:
                    
                    IsMouseVisible = true;

                    if (buttonSplahscreenStart.isClicked == true)
                    {
                        gamestate = GameState.character;
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
                    
                    break;

                case GameState.character:
                    
                    IsMouseVisible = true;

                    //Play1Charakterwahl Update
                    if (player1Back.isClicked == true)
                    {
                        player1Back.isClicked = false;
                    }
                    player1Back.Update(mouse);

                     if (player1For.isClicked == true)
                    {
                        player1For.isClicked = false;
                    }
                    player1For.Update(mouse);

                    //Play2Charakterwahl Update
                    if (player2Back.isClicked == true)
                    {
                        player2Back.isClicked = false;
                    }
                    player2Back.Update(mouse);

                     if (player2For.isClicked == true)
                    {
                        player2For.isClicked = false;
                    }
                    player2For.Update(mouse);

                    //Play3Charakterwahl Update
                    if (player3Back.isClicked == true)
                    {
                        player3Back.isClicked = false;
                    }
                    player3Back.Update(mouse);

                    if (player3For.isClicked == true)
                    {
                        player3For.isClicked = false;
                    }
                    player3For.Update(mouse);

                    //Play4Charakterwahl Update
                    if (player4Back.isClicked == true)
                    {
                        player4Back.isClicked = false;
                    }
                    player4Back.Update(mouse);

                    if (player4For.isClicked == true)
                    {
                        player4For.isClicked = false;
                    }
                    player4For.Update(mouse);

                    if (buttonCharakterBack.isClicked == true)
                    {
                        gamestate = GameState.splashMenu;
                        IsMouseVisible = false;
                        buttonCharakterBack.isClicked = false;
                    }
                    buttonCharakterBack.Update(mouse);

                    if (buttonCharakterFor.isClicked == true)
                    {
                        gamestate = GameState.ingame;
                        IsMouseVisible = false;
                        buttonCharakterBack.isClicked = false;
                    }
                    buttonCharakterFor.Update(mouse);

                    break;

                case GameState.howtoplay:
              
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

                    player1.Update();
                    player2.Update();
                    player3.Update();
                    player4.Update();

                    item.update();
                    count = collisionManager.checkPlayerAlive();

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
                    
                    spriteBatch.Draw(alroundscreen, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    spriteBatch.Draw(logoPicture, new Rectangle(346, 0, logoPicture.Width, logoPicture.Height), Color.White);
                    spriteBatch.Draw(logoBesch, new Rectangle(0, 420, logoBesch.Width, logoBesch.Height), logoBeschColor);

                    break;

                case GameState.splashMenu:
                   
                    spriteBatch.Draw(splashscreen, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    spriteBatch.Draw(buttonBackground, new Rectangle (490,150,buttonBackground.Width,buttonBackground.Height),Color.White);
                    buttonSplahscreenStart.Draw(spriteBatch);
                    buttonSplashscreenHowtoplay.Draw(spriteBatch);
                    buttonSplashscreenExit.Draw(spriteBatch);

                    break;

                case GameState.character:
                    
                    spriteBatch.Draw(alroundscreen, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    spriteBatch.Draw(charakterwahl, new Rectangle(0, 0, charakterwahl.Width, charakterwahl.Height), Color.White);
                    
                    player1Back.Draw(spriteBatch);
                    spriteBatch.Draw(player1Char, new Rectangle(335, 225, player1Char.Width, player1Char.Height), Color.White);
                    player1For.Draw(spriteBatch);

                    player2Back.Draw(spriteBatch);
                    spriteBatch.Draw(player2Char, new Rectangle(835, 225, player2Char.Width, player2Char.Height), Color.White);
                    player2For.Draw(spriteBatch);

                    player3Back.Draw(spriteBatch);
                    spriteBatch.Draw(player3Char, new Rectangle(335, 425, player3Char.Width, player3Char.Height), Color.White);
                    player3For.Draw(spriteBatch);

                    player4Back.Draw(spriteBatch);
                    spriteBatch.Draw(player4Char, new Rectangle(835, 425, player4Char.Width, player4Char.Height), Color.White);
                    player4For.Draw(spriteBatch);

                    buttonCharakterBack.Draw(spriteBatch);
                    buttonCharakterFor.Draw(spriteBatch);

                    break;

                case GameState.howtoplay:

                    spriteBatch.Draw(howtoplayscreen[0], new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

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
                    spriteBatch.DrawString(font, "count" +  count.ToString(), new Vector2(100, 500), Color.White);
                    //spriteBatch.End();

                    

                    base.Draw(gameTime);
                break;

            case GameState.pause:
                
                spriteBatch.Draw(alroundscreen, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                spriteBatch.Draw(pause, new Rectangle(0, 0, pause.Width, pause.Height), Color.White);
                spriteBatch.Draw(buttonBackground, new Rectangle(490, 150, buttonBackground.Width, buttonBackground.Height), Color.White);
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
