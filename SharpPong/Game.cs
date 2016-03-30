using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace SharpPong
{
    class Game
    {
        int level;
        public Player playerL, playerR;
        Tiles tiles;
        bool loose;
        int seconds;
        Text counting;

        // Ball and paddles
        Ball ball;
        public RectangleShape paddleL, paddleR, paddle;
        float paddleSpeed = 300f;
        
        // Time
        Clock clock, timer;
        float deltaTime;
        float delay = 0;              
//----------------------------------------------------------------------------------------------------------------------------------------------------
        public Game(int level)
        {
            this.level = level;
            this.clock = new Clock();
            this.timer = new Clock();
            this.ball = new Ball(14, 300); //  temporarily
            this.playerL = new Player("PlayerA", 0);
            this.playerR = new Player("PlayerB", 0);
            this.loose = false;

            // Left paddle
            Texture paddleTexture = new Texture("textures/paddle2.png");
            paddleTexture.Smooth = true;
            this.paddleL = new RectangleShape(new Vector2f(20, 120));
            paddleL.Texture = paddleTexture;
            paddleL.Position = new Vector2f(10, Settings.HEIGHT / 2 - paddleL.Size.Y / 2);

            // Right paddle
            this.paddleR = new RectangleShape(new Vector2f(20, 120));
            paddleR.Texture = paddleTexture;
            paddleR.Position = new Vector2f(Settings.WIDTH - paddleR.Size.X - 10, Settings.HEIGHT / 2 - paddleR.Size.Y / 2);

            // Paddle (Arkanoid)
            this.paddle = new RectangleShape(new Vector2f(120, 20));
            paddle.Texture = paddleTexture;
            paddle.Position = new Vector2f(Settings.WIDTH / 2 - paddle.Size.X , Settings.HEIGHT - paddle.Size.Y - 10);

            // Counting
            this.counting = new Text("", new Font("robotastic.ttf"));
            counting.Position = new Vector2f(Settings.WIDTH / 2 - 15, Settings.HEIGHT / 2 - 50);
            counting.CharacterSize = 50;
            counting.Color = new Color(255, 255, 255, 170);

            // Reading tiles (Arkanoid)
            this.tiles = new Tiles(97, 35);
            tiles.readTiles();
        }
//----------------------------------------------------------------------------------------------------------------------------------------------------
        // Increase game level after one or two minutes playing
        public void LevelUp()
        {
            level++;
            ball.speed += 10;           
        }
//----------------------------------------------------------------------------------------------------------------------------------------------------       
        // Handling paddles and ball movement
        public void movePong()
        {
            Random rnd = new Random();
            
            // Moving player's paddle
            if (Keyboard.IsKeyPressed(Keyboard.Key.Up) && paddleL.Position.Y > 5f)
            {
                paddleL.Position += new Vector2f(0f, -paddleSpeed * deltaTime);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Down) && paddleL.Position.Y < Settings.HEIGHT - (paddleL.Size.Y + 5))
            {
                paddleL.Position += new Vector2f(0f, paddleSpeed * deltaTime);
            }

            // Delay computer's paddle (after 3 seconds) 
            if (getTime() % 2 == 0)
                delay = rnd.Next(270);

            // Moving computer's paddle
            if (ball.ballShape.Position.Y < paddleR.Position.Y + paddleR.Size.Y/2 && paddleR.Position.Y > 5f)
                paddleR.Position += new Vector2f(0f, (paddleSpeed - delay) * deltaTime * (-1));
            else if(ball.ballShape.Position.Y > paddleR.Position.Y + paddleR.Size.Y / 2 && paddleR.Position.Y < Settings.HEIGHT - (paddleR.Size.Y + 5))
                paddleR.Position += new Vector2f(0f, (paddleSpeed - delay) * deltaTime * 1);

            int winner = ball.movingPong(deltaTime, paddleL, paddleR);

            // Gaining point
            if (winner == -1)
                playerL.score += 1;
            else if(winner == 1)
                playerR.score += 1;               
        }
//----------------------------------------------------------------------------------------------------------------------------------------------------
        public void moveArkanoid()
        {
            // Moving player's paddle
            if (Keyboard.IsKeyPressed(Keyboard.Key.Left) && paddle.Position.X > 5f)
                paddle.Position += new Vector2f(-paddleSpeed * deltaTime, 0f );
            
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right) && paddle.Position.X < Settings.WIDTH - (paddle.Size.X + 5))            
                paddle.Position += new Vector2f(paddleSpeed * deltaTime, 0f);
            
            // Moving the ball
            loose = ball.movingArkanoid(deltaTime , paddle, tiles);

            seconds = DateTime.Now.Second;

        }
//----------------------------------------------------------------------------------------------------------------------------------------------------
        // Running game
        public void run(RenderWindow window, Text time, Text score, RectangleShape background, int type)
        {       
            // Game loop
            while (window.IsOpen)
            {
                deltaTime = clock.Restart().AsSeconds();

                // Process events
                window.DispatchEvents();

                window.Draw(background);
                //**********************************************************
                // Ping Pong
                if (type == 1)
                {
                    // Move paddles and ball
                    movePong();

                    // Increase the level after 15 seconds 
                    if (getTime() % 15 == 0)
                        LevelUp();
                  
                    // Display paddles
                    window.Draw(paddleL);
                    window.Draw(paddleR);
                    // Display player's score
                    score.DisplayedString = playerL.score.ToString() + " : " + playerR.score.ToString();
                    window.Draw(score);
                }
                //**********************************************************
                // Arkanoid
                else if (type == 2)
                {
                    
                    // Move player paddle and ball
                    if (!loose)
                        moveArkanoid();
                    // Time delaying when player loose  
                    else
                    {
                        if (DateTime.Now.Second - seconds == 1)
                        {
                            counting.DisplayedString = "3";
                            window.Draw(counting);
                        }
                        else if (DateTime.Now.Second - seconds == 2)
                        {
                            counting.DisplayedString = "2";
                            window.Draw(counting);
                        }
                        else if (DateTime.Now.Second - seconds == 3)
                        {
                            counting.DisplayedString = "1";
                            window.Draw(counting);
                        }
                        else if (DateTime.Now.Second - seconds == 4)
                        {
                            counting.DisplayedString = "0";
                            window.Draw(counting);
                            loose = false;
                        }
                    }                       

                    // Display paddle
                    window.Draw(paddle);
                    
                    // Display tiles
                    for (int x = 0; x < tiles.xTab; x++)
                       for (int y = 0; y < tiles.yTab; y++)
                           if (tiles.tileMap[x, y] == 49 || tiles.tileMap[x, y] == 50)
                                window.Draw(tiles.tiles[x, y]);

                }
                //**********************************************************
                                                       
                // Display everything on screen                                                            
                time.DisplayedString = getTime().ToString();
                window.Draw(time);
                CircleShape ballObject = getBall();
                window.Draw(ballObject);

                // Update the window
                window.Display();

            }
        }
//----------------------------------------------------------------------------------------------------------------------------------------------------
        // Returning game time 
        public double getTime()
        {
            Time temp = timer.ElapsedTime;
            double time = Math.Round(temp.AsSeconds(), 2);
             
            return time;
        }
//----------------------------------------------------------------------------------------------------------------------------------------------------
        // Retutning ball
        public CircleShape getBall()
        {
            return ball.ballShape;
        }
//----------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
