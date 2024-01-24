using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Final_Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        List<Rectangle> laserwallRects;
        float gravity;
        float dx, dy;

        SpriteFont spaceFont;
        Texture2D rect;
        //Player
        Texture2D playerShip, screenFollow;
        Texture2D baseFlight1, baseFlight2, baseFlight3, baseFlight4;
        Texture2D jumpFlight1, jumpFlight2, jumpFlight3, jumpFlight4;
        KeyboardState currentstate, previousState;
        Rectangle playerShipRect, screenFollowRect;
        Vector2 playerShipSpeed, screenFollowSpeed;
        private Camera _camera;


        //Obstacles
        Texture2D laserWall, laserLight, meteor1, meteor2, meteor3;
        Rectangle meteor1Rect, meteor2Rect, meteor3Rect;
        Vector2 meteor1Speed, meteor2Speed, meteor3Speed;
        

        //Death
        Texture2D animExplode1, animExplode2, animExplode3, animExplode4, animExplode5, animExplode6;
        Texture2D animExplode7, animExplode8, animExplode9, animExplode10, animExplode11;
        bool crash;

        //Menu
        Texture2D startMenuBG, gameMenuBG, endMenuBG;
        Rectangle rectStartMenuBG, rectGameMenuBG, rectEndMenuBG;

        Screen screen;
        enum Screen
        {
            startMenu,
            gameMenu,
            endMenu,
        }
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() // TODO: Add your initialization logic here
        {

            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();
            _camera = new Camera();
            gravity = 0.1f;
            dx = 0;
            dy = 0;

            //Player
            playerShipRect = new Rectangle(180, 220, 90, 65);
            screenFollowRect = new Rectangle(400, 300, 0, 0);
            playerShipSpeed = new Vector2(2, 0);
            screenFollowSpeed = new Vector2(2, 0);



            //Obstacles
            laserwallRects = new List<Rectangle>() { };
            Random random = new Random();
            for (int i = 1; i < 50; i++)
            {
                laserwallRects.Add(new Rectangle(650 * i, random.Next(-120,70), 75, 270));
                laserwallRects.Add(new Rectangle(570 * i, random.Next(370, 450), 75, 270));
            }
            meteor1Rect = new Rectangle(100, 100, 100, 100);
            meteor2Rect = new Rectangle(100, 100, 100, 100);
            meteor3Rect = new Rectangle(100, 100, 100, 100);
            meteor1Speed = new Vector2(3, 0);
            meteor2Speed = new Vector2(1, 0);
            meteor3Speed = new Vector2(2, 0);

            //Menu
            rectStartMenuBG = new(0, 0, 800, 600);
            rectGameMenuBG = new(0, 0, 800, 600);
            rectEndMenuBG = new(0, 0, 800, 600);






            base.Initialize();
        }

        protected override void LoadContent() // TODO: use this.Content to load your game content here
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            spaceFont = Content.Load<SpriteFont>("Font");
            //Player

            playerShip = Content.Load<Texture2D>("Player/Ship2FixedHitBox");
            _camera = new Camera();

            //PlayerFlight
            baseFlight1 = Content.Load<Texture2D>("Player/Ship2_normal_flight_001");
            baseFlight2 = Content.Load<Texture2D>("Player/Ship2_normal_flight_003");
            baseFlight3 = Content.Load<Texture2D>("Player/Ship2_normal_flight_005");
            baseFlight4 = Content.Load<Texture2D>("Player/Ship2_normal_flight_007");
            jumpFlight1 = Content.Load<Texture2D>("Player/Ship2_turbo_flight_001");
            jumpFlight2 = Content.Load<Texture2D>("Player/Ship2_turbo_flight_003");
            jumpFlight3 = Content.Load<Texture2D>("Player/Ship2_turbo_flight_005");
            jumpFlight4 = Content.Load<Texture2D>("Player/Ship2_turbo_flight_007");

            //Obstacles
            laserWall = Content.Load<Texture2D>("Obstacles/Bomb_01");
            laserLight = Content.Load<Texture2D>("Obstacles/Bomb_Light");
            meteor1 = Content.Load<Texture2D>("Obstacles/Meteor_01");
            meteor2 = Content.Load<Texture2D>("Obstacles/Meteor_02");
            meteor3 = Content.Load<Texture2D>("Obstacles/Crystal_03");

            //Death
            animExplode1 = Content.Load<Texture2D>("Death/Explosion1_1");
            animExplode2 = Content.Load<Texture2D>("Death/Explosion1_2");
            animExplode3 = Content.Load<Texture2D>("Death/Explosion1_3");
            animExplode4 = Content.Load<Texture2D>("Death/Explosion1_4");
            animExplode5 = Content.Load<Texture2D>("Death/Explosion1_5");
            animExplode6 = Content.Load<Texture2D>("Death/Explosion1_6");
            animExplode7 = Content.Load<Texture2D>("Death/Explosion1_7");
            animExplode8 = Content.Load<Texture2D>("Death/Explosion1_8");
            animExplode9 = Content.Load<Texture2D>("Death/Explosion1_9");
            animExplode10 = Content.Load<Texture2D>("Death/Explosion1_10");
            animExplode11 = Content.Load<Texture2D>("Death/Explosion1_11");

            //Menu
            startMenuBG = Content.Load<Texture2D>("SpaceScreen");
            gameMenuBG = Content.Load<Texture2D>("SpaceScreen");
            endMenuBG = Content.Load<Texture2D>("SpaceScreen");
            rect = Content.Load<Texture2D>("rectangle");


        }

        protected override void Update(GameTime gameTime) // TODO: Add your update logic here
        {
            previousState = currentstate;
            currentstate = Keyboard.GetState();
            _camera.Follow(screenFollowRect);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || currentstate.IsKeyDown(Keys.Escape))
                Exit();



            if (screen == Screen.startMenu)
            {
                if (currentstate.IsKeyDown(Keys.Space))
                    screen = Screen.gameMenu;
            }
            else if (screen == Screen.gameMenu)
            {
                _camera.Follow(screenFollowRect);
                if (currentstate.IsKeyDown(Keys.Space) && previousState.IsKeyUp(Keys.Space))
                {
                    dy = -5;
                }
                if (!crash)
                {
                    dy += gravity;
                    playerShipRect.X += (int)dx;
                    playerShipRect.Y += (int)dy;
                    playerShipRect.X += (int)screenFollowSpeed.X;
                    playerShipRect.Y += (int)screenFollowSpeed.Y;

                    screenFollowRect.X += (int)screenFollowSpeed.X;
                    screenFollowRect.Y += (int)screenFollowSpeed.Y;
                }

                for (int i = 0; i < laserwallRects.Count; i++)
                {
                    if (playerShipRect.Intersects(laserwallRects[i]))
                    {
                        crash = true;
                    }
                }
                if (playerShipRect.Bottom >= _graphics.PreferredBackBufferHeight || playerShipRect.Top < 0)
                {
                    dy = 0;
                    playerShipSpeed.Y *= 0;
                    playerShipSpeed.X *= 0;
                }

            }
            else if (screen == Screen.endMenu)
            {

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) // TODO: Add your drawing code here
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();

            if (screen == Screen.startMenu)
            {
                _spriteBatch.Draw(startMenuBG, rectStartMenuBG, Color.White);
                _spriteBatch.DrawString(spaceFont, "Welcome To Your Space Flight Training!", new Vector2(210, 225), Color.Yellow);
                _spriteBatch.DrawString(spaceFont, "Press Space To Start The Simulation", new Vector2(230, 270), Color.Yellow);
            }
            else if (screen == Screen.gameMenu)
            {
                
                _spriteBatch.Draw(gameMenuBG, rectGameMenuBG, Color.White);
                _spriteBatch.End();
                _spriteBatch.Begin(transformMatrix: _camera.Transform);
                //_spriteBatch.Draw(rect, playerShipRect, Color.White);
                if (!crash)
                    _spriteBatch.Draw(playerShip, playerShipRect, Color.White);
                else
                    _spriteBatch.Draw(animExplode5, playerShipRect, Color.White);
                foreach (Rectangle r in laserwallRects)
                {
                    _spriteBatch.Draw(laserWall, r, Color.White);
                }
                
            }
            else if (screen == Screen.endMenu)
            {

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}