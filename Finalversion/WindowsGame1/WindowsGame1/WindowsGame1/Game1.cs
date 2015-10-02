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
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Model klavier,kleiderschrank,stuhl,sofa,kuehlschrank,arena,ultimatesphere,ground;
        private Matrix view, projection;
        private ItemManager item;
        private Model[] identifier;
        private Model itemSchatten;
        private List<Player> playerList;
        private Vector3[] spawnPoints;
        private float[] spawnRotation;
        private CharacterManager characterManager;

        private SpriteFont font,font2;
        private bool [,] buttonDown = new bool[4,6]; // Spalten (index 0 = Up, index 1 = Down, index 2 = A, index 3 = Left, index 4 = Right, index 5 = Start
        private bool showError;
        private bool mute,drawX;
        private bool gamePadOn;
        private bool [] charakterMenuPosition = new bool [4]; //true Oben, false Unten
   
        private BoundingBox arenaBounding;
        private BoundingBox groundBounding;
        private BoundingBox waterBounding;

        private CollisionManager collisionManager;


        private enum GameState {logo, splashMenu, character, howtoplay, ingame, pause, result, credits};
        private GameState gamestate;

        private Button [] buttonPause = new Button [2];
        private Button [] buttonCharakter = new Button[2]; // index 0 = Back , index 1 = For

        private Button player1Back, player1For, player2Back, player2For, player3Back, player3For, player4Back, player4For;
        private Button [] howtoplay = new Button [2];
        private Button [] splashMenuButtons = new Button[4]; //index 0 = Start , index 1 = Howtoplay, index 2 = Exit, index 3 = Mute
        private Button[] buttonResult = new Button[2];

        private Texture2D buttonBackground, buttonBackgroundPause;
        private Texture2D pause;
        private Texture2D splashscreen, lautsprecherX;
        private Texture2D [] howtoplayscreen = new Texture2D[5];
        private Texture2D logoPicture,logoklein;
        private Texture2D creditsPicture;
        private Texture2D charakterwahl;

        private Texture2D[] character = new Texture2D[5];
        private Texture2D ready;

        private Texture2D[] results = new Texture2D[4];

        private int[] menuIndex = new int[5]; //index 0 = Splashscreen, index 1 = Charakter, index 2 = How to play, index 3 = Pause, index 4 = Result
        private int resultIndex;
        private int player1Index, player2Index, player3Index, player4Index;
        private Color logoAnimation;
        private Color creditsAnimation;
        private bool logoStatus;
        private bool creditsStatus;

        private int screenWidth = 1280, screenHeight = 720;

        private Song backgroundMusic;
        private SoundEffect bubble;
        private SoundEffect collectItem;
        private SoundEffect dashEffect;
        private SoundEffect crashEffect;
        public Model[] items;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
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

            spawnPoints = new Vector3[]{new Vector3(0,0,-4),new Vector3(0,0,4),new Vector3(4,0,0),new Vector3(-4,0,0)};
            spawnRotation = new float[] { MathHelper.ToRadians(180f), 0, MathHelper.ToRadians(90f), MathHelper.ToRadians(-90f) };
            
            gamestate = GameState.logo;
            playerList = new List<Player>();

            logoAnimation = new Color(255, 255, 255, 255);
            creditsAnimation = new Color(255, 255, 255, 255);

            for (int i = 0; i < 5; i++)
                menuIndex[i] = 0;

            buttonPause[0].setPosition(new Vector2(555, 300));
            buttonPause[1].setPosition(new Vector2(555, 390));

            splashMenuButtons[0].setPosition(new Vector2(565, 290));
            splashMenuButtons[1].setPosition(new Vector2(565, 380));
            splashMenuButtons[2].setPosition(new Vector2(565, 470));
            splashMenuButtons[3].setPosition(new Vector2(565, 555));
         
            howtoplay[0].setPosition(new Vector2(30, 640));
            howtoplay[1].setPosition(new Vector2(1100, 640));
            buttonCharakter[0].setPosition(new Vector2(30, 640));
            buttonCharakter[1].setPosition(new Vector2(1100, 640));

            buttonResult[0].setPosition(new Vector2(1020, 510));
            buttonResult[1].setPosition(new Vector2(1020, 600));

            player1Back.setPosition(new Vector2(230, 260));
            player1For.setPosition(new Vector2(545, 260));

            player2Back.setPosition(new Vector2(660, 260));
            player2For.setPosition(new Vector2(970, 260));

            player3Back.setPosition(new Vector2(230, 500));
            player3For.setPosition(new Vector2(545, 500));

            player4Back.setPosition(new Vector2(660, 500));
            player4For.setPosition(new Vector2(970, 500));

            player1Index = 4;
            player2Index = 4;
            player3Index = 4;
            player4Index = 4;

            arenaBounding = new BoundingBox(new Vector3(-12.5f, 1f, -12.5f), new Vector3(12.5f, 1f, 12.5f));
            groundBounding = new BoundingBox (new Vector3(-25f, -7f, -25f), new Vector3 (25f, -7f, 25f));
            waterBounding = new BoundingBox(new Vector3(-25f, -1f, -25f), new Vector3(25f, -1f, 25f));

           
            Model[] modelle = new Model[] { klavier, kleiderschrank, sofa, kuehlschrank };
            characterManager = new CharacterManager(modelle);
            collisionManager = new CollisionManager(arenaBounding, groundBounding,crashEffect);
            item = new ItemManager(items, collisionManager,itemSchatten);


            showError = false;

            mute = false;
            drawX = false;
            gamePadOn = GamePad.GetState(PlayerIndex.One).IsConnected;

            for (int i = 0; i <= 3; i++)
            {
                charakterMenuPosition[i] = true;
            }
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
            font2 = Content.Load<SpriteFont>("SpriteFont2");
            klavier = Content.Load<Model>("Moebel/klavier");
            kleiderschrank = Content.Load<Model>("Moebel/kleiderschrank");
            sofa = Content.Load<Model>("Moebel/sofa");
            kuehlschrank = Content.Load<Model>("Moebel/kuehlschrank");
            stuhl = Content.Load<Model>("Moebel/stuhl");
            arena = Content.Load<Model>("arena");
            items = new Model[] { (Content.Load<Model>("Items/itemLangsamer")), (Content.Load<Model>("Items/itemSchneller")), (Content.Load<Model>("Items/itemPower")), (Content.Load<Model>("Items/itemUmkehren")) };
            ultimatesphere = Content.Load<Model>("ultimateSphere");
            identifier = new Model[] { Content.Load<Model>("player1"), Content.Load<Model>("player2"), Content.Load<Model>("player3"), Content.Load<Model>("player4") };
            ground = Content.Load<Model>("wasserBoden");
            itemSchatten = Content.Load<Model>("schattenItems");
            // Laden aller Texter2D Dateien 
            logoPicture = Content.Load<Texture2D>("Menu/Logo/LogoScreen");
            splashscreen = Content.Load<Texture2D>("Menu/splashMenu/SplashMenu");
            logoklein = Content.Load<Texture2D>("Menu/splashMenu/logo");
            buttonBackground = Content.Load<Texture2D>("Menu/splashMenu/ButtonBackground2");
            buttonBackgroundPause = Content.Load<Texture2D>("Menu/Pause/ButtonBackgroundPause");
            lautsprecherX = Content.Load<Texture2D>("Menu/splashMenu/lautsprecherX");
            charakterwahl = Content.Load<Texture2D>("Menu/Charakterwahl/Charakterwahl");
            character[0] = Content.Load<Texture2D>("Menu/Charakterwahl/Char1");
            character[1] = Content.Load<Texture2D>("Menu/Charakterwahl/Char2");
            character[2] = Content.Load<Texture2D>("Menu/Charakterwahl/Char3");
            character[3] = Content.Load<Texture2D>("Menu/Charakterwahl/Char4");
            character[4] = Content.Load<Texture2D>("Menu/Charakterwahl/Char0");
            ready = Content.Load<Texture2D>("Menu/Charakterwahl/Ready");
            howtoplayscreen[0] = Content.Load<Texture2D>("Menu/Howtoplay/Screen0");
            howtoplayscreen[1] = Content.Load<Texture2D>("Menu/Howtoplay/Screen1");
            howtoplayscreen[2] = Content.Load<Texture2D>("Menu/Howtoplay/Screen2");
            howtoplayscreen[3] = Content.Load<Texture2D>("Menu/Howtoplay/Screen3");
            howtoplayscreen[4] = Content.Load<Texture2D>("Menu/Howtoplay/Screen4");
            pause = Content.Load<Texture2D>("Menu/Pause/Background");
            creditsPicture = Content.Load<Texture2D>("Menu/Credits/Credits");
            results[0] = Content.Load<Texture2D>("Menu/Result/result0");
            results[1] = Content.Load<Texture2D>("Menu/Result/result1");
            results[2] = Content.Load<Texture2D>("Menu/Result/result2");
            results[3] = Content.Load<Texture2D>("Menu/Result/result3");

            //Initialisieren aller Buttons
            buttonPause[0] = new Button(Content.Load<Texture2D>("Menu/Pause/ButtonReturn"), Content.Load<Texture2D>("Menu/Pause/ButtonReturn2"), graphics.GraphicsDevice);
            buttonPause[1] = new Button(Content.Load<Texture2D>("Menu/Pause/ButtonMainmenu"), Content.Load<Texture2D>("Menu/Pause/ButtonMainmenu2"), graphics.GraphicsDevice);
            splashMenuButtons[0] = new Button(Content.Load<Texture2D>("Menu/SplashMenu/StartButton"), Content.Load<Texture2D>("Menu/SplashMenu/StartButton2"), graphics.GraphicsDevice);
            splashMenuButtons[1] = new Button(Content.Load<Texture2D>("Menu/SplashMenu/Howtoplay"), Content.Load<Texture2D>("Menu/SplashMenu/Howtoplay2"), graphics.GraphicsDevice);
            splashMenuButtons[2] = new Button(Content.Load<Texture2D>("Menu/SplashMenu/ExitButton"), Content.Load<Texture2D>("Menu/SplashMenu/ExitButton2"), graphics.GraphicsDevice);
            splashMenuButtons[3] = new Button(Content.Load<Texture2D>("Menu/splashMenu/Lautsprecher"), Content.Load<Texture2D>("Menu/splashMenu/Lautsprecher2"), graphics.GraphicsDevice);
            howtoplay[0] = new Button(Content.Load<Texture2D>("Menu/Back"), Content.Load<Texture2D>("Menu/Back2"), graphics.GraphicsDevice);
            howtoplay[1] = new Button(Content.Load<Texture2D>("Menu/For"), Content.Load<Texture2D>("Menu/For2"), graphics.GraphicsDevice);
            buttonCharakter[0] = new Button(Content.Load<Texture2D>("Menu/Back"), Content.Load<Texture2D>("Menu/Back2"), graphics.GraphicsDevice);
            buttonCharakter[1] = new Button(Content.Load<Texture2D>("Menu/For"), Content.Load<Texture2D>("Menu/For2"), graphics.GraphicsDevice);
            player1Back = new Button(Content.Load<Texture2D>("Menu/Charakterwahl/CBack"), Content.Load<Texture2D>("Menu/Charakterwahl/CBack2"), graphics.GraphicsDevice);
            player1For = new Button(Content.Load<Texture2D>("Menu/Charakterwahl/CFor"), Content.Load<Texture2D>("Menu/Charakterwahl/CFor2"), graphics.GraphicsDevice);
            player2Back = new Button(Content.Load<Texture2D>("Menu/Charakterwahl/CBack"), Content.Load<Texture2D>("Menu/Charakterwahl/CBack2"), graphics.GraphicsDevice);
            player2For = new Button(Content.Load<Texture2D>("Menu/Charakterwahl/CFor"), Content.Load<Texture2D>("Menu/Charakterwahl/CFor2"), graphics.GraphicsDevice);
            player3Back = new Button(Content.Load<Texture2D>("Menu/Charakterwahl/CBack"), Content.Load<Texture2D>("Menu/Charakterwahl/CBack2"), graphics.GraphicsDevice);
            player3For = new Button(Content.Load<Texture2D>("Menu/Charakterwahl/CFor"), Content.Load<Texture2D>("Menu/Charakterwahl/CFor2"), graphics.GraphicsDevice);
            player4Back = new Button(Content.Load<Texture2D>("Menu/Charakterwahl/CBack"), Content.Load<Texture2D>("Menu/Charakterwahl/CBack2"), graphics.GraphicsDevice);
            player4For = new Button(Content.Load<Texture2D>("Menu/Charakterwahl/CFor"), Content.Load<Texture2D>("Menu/Charakterwahl/CFor2"), graphics.GraphicsDevice);
            buttonResult[1] = new Button(Content.Load<Texture2D>("Menu/Result/ButtonMainmenu"), Content.Load<Texture2D>("Menu/Result/ButtonMainmenu2"), graphics.GraphicsDevice);
            buttonResult[0] = new Button(Content.Load<Texture2D>("Menu/Result/ButtonResultNewGame"), Content.Load<Texture2D>("Menu/Result/ButtonResultNewGame2"), graphics.GraphicsDevice);

            backgroundMusic = Content.Load<Song>("Sound/Rocket");
            bubble = Content.Load<SoundEffect>("Sound/bubble");
            collectItem = Content.Load<SoundEffect>("Sound/collectItem");
            dashEffect = Content.Load<SoundEffect>("Sound/Dash");
            crashEffect = Content.Load<SoundEffect>("Sound/crash");
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

/*
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);

            if (currentState.IsConnected && currentState.Buttons.A ==
                ButtonState.Pressed)
            {
                vibrationAmount =
                    MathHelper.Clamp(vibrationAmount + 0.03f, 0.0f, 1.0f);
                GamePad.SetVibration(PlayerIndex.One,
                    vibrationAmount, vibrationAmount);
            }
            else
            {
                vibrationAmount =
                    MathHelper.Clamp(vibrationAmount - 0.05f, 0.0f, 1.0f);
                GamePad.SetVibration(PlayerIndex.One,
                    vibrationAmount, vibrationAmount);
            }
            */


            if (MediaPlayer.State != MediaState.Playing) MediaPlayer.Play(backgroundMusic);

          //
 	    if (gamePadOn == true)
            {
                if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Released)
                    buttonDown[0,0] = false;

                if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Released)
                    buttonDown[0,1] = false;

                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Released)
                    buttonDown[0,2] = false;

                if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Released)
                    buttonDown[0, 3] = false;

                if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Released)
                    buttonDown[0, 4] = false;

                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Released)
                    buttonDown[0, 5] = false;


                if (GamePad.GetState(PlayerIndex.Two).DPad.Up == ButtonState.Released)
                    buttonDown[1, 0] = false;

                if (GamePad.GetState(PlayerIndex.Two).DPad.Down == ButtonState.Released)
                    buttonDown[1, 1] = false;

                if (GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Released)
                    buttonDown[1, 2] = false;

                if (GamePad.GetState(PlayerIndex.Two).DPad.Left == ButtonState.Released)
                    buttonDown[1, 3] = false;

                if (GamePad.GetState(PlayerIndex.Two).DPad.Right == ButtonState.Released)
                    buttonDown[1, 4] = false;

                if (GamePad.GetState(PlayerIndex.Two).Buttons.Start == ButtonState.Released)
                    buttonDown[1, 5] = false;


                if (GamePad.GetState(PlayerIndex.Three).DPad.Up == ButtonState.Released)
                    buttonDown[2, 0] = false;

                if (GamePad.GetState(PlayerIndex.Three).DPad.Down == ButtonState.Released)
                    buttonDown[2, 1] = false;

                if (GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Released)
                    buttonDown[2, 2] = false;

                if (GamePad.GetState(PlayerIndex.Three).DPad.Left == ButtonState.Released)
                    buttonDown[2, 3] = false;

                if (GamePad.GetState(PlayerIndex.Three).DPad.Right == ButtonState.Released)
                    buttonDown[2, 4] = false;

                if (GamePad.GetState(PlayerIndex.Three).Buttons.Start == ButtonState.Released)
                    buttonDown[2, 5] = false;


                if (GamePad.GetState(PlayerIndex.Four).DPad.Up == ButtonState.Released)
                    buttonDown[3, 0] = false;

                if (GamePad.GetState(PlayerIndex.Four).DPad.Down == ButtonState.Released)
                    buttonDown[3, 1] = false;

                if (GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Released)
                    buttonDown[3, 2] = false;

                if (GamePad.GetState(PlayerIndex.Four).DPad.Left == ButtonState.Released)
                    buttonDown[3, 3] = false;

                if (GamePad.GetState(PlayerIndex.Four).DPad.Right == ButtonState.Released)
                    buttonDown[3, 4] = false;

                if (GamePad.GetState(PlayerIndex.Four).Buttons.Start == ButtonState.Released)
                    buttonDown[3, 5] = false;

	    }
            else
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    buttonDown[0,0] = true;
                if (Mouse.GetState().LeftButton == ButtonState.Released)
                    buttonDown[0,0] = false;
            }

            base.Update(gameTime);
            
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Two).Buttons.Back == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Three).Buttons.Back == ButtonState.Pressed || GamePad.GetState(PlayerIndex.Four).Buttons.Back == ButtonState.Pressed)
                gamestate = GameState.credits;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                gamestate = GameState.credits;

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
                    
                    if (gamePadOn == true)
                    {
                        splashMenuButtons[menuIndex[0]].UpdatePad(1);

                        if ((GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && !buttonDown[0, 0]) || (GamePad.GetState(PlayerIndex.Two).DPad.Up == ButtonState.Pressed && !buttonDown[1, 0]) || (GamePad.GetState(PlayerIndex.Three).DPad.Up == ButtonState.Pressed && !buttonDown[2,0] ) || ( GamePad.GetState(PlayerIndex.Four).DPad.Up == ButtonState.Pressed && !buttonDown[3,0]))
                        {
                            if (menuIndex[0] == 0)
                            {
                                splashMenuButtons[menuIndex[0]].UpdatePad(0);
                                menuIndex[0] = 3;
                                splashMenuButtons[menuIndex[0]].UpdatePad(1);
                            }
                            else
                            {
                                splashMenuButtons[menuIndex[0]].UpdatePad(0);
                                menuIndex[0] -= 1;
                                splashMenuButtons[menuIndex[0]].UpdatePad(1);
                            }

                            for (int i = 0; i <= 3; i++)
                            {
                                buttonDown[i, 0] = true;
                            }
                        }

                        if (( GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed && !buttonDown[0, 1] ) || ( GamePad.GetState(PlayerIndex.Two).DPad.Down == ButtonState.Pressed && !buttonDown[1, 1] ) || ( GamePad.GetState(PlayerIndex.Three).DPad.Down == ButtonState.Pressed && !buttonDown[2, 1] ) || ( GamePad.GetState(PlayerIndex.Four).DPad.Down == ButtonState.Pressed && !buttonDown[3, 1] ))
                        {
                            if (menuIndex[0] == 3)
                            {
                                splashMenuButtons[menuIndex[0]].UpdatePad(0);
                                menuIndex[0] = 0;
                                splashMenuButtons[menuIndex[0]].UpdatePad(1);
                            }
                            else
                            {
                                splashMenuButtons[menuIndex[0]].UpdatePad(0);
                                menuIndex[0] += 1;
                                splashMenuButtons[menuIndex[0]].UpdatePad(1);
                            }

                            for (int i = 0; i <= 3; i++)
                            {
                                buttonDown[i, 1] = true;
                            }
                        }

                        if ((GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && !buttonDown[0, 2]) || (GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed && !buttonDown[1, 2]) || (GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed && !buttonDown[2, 2]) || (GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed && !buttonDown[3, 2]))
                        {
                            if (menuIndex[0] == 0) gamestate = GameState.character;
                            if (menuIndex[0] == 1) gamestate = GameState.howtoplay;
                            if (menuIndex[0] == 2) this.Exit();
                            if (menuIndex[0] == 3)
                            {
                                mute = !mute;
                                if (mute)
                                {
                                    MediaPlayer.Volume = 0;
                                }
                                else
                                {
                                    MediaPlayer.Volume = 1;
                                }
                                
                                drawX = !drawX;
                            }

                            for (int i = 0; i <= 3; i++)
                            {
                                buttonDown[i, 2] = true;
                            }
                        }
                    }
                    else
                    {
                        IsMouseVisible = true;

                        if (splashMenuButtons[0].isClicked == true && !buttonDown[0,0])
                        {
                            gamestate = GameState.character;
                            IsMouseVisible = false;
                            splashMenuButtons[0].isClicked = false;
                        }
                        splashMenuButtons[0].Update(mouse);

                        if (splashMenuButtons[1].isClicked == true && !buttonDown[0, 0])
                        {
                            gamestate = GameState.howtoplay;
                            IsMouseVisible = false;
                            splashMenuButtons[1].isClicked = false;
                        }
                        splashMenuButtons[1].Update(mouse);

                        if (splashMenuButtons[2].isClicked == true && !buttonDown[0, 0])
                        {
                            gamestate = GameState.credits;
                        }
                        splashMenuButtons[2].Update(mouse);

                        if (splashMenuButtons[3].isClicked == true && !buttonDown[0, 0])
                        {
                           mute = !mute;
                                if (mute)
                                {
                                    MediaPlayer.Volume = 0;
                                }
                                else
                                {
                                    MediaPlayer.Volume = 1;
                                }

                                drawX = !drawX;

                            splashMenuButtons[3].isClicked = false;


                        }
                        splashMenuButtons[3].Update(mouse);
                    }
                    break;

                case GameState.character:

                    if (gamePadOn == true)
                    {

                        //Play1Charakterwahl Update
                        if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && !buttonDown[0, 0])
                        {
                            if (charakterMenuPosition[0] == true) charakterMenuPosition[0] = false;
                            else charakterMenuPosition[0] = true;
                            buttonDown[0, 0] = true;
                        }

                        if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed && !buttonDown[0, 1])
                        {
                            if (charakterMenuPosition[0] == false) charakterMenuPosition[0] = true;
                            else charakterMenuPosition[0] = false;
                            buttonCharakter[1].UpdatePad(1);
                            buttonDown[0, 1] = true;
                        }

                        if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed && !buttonDown[0, 4])
                        {
                            if (charakterMenuPosition[0] == true)
                            {
                                player1Index = (player1Index + 1) % 5;
                                buttonDown[0, 4] = true;
                            }
                            else
                            {
                                buttonCharakter[menuIndex[1]].UpdatePad(0);
                                if (menuIndex[1] == 1) menuIndex[1] = 0;
                                else menuIndex[1] += 1;
                                buttonCharakter[menuIndex[1]].UpdatePad(1);
                                buttonDown[0, 4] = true;
                            }
                        }
                        if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed && !buttonDown[0, 3])
                        {
                            if (charakterMenuPosition[0] == true)
                            {
                                player1Index = (player1Index - 1);
                                if (player1Index == -1) player1Index = 4;
                                buttonDown[0, 3] = true;
                            }
                            else
                            {
                                buttonCharakter[menuIndex[1]].UpdatePad(0);
                                if (menuIndex[1] == 0) menuIndex[1] = 1;
                                else menuIndex[1] -= 1;
                                buttonCharakter[menuIndex[1]].UpdatePad(1);
                                buttonDown[0, 3] = true;
                            }
                        }

                        if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && !buttonDown[0, 2])
                        {
                            if (charakterMenuPosition[0] == true)
                            {
                                if (player1Index == 0)
                                {
                                    buttonDown[0, 2] = true;
                                }
                                else
                                {
                                    buttonDown[0, 2] = true;
                                }
                            }
                            else
                            {
                                if (menuIndex[1] == 0)
                                {
                                    gamestate = GameState.splashMenu;
                                    buttonDown[0, 2] = true;
                                }
                                else
                                {
                                    List<int> playerIndex = new List<int> { player1Index, player2Index, player3Index, player4Index };
                                    playerList.Clear();
                                    if (playerIndex.FindAll(index => index == 4).Count < 3)
                                    {
                                        for (int i = 0; i < playerIndex.Count; i++)
                                        {
                                            if (playerIndex[i] != 4)
                                            {
                                                playerList.Add(new Player(spawnPoints[playerList.Count], spawnRotation[playerList.Count], i, collisionManager, characterManager.getStruct(playerIndex[i]), ultimatesphere, item,dashEffect));
                                            }
                                        }

                                        collisionManager.setPlayers(playerList);

                                        gamestate = GameState.ingame;
                                        buttonDown[0, 2] = true;
                                        showError = false;

                                    }
                                    else
                                    {
                                        showError = true; 
                                        buttonDown[0, 2] = true;
                                    }
                                }
                            }
                        }

                        //Play2Charakterwahl Update
                        if (GamePad.GetState(PlayerIndex.Two).DPad.Up == ButtonState.Pressed && !buttonDown[1, 0])
                        {
                            if (charakterMenuPosition[1] == true) charakterMenuPosition[1] = false;
                            else charakterMenuPosition[1] = true;
                            buttonDown[1, 0] = true;
                        }

                        if (GamePad.GetState(PlayerIndex.Two).DPad.Down == ButtonState.Pressed && !buttonDown[1, 1])
                        {
                            if (charakterMenuPosition[1] == false) charakterMenuPosition[1] = true;
                            else charakterMenuPosition[1] = false;
                            buttonDown[1, 1] = true;
                        }

                        if (GamePad.GetState(PlayerIndex.Two).DPad.Right == ButtonState.Pressed && !buttonDown[1, 4])
                        {
                            if (charakterMenuPosition[1] == true)
                            {
                                player2Index = (player2Index + 1) % 5;
                                buttonDown[1, 4] = true;
                            }
                            else
                            {
                                buttonCharakter[menuIndex[1]].UpdatePad(0);
                                if (menuIndex[1] == 1) menuIndex[1] = 0;
                                else menuIndex[1] += 1;
                                buttonCharakter[menuIndex[1]].UpdatePad(1);
                                buttonDown[1, 4] = true;
                            }
                        }
                        if (GamePad.GetState(PlayerIndex.Two).DPad.Left == ButtonState.Pressed && !buttonDown[1, 3])
                        {
                            if (charakterMenuPosition[1] == true)
                            {
                                player2Index = (player2Index - 1);
                                if (player2Index == -1) player1Index = 4;
                                buttonDown[1, 3] = true;
                            }
                            else
                            {
                                buttonCharakter[menuIndex[1]].UpdatePad(0);
                                if (menuIndex[1] == 0) menuIndex[1] = 1;
                                else menuIndex[1] -= 1;
                                buttonCharakter[menuIndex[1]].UpdatePad(1);
                                buttonDown[1, 3] = true;
                            }
                        }

                        if (GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed && !buttonDown[1, 2])
                        {
                            if (charakterMenuPosition[1] == true)
                            {
                                if (player2Index == 0)
                                {
                                    buttonDown[1, 2] = true;
                                }
                                else
                                {
                                    buttonDown[1, 2] = true;
                                }
                            }
                            else
                            {
                                if (menuIndex[1] == 0)
                                {
                                    gamestate = GameState.splashMenu;
                                    buttonDown[1, 2] = true;
                                }
                                else
                                {
                                    List<int> playerIndex = new List<int> { player1Index, player2Index, player3Index, player4Index };
                                    playerList.Clear();
                                    if (playerIndex.FindAll(index => index == 4).Count < 3)
                                    {
                                        for (int i = 0; i < playerIndex.Count; i++)
                                        {
                                            if (playerIndex[i] != 4)
                                            {
                                                playerList.Add(new Player(spawnPoints[playerList.Count], spawnRotation[playerList.Count], i, collisionManager, characterManager.getStruct(playerIndex[i]), ultimatesphere, item,dashEffect));

                                            }
                                        }

                                        collisionManager.setPlayers(playerList);

                                        gamestate = GameState.ingame;
                                        buttonDown[1, 2] = true;
                                        showError = false;

                                    }
                                    else
                                    {
                                        showError = true;
                                        buttonDown[1, 2] = true;
                                    }
                                }
                            }
                        }

                        //Play3Charakterwahl Update
                        if (GamePad.GetState(PlayerIndex.Three).DPad.Up == ButtonState.Pressed && !buttonDown[2, 0])
                        {
                            if (charakterMenuPosition[2] == true) charakterMenuPosition[2] = false;
                            else charakterMenuPosition[2] = true;
                            buttonDown[2, 0] = true;
                        }

                        if (GamePad.GetState(PlayerIndex.Three).DPad.Down == ButtonState.Pressed && !buttonDown[2, 1])
                        {
                            if (charakterMenuPosition[2] == false) charakterMenuPosition[2] = true;
                            else charakterMenuPosition[2] = false;
                            buttonCharakter[1].UpdatePad(1);
                            buttonDown[2, 1] = true;
                        }

                        if (GamePad.GetState(PlayerIndex.Three).DPad.Right == ButtonState.Pressed && !buttonDown[2, 4])
                        {
                            if (charakterMenuPosition[2] == true)
                            {
                                player3Index = (player3Index + 1) % 5;
                                buttonDown[2, 4] = true;
                            }
                            else
                            {
                                buttonCharakter[menuIndex[1]].UpdatePad(0);
                                if (menuIndex[1] == 1) menuIndex[1] = 0;
                                else menuIndex[1] += 1;
                                buttonCharakter[menuIndex[1]].UpdatePad(1);
                                buttonDown[2, 4] = true;
                            }
                        }
                        if (GamePad.GetState(PlayerIndex.Three).DPad.Left == ButtonState.Pressed && !buttonDown[2, 3])
                        {
                            if (charakterMenuPosition[2] == true)
                            {
                                player3Index = (player3Index - 1);
                                if (player3Index == -1) player3Index = 4;
                                buttonDown[2, 3] = true;
                            }
                            else
                            {
                                buttonCharakter[menuIndex[1]].UpdatePad(0);
                                if (menuIndex[1] == 0) menuIndex[1] = 1;
                                else menuIndex[1] -= 1;
                                buttonCharakter[menuIndex[1]].UpdatePad(1);
                                buttonDown[2, 3] = true;
                            }
                        }

                        if (GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed && !buttonDown[2, 2])
                        {
                            if (charakterMenuPosition[2] == true)
                            {
                                if (player3Index == 0)
                                {
                                    buttonDown[2, 2] = true;
                                }
                                else
                                {
                                    buttonDown[2, 2] = true;
                                }
                            }
                            else
                            {
                                if (menuIndex[1] == 0)
                                {
                                    gamestate = GameState.splashMenu;
                                    buttonDown[2, 2] = true;
                                }
                                else
                                {
                                    List<int> playerIndex = new List<int> { player1Index, player2Index, player3Index, player4Index };
                                    playerList.Clear();
                                    if (playerIndex.FindAll(index => index == 4).Count < 3)
                                    {
                                        for (int i = 0; i < playerIndex.Count; i++)
                                        {
                                            if (playerIndex[i] != 4)
                                            {
                                                playerList.Add(new Player(spawnPoints[playerList.Count], spawnRotation[playerList.Count], i, collisionManager, characterManager.getStruct(playerIndex[i]), ultimatesphere, item,dashEffect));

                                            }
                                        }

                                        collisionManager.setPlayers(playerList);

                                        gamestate = GameState.ingame;
                                        buttonDown[2, 2] = true;
                                        showError = false;

                                    }
                                    else
                                    {
                                        showError = true;
                                        buttonDown[2, 2] = true;
                                    }
                                }
                            }
                        }

                        //Play4Charakterwahl Update
                        if (GamePad.GetState(PlayerIndex.Four).DPad.Up == ButtonState.Pressed && !buttonDown[3, 0])
                        {
                            if (charakterMenuPosition[3] == true) charakterMenuPosition[3] = false;
                            else charakterMenuPosition[3] = true;
                            buttonDown[3, 0] = true;
                        }

                        if (GamePad.GetState(PlayerIndex.Four).DPad.Down == ButtonState.Pressed && !buttonDown[3, 1])
                        {
                            if (charakterMenuPosition[3] == false) charakterMenuPosition[3] = true;
                            else charakterMenuPosition[3] = false;
                            buttonCharakter[1].UpdatePad(1);
                            buttonDown[3, 1] = true;
                        }

                        if (GamePad.GetState(PlayerIndex.Four).DPad.Right == ButtonState.Pressed && !buttonDown[3, 4])
                        {
                            if (charakterMenuPosition[3] == true)
                            {
                                player4Index = (player4Index + 1) % 5;
                                buttonDown[3, 4] = true;
                            }
                            else
                            {
                                buttonCharakter[menuIndex[1]].UpdatePad(0);
                                if (menuIndex[1] == 1) menuIndex[1] = 0;
                                else menuIndex[1] += 1;
                                buttonCharakter[menuIndex[1]].UpdatePad(1);
                                buttonDown[3, 4] = true;
                            }
                        }
                        if (GamePad.GetState(PlayerIndex.Four).DPad.Left == ButtonState.Pressed && !buttonDown[3, 3])
                        {
                            if (charakterMenuPosition[3] == true)
                            {
                                player4Index = (player4Index - 1);
                                if (player4Index == -1) player4Index = 4;
                                buttonDown[3, 3] = true;
                            }
                            else
                            {
                                buttonCharakter[menuIndex[1]].UpdatePad(0);
                                if (menuIndex[1] == 0) menuIndex[1] = 1;
                                else menuIndex[1] -= 1;
                                buttonCharakter[menuIndex[1]].UpdatePad(1);
                                buttonDown[3, 3] = true;
                            }
                        }

                        if (GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed && !buttonDown[3, 2])
                        {
                            if (charakterMenuPosition[3] == true)
                            {
                                if (player4Index == 0)
                                {
                                    buttonDown[3, 2] = true;
                                }
                                else
                                {
                                    buttonDown[3, 2] = true;
                                }
                            }
                            else
                            {
                                if (menuIndex[1] == 0)
                                {
                                    gamestate = GameState.splashMenu;
                                    buttonDown[3, 2] = true;
                                }
                                else
                                {
                                    List<int> playerIndex = new List<int> { player1Index, player2Index, player3Index, player4Index };
                                    playerList.Clear();
                                    if (playerIndex.FindAll(index => index == 4).Count < 3)
                                    {
                                        for (int i = 0; i < playerIndex.Count; i++)
                                        {
                                            if (playerIndex[i] != 4)
                                            {
                                                playerList.Add(new Player(spawnPoints[playerList.Count], spawnRotation[playerList.Count], i, collisionManager, characterManager.getStruct(playerIndex[i]), ultimatesphere, item,dashEffect));

                                            }
                                        }

                                        collisionManager.setPlayers(playerList);

                                        gamestate = GameState.ingame;
                                        buttonDown[3, 2] = true;
                                        showError = false;

                                    }
                                    else
                                    {
                                        showError = true;
                                        buttonDown[3, 2] = true;
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        IsMouseVisible = true;

                        //Play1Charakterwahl Update
                        if (player1Back.isClicked == true && !buttonDown[0, 0])
                        {
                            player1Index = (player1Index - 1);
                            if (player1Index == -1)
                                player1Index = 4;

                            player1Back.isClicked = false;
                        }
                        player1Back.Update(mouse);

                        if (player1For.isClicked == true && !buttonDown[0, 0])
                        {
                            player1Index = (player1Index + 1) % 5;

                            player1For.isClicked = false;
                        }
                        player1For.Update(mouse);

                        //Play2Charakterwahl Update
                        if (player2Back.isClicked == true && !buttonDown[0, 0])
                        {
                            player2Index = (player2Index - 1);
                            if (player2Index == -1) player2Index = 4;

                            player2Back.isClicked = false;
                        }
                        player2Back.Update(mouse);

                        if (player2For.isClicked == true && !buttonDown[0, 0])
                        {
                            player2Index = (player2Index + 1) % 5;

                            player2For.isClicked = false;
                        }
                        player2For.Update(mouse);

                        //Play3Charakterwahl Update
                        if (player3Back.isClicked == true && !buttonDown[0, 0])
                        {
                            player3Index = (player3Index - 1);
                            if (player3Index == -1) player3Index = 4;

                            player3Back.isClicked = false;
                        }
                        player3Back.Update(mouse);

                        if (player3For.isClicked == true && !buttonDown[0, 0])
                        {
                            player3Index = (player3Index + 1) % 5;

                            player3For.isClicked = false;
                        }
                        player3For.Update(mouse);

                        //Play4Charakterwahl Update
                        if (player4Back.isClicked == true && !buttonDown[0, 0])
                        {
                            player4Index = (player4Index - 1);
                            if (player4Index == -1)
                                player4Index = 4;


                            player4Back.isClicked = false;
                        }
                        player4Back.Update(mouse);

                        if (player4For.isClicked == true && !buttonDown[0, 0])
                        {
                            player4Index = (player4Index + 1) % 5;

                            player4For.isClicked = false;
                        }
                        player4For.Update(mouse);

                        if (buttonCharakter[0].isClicked == true && !buttonDown[0, 0])
                        {
                            gamestate = GameState.splashMenu;
                            IsMouseVisible = false;
                            buttonCharakter[0].isClicked = false;
                        }
                        buttonCharakter[0].Update(mouse);

                        if (buttonCharakter[1].isClicked == true && !buttonDown[0, 0])
                        {
                            List<int> playerIndex = new List<int> { player1Index, player2Index, player3Index, player4Index };
                            playerList.Clear();
                            if (playerIndex.FindAll(index => index == 4).Count < 3)
                            {
                                for (int i = 0; i < playerIndex.Count; i++)
                                {
                                    if (playerIndex[i] != 4)
                                    {
                                        playerList.Add(new Player(spawnPoints[playerList.Count], spawnRotation[playerList.Count], i, collisionManager, characterManager.getStruct(playerIndex[i]), ultimatesphere,item,dashEffect));

                                    }
                                }

                                collisionManager.setPlayers(playerList);

                                gamestate = GameState.ingame;
                                IsMouseVisible = false;
                                buttonCharakter[1].isClicked = false;
                                showError = false;

                            }
                            else
                            {
                                showError = true;
                            }



                        }
                    }
                        
                    buttonCharakter[1].Update(mouse);

                    break;

                case GameState.howtoplay:

                    if (gamePadOn == true)
                    {
                        if ((GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed && !buttonDown[0, 3]) || (GamePad.GetState(PlayerIndex.Two).DPad.Left == ButtonState.Pressed && !buttonDown[1, 3]) || (GamePad.GetState(PlayerIndex.Three).DPad.Left == ButtonState.Pressed && !buttonDown[2, 3]) || (GamePad.GetState(PlayerIndex.Four).DPad.Left == ButtonState.Pressed && !buttonDown[3, 3]))
                        {
                            if (menuIndex[2] == 0)
                            { 
                                gamestate = GameState.splashMenu;
                            }
                            else
                            {
                                menuIndex[2] -= 1;
                            }

                            for (int i = 0; i <= 3; i++)
                            {
                                buttonDown[i, 3] = true;
                            }
                        }

                        if ((GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed && !buttonDown[0, 4]) || (GamePad.GetState(PlayerIndex.Two).DPad.Right == ButtonState.Pressed && !buttonDown[1, 4]) || (GamePad.GetState(PlayerIndex.Three).DPad.Right == ButtonState.Pressed && !buttonDown[2, 4]) || (GamePad.GetState(PlayerIndex.Four).DPad.Right == ButtonState.Pressed && !buttonDown[3, 4]))
                        {
                            if (menuIndex[2] == 4)
                            {
                                gamestate = GameState.splashMenu;
                            }
                            else
                            {
                                menuIndex[2] += 1;
                            }

                            for (int i = 0; i <= 3; i++)
                            {
                                buttonDown[i, 4] = true;
                            }
                        }
                    }
                    else
                    {
                        IsMouseVisible = true;

                        if (howtoplay[0].isClicked == true && !buttonDown[0, 0])
                        {
                            if (menuIndex[2] == 0)
                            {
                                gamestate = GameState.splashMenu;
                                howtoplay[0].isClicked = false;
                                IsMouseVisible = false;
                            }
                            else
                            {
                                menuIndex[2] -= 1;
                                howtoplay[0].isClicked = false;
                            }

                        }
                        howtoplay[0].Update(mouse);

                        if (howtoplay[1].isClicked == true && !buttonDown[0, 0])
                        {
                            if (menuIndex[2] == 4)
                            {
                                gamestate = GameState.splashMenu;
                                menuIndex[2] = 0;
                                howtoplay[1].isClicked = false;
                                IsMouseVisible = false;
                            }
                            else
                            {
                                menuIndex[2] += 1;
                                howtoplay[1].isClicked = false;
                            }

                        }
                        howtoplay[1].Update(mouse);
                    }

                    break;

                case GameState.ingame:
                    if (!mute)
                    {
                        MediaPlayer.Volume = 0.5f;
                    }
                    collisionManager.Update(mute);
                    for (int i = 0; i < playerList.Count; i++)
                    {
                        playerList[i].Update(gameTime,mute);
                    }

                    item.update(gameTime, collectItem,mute);
                    for(int i = 0;i<playerList.Count;i++)
                    {
                        if (playerList[i].getCollisionSpheres()[0].getSphere().Intersects(waterBounding))
                        {
                            if (!mute)
                            {
                                bubble.Play();
                            }
                        }
                    }


                    if (gamePadOn == true)
                    {
                        if ((GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed && !buttonDown[0, 5]) || (GamePad.GetState(PlayerIndex.Two).Buttons.Start == ButtonState.Pressed && !buttonDown[1, 5]) || (GamePad.GetState(PlayerIndex.Three).Buttons.Start == ButtonState.Pressed && !buttonDown[2, 5]) || (GamePad.GetState(PlayerIndex.Four).Buttons.Start == ButtonState.Pressed && !buttonDown[3, 5]))
                        {
                            gamestate = GameState.pause;

                            for (int i = 0; i <= 3; i++)
                            {
                                buttonDown[i, 2] = true;
                            }
                        }
                    }
                    else
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.P) && !buttonDown[0, 0]) 
                            gamestate = GameState.pause;
                    }

                    if (collisionManager.checkPlayerAlive() == 1)
                    {
                        item.resetItem();
                        gamestate = GameState.result;
                    }

                    break;

                case GameState.pause:
                    if (mute)
                    {
                        MediaPlayer.Volume = 0;
                    }
                    else
                    {
                        MediaPlayer.Volume = 1;
                    }
                    if (gamePadOn == true)
                    {
                        buttonPause[menuIndex[3]].UpdatePad(1);

                        if ((GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && !buttonDown[0, 0]) || (GamePad.GetState(PlayerIndex.Two).DPad.Up == ButtonState.Pressed && !buttonDown[1, 0]) || (GamePad.GetState(PlayerIndex.Three).DPad.Up == ButtonState.Pressed && !buttonDown[2, 0]) || (GamePad.GetState(PlayerIndex.Four).DPad.Up == ButtonState.Pressed && !buttonDown[3, 0]))
                        {
                            if (menuIndex[3] == 0)
                            {
                                buttonPause[menuIndex[3]].UpdatePad(0);
                                menuIndex[3] = 1;
                                buttonPause[menuIndex[3]].UpdatePad(1);
                            }
                            else
                            {
                                buttonPause[menuIndex[3]].UpdatePad(0);
                                menuIndex[3] -= 1;
                                buttonPause[menuIndex[3]].UpdatePad(1);
                            }

                            for (int i = 0; i <= 3; i++)
                            {
                                buttonDown[i, 0] = true;
                            }
                        }

                        if ((GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed && !buttonDown[0, 1]) || (GamePad.GetState(PlayerIndex.Two).DPad.Down == ButtonState.Pressed && !buttonDown[1, 1]) || (GamePad.GetState(PlayerIndex.Three).DPad.Down == ButtonState.Pressed && !buttonDown[2, 1]) || (GamePad.GetState(PlayerIndex.Four).DPad.Down == ButtonState.Pressed && !buttonDown[3, 1]))
                        {
                            if (menuIndex[3] == 1)
                            {
                                buttonPause[menuIndex[3]].UpdatePad(0);
                                menuIndex[3] = 0;
                                buttonPause[menuIndex[3]].UpdatePad(1);
                            }
                            else
                            {
                                buttonPause[menuIndex[3]].UpdatePad(0);
                                menuIndex[3] += 1;
                                buttonPause[menuIndex[3]].UpdatePad(1);
                            }

                            for (int i = 0; i <= 3; i++)
                            {
                                buttonDown[i, 1] = true;
                            }
                        }

                        if ((GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && !buttonDown[0, 2]) || (GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed && !buttonDown[1, 2]) || (GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed && !buttonDown[2, 2]) || (GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed && !buttonDown[3, 2]))
                        {
                            if (menuIndex[3] == 0) gamestate = GameState.ingame;
                            if (menuIndex[3] == 1) gamestate = GameState.splashMenu;
                            
                            for (int i = 0; i <= 3; i++)
                            {
                                buttonDown[i, 2] = true;
                            }
                        } 
                    }
                    else
                    {
                        IsMouseVisible = true;

                        if (buttonPause[0].isClicked == true && !buttonDown[0, 0])
                        {
                            gamestate = GameState.ingame;
                            IsMouseVisible = false;
                            buttonPause[0].isClicked = false;
                        }

                        if (buttonPause[1].isClicked == true && !buttonDown[0, 0])
                        {
                            gamestate = GameState.splashMenu;
                            buttonPause[1].isClicked = false;

                        }
                        buttonPause[1].Update(mouse);
                        buttonPause[0].Update(mouse);
                    }

                    break;

                case GameState.result:
                    if (mute)
                    {
                        MediaPlayer.Volume = 0;
                    }
                    else
                    {
                        MediaPlayer.Volume = 1;
                    }

                    resultIndex = collisionManager.winner();

                    if (gamePadOn == true)
                    {
                        buttonResult[menuIndex[4]].UpdatePad(1);

                        if ((GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && !buttonDown[0, 0]) || (GamePad.GetState(PlayerIndex.Two).DPad.Up == ButtonState.Pressed && !buttonDown[1, 0]) || (GamePad.GetState(PlayerIndex.Three).DPad.Up == ButtonState.Pressed && !buttonDown[2, 0]) || (GamePad.GetState(PlayerIndex.Four).DPad.Up == ButtonState.Pressed && !buttonDown[3, 0]))
                        {
                            if (menuIndex[4] == 0)
                            {
                                buttonResult[menuIndex[4]].UpdatePad(0);
                                menuIndex[4] = 1;
                                buttonResult[menuIndex[4]].UpdatePad(1);
                            }
                            else
                            {
                                buttonResult[menuIndex[4]].UpdatePad(0);
                                menuIndex[4] -= 1;
                                buttonResult[menuIndex[4]].UpdatePad(1);
                            }

                            for (int i = 0; i <= 3; i++)
                            {
                                buttonDown[i, 0] = true;
                            }
                        }

                        if ((GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed && !buttonDown[0, 1]) || (GamePad.GetState(PlayerIndex.Two).DPad.Down == ButtonState.Pressed && !buttonDown[1, 1]) || (GamePad.GetState(PlayerIndex.Three).DPad.Down == ButtonState.Pressed && !buttonDown[2, 1]) || (GamePad.GetState(PlayerIndex.Four).DPad.Down == ButtonState.Pressed && !buttonDown[3, 1]))
                        {
                            if (menuIndex[4] == 1)
                            {
                                buttonResult[menuIndex[4]].UpdatePad(0);
                                menuIndex[4] = 0;
                                buttonResult[menuIndex[4]].UpdatePad(1);
                            }
                            else
                            {
                                buttonResult[menuIndex[4]].UpdatePad(0);
                                menuIndex[4] += 1;
                                buttonResult[menuIndex[4]].UpdatePad(1);
                            }

                            for (int i = 0; i <= 3; i++)
                            {
                                buttonDown[i, 1] = true;
                            }
                        }

                        if ((GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && !buttonDown[0, 2]) || (GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed && !buttonDown[1, 2]) || (GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed && !buttonDown[2, 2]) || (GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed && !buttonDown[3, 2]))
                        {
                            if (menuIndex[4] == 0)
                            {
                                playerList.Clear();
                                gamestate = GameState.character;
                            }
                            if (menuIndex[4] == 1) gamestate = GameState.splashMenu;

                            for (int i = 0; i <= 3; i++)
                            {
                                buttonDown[i, 2] = true;
                            }
                        } 
                    }
                    else
                    {
                        
                        IsMouseVisible = true;

                        if (buttonResult[1].isClicked == true && !buttonDown[0, 0])
                        {
                            gamestate = GameState.splashMenu;
                            IsMouseVisible = false;
                            buttonResult[1].isClicked = false;
                        }



                        if (buttonResult[0].isClicked == true && !buttonDown[0, 0])
                        {
                            playerList.Clear();
                            gamestate = GameState.character;
                            IsMouseVisible = false;
                            buttonResult[0].isClicked = false;
                        }
                        buttonResult[1].Update(mouse);
                        buttonResult[0].Update(mouse);
                    }

                    break;

                case GameState.credits :
                    if (mute)
                    {
                        MediaPlayer.Volume = 0;
                    }
                    else
                    {
                        MediaPlayer.Volume = 1;
                    }

                    if (gameTime.TotalGameTime.Seconds > 5)
                    {
                        if (creditsAnimation.A == 255) creditsStatus = false;
                        if (creditsAnimation.A == 0) creditsStatus = true;
                        if (creditsStatus)
                        {
                            this.Exit();
                        }
                        else
                        {
                            creditsAnimation.A -= 3;
                            creditsAnimation.R -= 3;
                            creditsAnimation.G -= 3;
                            creditsAnimation.B -= 3;
                        }
                    }

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
                    spriteBatch.Draw(logoklein, new Rectangle(590, 180, logoklein.Width, logoklein.Height), Color.White);

                    splashMenuButtons[0].Draw(spriteBatch);
                    splashMenuButtons[1].Draw(spriteBatch);
                    splashMenuButtons[2].Draw(spriteBatch);
                    splashMenuButtons[3].Draw(spriteBatch);

                   
                    
		    if (drawX)
                    {
                        spriteBatch.Draw(lautsprecherX, new Rectangle(565, 555, lautsprecherX.Width, lautsprecherX.Height), Color.White);

                    }
                  
                    

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

                    buttonCharakter[0].Draw(spriteBatch);
                    buttonCharakter[1].Draw(spriteBatch);

                    if (showError)
                    {
                        spriteBatch.DrawString(font, "Please select at least two Players!", new Vector2(440, 670), Color.Red);
                    }

                    break;

                case GameState.howtoplay:

                    spriteBatch.Draw(howtoplayscreen[menuIndex[2]], new Rectangle(0, 0, howtoplayscreen[menuIndex[2]].Width, howtoplayscreen[menuIndex[2]].Height), Color.White);
                    howtoplay[0].Draw(spriteBatch);
                    howtoplay[1].Draw(spriteBatch);

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
                    for (int i = 0; i < playerList.Count; i++)
                    {
                        Matrix world2 = Matrix.Identity * Matrix.CreateTranslation(new Vector3(playerList[i].getPosition().X-0.6f, playerList[i].getPosition().Y+2, playerList[i].getPosition().Z));
                        foreach (ModelMesh mesh in identifier[playerList[i].getPlayerIndex()].Meshes)
                        {
                            foreach (BasicEffect basic in mesh.Effects)
                            {
                                basic.World = world2;
                                basic.View = view;
                                basic.Projection = projection;
                                basic.EnableDefaultLighting();
                            }
                            mesh.Draw();
                        }
                    }
                    Matrix world3 = Matrix.Identity*Matrix.CreateTranslation(new Vector3(-5,-1,0));
                    foreach (ModelMesh mesh in ground.Meshes)
                    {
                        foreach (BasicEffect basic in mesh.Effects)
                        {
                            basic.World = world3;
                            basic.View = view;
                            basic.Projection = projection;
                            basic.EnableDefaultLighting();
                        }
                        mesh.Draw();
                    }

                    for (int i = 0; i < playerList.Count; i++)
                    {
                        playerList[i].Draw(view, projection);
                    }
                   

                    item.draw(view, projection);


                    //spriteBatch.DrawString(font, "canFall " + collisionManager.canFall(playerList[0]).ToString(), new Vector2(100, 100), Color.Black);
                    //spriteBatch.DrawString(font, "isAlive " + playerList[0].isAlive.ToString(), new Vector2(100, 150), Color.Black);
                    
                    //spriteBatch.DrawString(font, "canMove " + playerList[0].canMove.ToString(), new Vector2(100, 200), Color.Black);
                    //spriteBatch.DrawString(font, "sphere" + playerList[0].getCollisionSpheres()[0].getCenterPos().ToString(), new Vector2(100, 250), Color.Black);
                    //spriteBatch.DrawString(font, "effectTime" + (item.effectTime + item.activationTime).ToString(), new Vector2(100, 300), Color.Black);
                    //spriteBatch.DrawString(font, "effectTime" + ((item.effectTime + item.activationTime)>=gameTime.TotalGameTime).ToString(), new Vector2(100, 350), Color.Black);
                    //spriteBatch.DrawString(font, "effectTime" + gameTime.TotalGameTime.ToString(), new Vector2(100, 400), Color.Black);
                    //spriteBatch.DrawString(font, "enemyMass" + collisionManager.test.ToString(), new Vector2(100, 450), Color.Black);
                    //spriteBatch.DrawString(font, "enemyCount" + collisionManager.test.ToString(), new Vector2(100,500), Color.Black);
                   
                    int y = 100;
                    spriteBatch.DrawString(font, "DashCountdown", new Vector2(20,y), Color.White);
                    y += 20;
                    for(int i = 0;i<playerList.Count;i++)
                    {
                        y += 20;
                        int dashCountdown = playerList[i].getRestDashTime(gameTime).Seconds;
                        if (dashCountdown <= 0)
                        {
                            spriteBatch.DrawString(font, "Player " + (playerList[i].getPlayerIndex() + 1).ToString() + " Bereit!", new Vector2(20, y), Color.White);
                        }

                        else
                        {
                            spriteBatch.DrawString(font, "Player " + (playerList[i].getPlayerIndex() + 1).ToString() + " " + dashCountdown.ToString(), new Vector2(20, y), Color.White);
                        }
                    }
                    if (item.getPickedUp())
                    {
                        List<String> affectedPlayer = item.getAffectedPlayer(playerList);
                        int v = 100;
                        spriteBatch.DrawString(font, item.getRestTime(gameTime).ToString(), new Vector2(1110, v), Color.White);
                        v += 40;
                        for (int i = 0; i < affectedPlayer.Count; i++)
                        {
                            v += 30;
                            spriteBatch.DrawString(font, "Player " + affectedPlayer[i].ToString(), new Vector2(1100, v), Color.White);
                            
                        }
                            
                       
                    }


                    base.Draw(gameTime);

                break;

            case GameState.pause:
                
                spriteBatch.Draw(pause, new Rectangle(0, 0, pause.Width, pause.Height), Color.White);
                spriteBatch.Draw(buttonBackgroundPause, new Rectangle(480, 150, buttonBackgroundPause.Width, buttonBackgroundPause.Height), Color.White);
                buttonPause[0].Draw(spriteBatch);
                buttonPause[1].Draw(spriteBatch);
                
                break;

            case GameState.result:

                spriteBatch.Draw(results[resultIndex], new Rectangle(0, 0, results[resultIndex].Width, results[resultIndex].Height), Color.White);
                spriteBatch.DrawString(font2,  "Player "+ (collisionManager.winnerIndex()+1).ToString(), new Vector2(10, 350), Color.White);
                buttonResult[1].Draw(spriteBatch);
                buttonResult[0].Draw(spriteBatch);

                break;

             case GameState.credits :

                spriteBatch.Draw(creditsPicture, new Rectangle(0, 0, creditsPicture.Width, creditsPicture.Height), creditsAnimation);

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
