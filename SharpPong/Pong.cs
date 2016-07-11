using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace SharpPong
{
    class Pong : Game
    {
        private int level;
        private float delay = 0;
        protected RectangleShape paddle, paddleOp, paddleL, paddleR;
        protected string[] keysPlayer;
        //----------------------------------------------------------------------------------------------
        public Pong(RenderWindow rw) : base(rw)
        {

            this.level = 1;

            // Left paddle
            Texture paddleTexture = ResourceManager.GetTexture("resources/textures/paddle2.png");
            paddleTexture.Smooth = true;
            this.paddleL = new RectangleShape(new Vector2f(20, 120));
            paddleL.Texture = paddleTexture;
            paddleL.Position = new Vector2f(10, Settings.HEIGHT / 2 - paddleL.Size.Y / 2);

            // Right paddle
            this.paddleR = new RectangleShape(new Vector2f(20, 120));
            paddleR.Texture = paddleTexture;
            paddleR.Position = new Vector2f(Settings.WIDTH - paddleR.Size.X - 10, Settings.HEIGHT / 2 - paddleR.Size.Y / 2);

            // Player
            this.paddle = paddleL;
            this.keysPlayer = new string[2] { "Up", "Down" };
            this.playerL.setPlayersKeys(keysPlayer);

        }
        //----------------------------------------------------------------------------------------------
        // Increase game level
        private void LevelUp()
        {

            level++;
            ball.speed += 5;

        } 
        //----------------------------------------------------------------------------------------------
        protected override void Move()
        {
            int winner = 0;

            if (window.HasFocus())
            {
                // Moving player's paddle.
                MovePlayer();
            }
                
            // Moving opponent's paddle.
            MoveOpponent();            
            
            // Moving the ball.
            if (running)
                winner = ball.movingPong(deltaTime, paddleL, paddleR);

            // Gaining point by player or by opponent.
            if (winner == -1)
                playerL.score += 1;
            else if (winner == 1)
                playerR.score += 1;

            if (winner != 0)
            {

                paddleL.Position = new Vector2f(10, Settings.HEIGHT / 2 - paddleL.Size.Y / 2);
                paddleR.Position = new Vector2f(Settings.WIDTH - paddleR.Size.X - 10, Settings.HEIGHT / 2 - paddleR.Size.Y / 2);
                running = false;

            }

            // TODO: Loosing game
  
            // Increase the level after 15 seconds. 
            if (GetTime() % 15 == 0 && running)
               LevelUp();

        }
        //----------------------------------------------------------------------------------------------
        protected void MovePlayer()
        {
            
            // Moving player's paddle.
            if (Keyboard.IsKeyPressed((Keyboard.Key)playerL.keys[0]) && paddle.Position.Y > 5f)
            {
                paddle.Position += new Vector2f(0f, -paddleSpeed * deltaTime);
            }
            if (Keyboard.IsKeyPressed((Keyboard.Key)playerL.keys[1]) && paddle.Position.Y < Settings.HEIGHT - (paddle.Size.Y + 5))
            {
                paddle.Position += new Vector2f(0f, paddleSpeed * deltaTime);
            }

        }
        //----------------------------------------------------------------------------------------------
        protected virtual void MoveOpponent()
        {
    
            Random rnd = new Random();

            // Delay computer's paddle (after 2 seconds). 
            if (GetTime() % 2 == 0)
                delay = rnd.Next(270);

            // Moving computer's paddle.
            if (ball.ballShape.Position.Y < paddleR.Position.Y + paddleR.Size.Y / 2 && paddleR.Position.Y > 5f)
            {
                paddleR.Position += new Vector2f(0f, (paddleSpeed - delay) * deltaTime * (-1));
            }
            else if (ball.ballShape.Position.Y > paddleR.Position.Y + paddleR.Size.Y / 2 && paddleR.Position.Y < Settings.HEIGHT - (paddleR.Size.Y + 5))
            {
                paddleR.Position += new Vector2f(0f, (paddleSpeed - delay) * deltaTime * 1);
            }

        }
        //----------------------------------------------------------------------------------------------
        protected override void PostRender()
        {
            // Display players paddles.
            window.Draw(paddleL);
            window.Draw(paddleR);

            // Display time.    
            if (running || !gameOn)
                time.DisplayedString = GetTime().ToString();

            window.Draw(time);

            // Display players score.
            score.DisplayedString = playerL.score.ToString() + " : " + playerR.score.ToString();
            window.Draw(score);

        }
        //----------------------------------------------------------------------------------------------
    }
}
