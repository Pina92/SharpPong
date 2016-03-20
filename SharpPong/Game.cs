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
        Ball ball;
        public RectangleShape paddleL, paddleR;
        public Player playerL, playerR;
        Clock clock, timer;
        float deltaTime;
        float delay = 0;
        //----------------------------------------------------------------------------------------------------------------------------------------------------
        public Game(int level)
        {
            this.level = level;
            this.clock = new Clock();
            this.timer = new Clock();
            this.ball = new Ball(14, 200); //  temporarily
            this.playerL = new Player("PlayerA", 0);
            this.playerR = new Player("PlayerB", 0);

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
        public void move()
        {
            float paddleSpeed = 300f;
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

            int winner = ball.moving(deltaTime, paddleL, paddleR);

            // Gaining point
            if (winner == -1)
                playerL.score += 1;
            else if(winner == 1)
                playerR.score += 1;               
        }
//----------------------------------------------------------------------------------------------------------------------------------------------------
        // Running game
        public void run(RenderWindow window, Text time, Text score, RectangleShape background)
        {
            // Game loop
            while (window.IsOpen)
            {
                deltaTime = clock.Restart().AsSeconds();

                // Process events
                window.DispatchEvents();

                // Move paddles and ball
                move();

                // Increase the level after 15 seconds 
                if (getTime() % 15 == 0)
                    LevelUp();           

                // Display everything on screen
                time.DisplayedString = getTime().ToString();
                score.DisplayedString = playerL.score.ToString() + " : " + playerR.score.ToString();
                CircleShape ballObject = getBall();

                window.Draw(background);
                window.Draw(paddleL);
                window.Draw(paddleR);
                window.Draw(ballObject);
                window.Draw(time);
                window.Draw(score);

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
