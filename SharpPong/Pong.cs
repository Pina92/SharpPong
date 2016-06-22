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
        public RectangleShape paddle, paddleOp, paddleL, paddleR;
        public string[] keysPlayer;
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
        public void LevelUp()
        {

            level++;
            ball.speed += 5;

        } 
        //----------------------------------------------------------------------------------------------
        public override void move()
        {
            int winner = 0;           

            // Moving player's paddle
            movePlayer();
            
            // Moving opponent's paddle
            moveOpponent();
            
            // Moving the ball
            if (running)
                winner = ball.movingPong(deltaTime, paddleL, paddleR);

            // Gaining point by player or by opponent
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
  
            
            // Increase the level after 15 seconds 
            //if (getTime() % 15 == 0 && running)
               // LevelUp();

        }
        //----------------------------------------------------------------------------------------------
        public void movePlayer()
        {
            // Moving player's paddle
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
        public virtual void moveOpponent()
        {
    
            Random rnd = new Random();

            // Delay computer's paddle (after 2 seconds) 
            if (getTime() % 2 == 0)
                delay = rnd.Next(270);

            // Moving computer's paddle
            if (ball.ballShape.Position.Y < paddleR.Position.Y + paddleR.Size.Y / 2 && paddleR.Position.Y > 5f)
                paddleR.Position += new Vector2f(0f, (paddleSpeed - delay) * deltaTime * (-1));
            else if (ball.ballShape.Position.Y > paddleR.Position.Y + paddleR.Size.Y / 2 && paddleR.Position.Y < Settings.HEIGHT - (paddleR.Size.Y + 5))
                paddleR.Position += new Vector2f(0f, (paddleSpeed - delay) * deltaTime * 1);

        }
        //----------------------------------------------------------------------------------------------
        public override void postRender()
        {
            // Display paddles
            window.Draw(paddleL);
            window.Draw(paddleR);

        }
        //----------------------------------------------------------------------------------------------
    }
}
