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
        bool down;

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
        Button howtoplayBack, howtoplayFor;
        Button optionMute;
        Button buttonResultRetry, buttonResultMainMenu; 

        Texture2D buttonBackground, buttonBackgroundPause;
        Texture2D pause;
        Texture2D splashscreen;
        Texture2D [] howtoplayscreen = new Texture2D[4];
        Texture2D logoPicture;
        Texture2D charakterwahl;

        Texture2D[] character = new Texture2D[5];

        Texture2D [] results = new Texture2D[4];

        int resultIndex;
        int player1Index, player2Index, player3Index, player4Index;
        int howtoplayIndex;
        Color logoAnimation;
        Color resultAnimation;
        bool logoStatus;

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
            
            gamestate = GameState.splashMenu;

            logoAnimation = new Color(255, 255, 255, 255);

            howtoplayIndex = 0;
            buttonMiddle = 565;
            buttonPauseReturn.setPosition(new Vector2(buttonMiddle, 300));
            buttonPauseMainmenu.setPosition(new Vector2(buttonMiddle, 390));
            buttonSplahscreenStart.setPosition(new Vector2(buttonMiddle, 290));
            buttonSplashscreenHowtoplay.setPosition(new Vector2(buttonMiddle, 380));
            buttonSplashscreenExit.setPosition(new Vector2(buttonMiddle, 470));
            howtoplayBack.setPosition(new Vector2(30, 640));
            howtoplayFor.setPosition(new Vector2(1100, 640));
            buttonCharakterBack.setPosition(new Vector2(30, 640));
            buttonCharakterFor.setPosition(new Vector2(1100, 640));

            buttonResultRetry.setPosition(new Vector2(1020, 440));
            buttonResultMainMenu.setPosition(new Vector2(1020, 530));

            player1Back.setPosition(new Vector2(230, 260));
            player1For.setPosition(new Vector2(545, 260));

            player2Back.setPosition(new Vector2(660, 260));
            player2For.setPosition(new Vector2(970, 260));

            player3Back.setPosition(new Vector2(230, 500));
            player3For.setPosition(new Vector2(545, 500));

            player4Back.setPosition(new Vector2(660, 500));
            player4For.setPosition(new Vector2(970, 500));

            player1Index = 0;
            player2Index = 0;
            player3Index = 0;
            player4Index = 0;

            arenaBounding = new BoundingBox(new Vector3(-12.5f, 1f, -12.5f), new Vector3(12.5f, 1f, 12.5f));
            groundBounding = new BoundingBox (new Vector3(-25f, -1f, -25f), new Vector3 (25f, -1f, 25f));

            Model[] modelle = new Model[] { klavier, kleiderschrank, sofa, kuehlschrank };
            CharacterManager characterManager = new CharacterManager(modelle);

            collisionManager = new CollisionManager(arenaBounding, groundBounding);
            player1 = new Player(new Vector3(0, characterManager.getStruct(0).yPosition, -4), MathHelper.ToRadians(180f), 0, characterManager.getStruct(0).modelId, ultimatesphere, collisionManager, characterManager);
            player2 = new Player(new Vector3(0, characterManager.getStruct(1).yPosition, 4), 0, 1, characterManager.getStruct(1).modelId, ultimatesphere, collisionManager, characterManager);
            player3 = new Player(new Vector3(4, characterManager.getStruct(2).yPosition, 0), MathHelper.ToRadians(90f), 2, characterManager.getStruct(2).modelId, ultimatesphere, collisionManager, characterManager);
            player4 = new Player(new Vector3(-4, characterManager.getStruct(3).yPosition, 0), MathHelper.ToRadians(-90f), 3, characterManager.getStruct(3).modelId, ultimatesphere, collisionManager, characterManager);
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

            logoPicture = Content.Load<Texture2D>("Menu/Logo/LogoScreen");
            splashscreen = Content.Load<Texture2D>("Menu/splashMenu/SplashMenu");
            buttonBackground = Content.Load<Texture2D>("Menu/splashMenu/ButtonBackground2");
            buttonBackgroundPause = Content.Load<Texture2D>("Menu/Pause/ButtonBackgroundPause");
            charakterwahl = Content.Load<Texture2D>("Menu/Charakterwahl/Charakterwahl");

            character[0] = Content.Load<Texture2D>("Menu/Charakterwahl/Char0");
            character[1] = Content.Load<Texture2D>("Menu/Charakterwahl/Char1");
            character[2] = Content.Load<Texture2D>("Menu/Charakterwahl/Char2");
            character[3] = Content.Load<Texture2D>("Menu/Charakterwahl/Char3");
            character[4] = Content.Load<Texture2D>("Menu/Charakterwahl/Char4");

            howtoplayscreen[0] = Content.Load<Texture2D>("Menu/Howtoplay/Screen0");
            howtoplayscreen[1] = Content.Load<Texture2D>("Menu/Howtoplay/Screen3");
            howtoplayscreen[2] = Content.Load<Texture2D>("Menu/Howtoplay/Screen2");
            howtoplayscreen[3] = Content.Load<Texture2D>("Menu/Howtoplay/Screen3");
            pause = Content.Load<Texture2D>("Menu/Pause/Pause");

            buttonPauseReturn = new Button(Content.Load<Texture2D>("Menu/Pause/ButtonReturn"), Content.Load<Texture2D>("Menu/Pause/ButtonReturn2"), graphics.GraphicsDevice);
            buttonPauseMainmenu = new Button(Content.Load<Texture2D>("Menu/Pause/ButtonMainmenu"), Content.Load<Texture2D>("Menu/Pause/ButtonMainmenu2"), graphics.GraphicsDevice);
            buttonSplahscreenStart = new Button(Content.Load<Texture2D>("Menu/SplashMenu/StartButton"), Content.Load<Texture2D>("Menu/SplashMenu/StartButton2"), graphics.GraphicsDevice);
            buttonSplashscreenHowtoplay = new Button(Content.Load<Texture2D>("Menu/SplashMenu/Howtoplay"), Content.Load<Texture2D>("Menu/SplashMenu/Howtoplay2"), graphics.GraphicsDevice);
            buttonSplashscreenExit = new Button(Content.Load<Texture2D>("Menu/SplashMenu/ExitButton"), Content.Load<Texture2D>("Menu/SplashMenu/ExitButton2"), graphics.GraphicsDevice);
            howtoplayBack = new Button(Content.Load<Texture2D>("Menu/Back"), Content.Load<Texture2D>("Menu/Back2"), graphics.GraphicsDevice);
            howtoplayFor = new Button(Content.Load<Texture2D>("Menu/For"), Content.Load<Texture2D>("Menu/For2"), graphics.GraphicsDevice);
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

            buttonResultMainMenu = new Button(Content.Load<Texture2D>("Menu/Result/ButtonMainmenu"), Content.Load<Texture2D>("Menu/Result/ButtonMainmenu2"), graphics.GraphicsDevice);
            buttonResultRetry = new Button(Content.Load<Texture2D>("Menu/Result/ButtonResultRetry"), Content.Load<Texture2D>("Menu/Result/ButtonResultRetry2"), graphics.GraphicsDevice);

            results[0] = Content.Load<Texture2D>("Menu/Result/result0");
            results[1] = Content.Load<Texture2D>("Menu/Result/result1");
            results[2] = Content.Load<Texture2D>("Menu/Result/result2");
            results[3] = Content.Load<Texture2D>("Menu/Result/result3");
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
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                down = true;
            if (Mouse.GetState().LeftButton == ButtonState.Released)
                down = false;

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

                    if (gameTime.TotalGameTime.Seconds > 1)
                    {
                        if (logoAnimation.A == 255) logoStatus = false;
                        if (logoAnimation.A == 0) logoStatus = true;
                        if (logoStatus)
                        {
                            gamestate = GameState.splashMenu;
                        }
                        else
                        {
                            logoAnimation.A -= 3;
                            logoAnimation.R -= 3;
                            logoAnimation.G -= 3;
                            logoAnimation.B -= 3;
                        }
                    }

                    break;

                case GameState.splashMenu:
                    
                    IsMouseVisible = true;

                    if (buttonSplahscreenStart.isClicked == true && !down)
                    {
                        gamestate = GameState.character;
                        IsMouseVisible = false;
                        buttonSplahscreenStart.isClicked = false;
                    }
                    buttonSplahscreenStart.Update(mouse);

                    if (buttonSplashscreenHowtoplay.isClicked == true && !down)
                    {
                        gamestate = GameState.howtoplay;
                        IsMouseVisible = false;
                        buttonSplashscreenHowtoplay.isClicked = false;
                    }
                    buttonSplashscreenHowtoplay.Update(mouse);

                    if (buttonSplashscreenExit.isClicked == true && !down)
                    {
                        this.Exit();
                    }
                    buttonSplashscreenExit.Update(mouse);
                    
                    break;

                case GameState.character:
                    
                    IsMouseVisible = true;

                    //Play1Charakterwahl Update
                    if (player1Back.isClicked == true && !down)
                    {
                        player1Index = (player1Index - 1)%5;

                        player1Back.isClicked = false;
                    }
                    player1Back.Update(mouse);

                    if (player1For.isClicked == true && !down)
                    {
                        player1Index = (player1Index + 1) % 5;

                        player1For.isClicked = false;
                    }
                    player1For.Update(mouse);

                    //Play2Charakterwahl Update
                    if (player2Back.isClicked == true && !down)
                    {
                        player2Index = (player2Index - 1) % 5;

                        player2Back.isClicked = false;
                    }
                    player2Back.Update(mouse);

                    if (player2For.isClicked == true && !down)
                    {
                        player2Index = (player2Index + 1) % 5;

                        player2For.isClicked = false;
                    }
                    player2For.Update(mouse);

                    //Play3Charakterwahl Update
                    if (player3Back.isClicked == true && !down)
                    {
                        player3Index = (player3Index - 1) % 5;

                        player3Back.isClicked = false;
                    }
                    player3Back.Update(mouse);

                    if (player3For.isClicked == true && !down)
                    {
                        player3Index = (player3Index + 1) % 5;

                        player3For.isClicked = false;
                    }
                    player3For.Update(mouse);

                    //Play4Charakterwahl Update
                    if (player4Back.isClicked == true && !down)
                    {
                        player4Index = (player4Index - 1) % 5;

                        player4Back.isClicked = false;
                    }
                    player4Back.Update(mouse);

                    if (player4For.isClicked == true && !down)
                    {
                        player4Index = (player4Index + 1) % 5;

                        player4For.isClicked = false;
                    }
                    player4For.Update(mouse);

                    if (buttonCharakterBack.isClicked == true && !down)
                    {
                        gamestate = GameState.splashMenu;
                        IsMouseVisible = false;
                        buttonCharakterBack.isClicked = false;
                    }
                    buttonCharakterBack.Update(mouse);

                    if (buttonCharakterFor.isClicked == true && !down)
                    {
                       if (player1Index != 0 && player2Index != 0)
                        {
                            if (player1Index != player2Index && player1Index != player3Index && player1Index != player4Index)
                            {
                                if (player2Index != player1Index && player2Index != player3Index && player2Index != player4Index)
                                {
                                    if (player3Index != 0)
                                    {
                                        if (player3Index != player1Index && player3Index != player2Index && player3Index != player4Index)
                                        {
                                            if (player4Index != 0)
                                            {
                                                if (player4Index != player1Index && player4Index != player2Index && player4Index != player3Index)
                                                {
                                                    gamestate = GameState.ingame;
                                                    IsMouseVisible = false;
                                                }
                                            }
                             
                                            else
                                            {
                                                gamestate = GameState.ingame;
                                                IsMouseVisible = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        gamestate = GameState.ingame;
                                        IsMouseVisible = false;
                                    }
                                }
                            }
                        }
                     }
                        
                    buttonCharakterBack.isClicked = false;
                    buttonCharakterFor.Update(mouse);

                    break;

                case GameState.howtoplay:

                    IsMouseVisible = true;

                    if (howtoplayBack.isClicked == true && !down)
                    {
                        if (howtoplayIndex == 0)
                        {
                            gamestate = GameState.splashMenu;
                            howtoplayBack.isClicked = false;
                            IsMouseVisible = false;
                        }
                        else
                        {
                            howtoplayIndex -= 1;
                            howtoplayBack.isClicked = false;
                        }

                    }
                    howtoplayBack.Update(mouse);

                    if (howtoplayFor.isClicked == true)
                    {
                        if (howtoplayIndex == 3)
                        {
                            gamestate = GameState.splashMenu;
                            howtoplayIndex = 0;
                            howtoplayFor.isClicked = false;
                            IsMouseVisible = false;
                        }
                        else
                        {
                            howtoplayIndex += 1;
                            howtoplayFor.isClicked = false;
                        }

                    }
                    howtoplayFor.Update(mouse);

                    break;

                case GameState.ingame:

                    player1.Update();
                    player2.Update();
                    player3.Update();
                    player4.Update();

                    item.update();
                    count = collisionManager.checkPlayerAlive();

                    if (Keyboard.GetState().IsKeyDown(Keys.P) && !down)
                        gamestate = GameState.pause;

                    if (count == 1)
                    {
                        gamestate = GameState.result;
                    }

                    break;

                case GameState.pause:
                    
                    IsMouseVisible = true;

                    if (buttonPauseReturn.isClicked == true && !down)
                    {
                        gamestate = GameState.ingame;
                        IsMouseVisible = false; 
                        buttonPauseReturn.isClicked = false;
                    }

                    if (buttonPauseMainmenu.isClicked == true && !down)
                    {
                        gamestate = GameState.splashMenu;
                        buttonPauseMainmenu.isClicked = false;
                    }
                    buttonPauseMainmenu.Update(mouse);
                    buttonPauseReturn.Update(mouse);
                    
                    break;

                case GameState.result:

                    resultIndex = collisionManager.winner();
                    IsMouseVisible = true;

                    if (buttonResultMainMenu.isClicked == true && !down)
                    {
                        gamestate = GameState.splashMenu;
                        IsMouseVisible = false;
                        buttonResultMainMenu.isClicked = false;
                    }
                    buttonResultMainMenu.Update(mouse);

                    
                    if (buttonResultRetry.isClicked == true && !down)
                    {                       
                        gamestate = GameState.character;
                        IsMouseVisible = false;
                        buttonResultRetry.isClicked = false;
                    }
                    buttonResultRetry.Update(mouse);


                    break;

            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            
            

            //GameState
            spriteBatch.Begin();

            switch (gamestate)
            {
                case GameState.logo:

                    spriteBatch.Draw(logoPicture, new Rectangle(0, 0, logoPicture.Width, logoPicture.Height), logoAnimation);

                    break;

                case GameState.splashMenu:
                   
                    spriteBatch.Draw(splashscreen, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    spriteBatch.Draw(buttonBackground, new Rectangle (490,150,buttonBackground.Width,buttonBackground.Height),Color.White);
                    buttonSplahscreenStart.Draw(spriteBatch);
                    buttonSplashscreenHowtoplay.Draw(spriteBatch);
                    buttonSplashscreenExit.Draw(spriteBatch);

                    break;

                case GameState.character:
                    spriteBatch.Draw(charakterwahl, new Rectangle(0, 0, charakterwahl.Width, charakterwahl.Height), Color.White);
                    
                    player1Back.Draw(spriteBatch);
                    spriteBatch.Draw(character[player1Index], new Rectangle(295, 180, character[player1Index].Width, character[player1Index].Height), Color.White);
                    player1For.Draw(spriteBatch);

                    player2Back.Draw(spriteBatch);
                    spriteBatch.Draw(character[player2Index], new Rectangle(725, 180, character[player2Index].Width, character[player2Index].Height), Color.White);
                    player2For.Draw(spriteBatch);

                    player3Back.Draw(spriteBatch);
                    spriteBatch.Draw(character[player3Index], new Rectangle(295, 425, character[player3Index].Width, character[player3Index].Height), Color.White);
                    player3For.Draw(spriteBatch);

                    player4Back.Draw(spriteBatch);
                    spriteBatch.Draw(character[player4Index], new Rectangle(725, 425, character[player4Index].Width, character[player4Index].Height), Color.White);
                    player4For.Draw(spriteBatch);

                    buttonCharakterBack.Draw(spriteBatch);
                    buttonCharakterFor.Draw(spriteBatch);

                    break;

                case GameState.howtoplay:

                    spriteBatch.Draw(howtoplayscreen[howtoplayIndex], new Rectangle(0, 0, howtoplayscreen[howtoplayIndex].Width, howtoplayscreen[howtoplayIndex].Height), Color.White);
                    howtoplayBack.Draw(spriteBatch);
                    howtoplayFor.Draw(spriteBatch);

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

                    //spriteBatch.DrawString(font, "Sphere0 " + player1.sphere[0].getCenterPos().ToString(), new Vector2(100, 100), Color.White);
                    //spriteBatch.DrawString(font, "Sphere1 " + player1.sphere[1].getCenterPos().ToString(), new Vector2(100, 150), Color.White);                  

                    base.Draw(gameTime);
                break;

            case GameState.pause:
                
                spriteBatch.Draw(pause, new Rectangle(0, 0, pause.Width, pause.Height), Color.White);
                spriteBatch.Draw(buttonBackgroundPause, new Rectangle(480, 150, buttonBackgroundPause.Width, buttonBackgroundPause.Height), Color.White);
                buttonPauseReturn.Draw(spriteBatch);
                buttonPauseMainmenu.Draw(spriteBatch);
                
                break;

            case GameState.result:

                spriteBatch.Draw(results[resultIndex], new Rectangle(0, 0, results[resultIndex].Width, results[resultIndex].Height), Color.White);

                buttonResultMainMenu.Draw(spriteBatch);
                buttonResultRetry.Draw(spriteBatch);

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
