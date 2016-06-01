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
        float delay = 0;
        public RectangleShape paddle, paddleOp, paddleL, paddleR;
        public string[] keysPlayer;
        //----------------------------------------------------------------------------------------------
        public Pong()
        {
            // Left paddle
            Texture paddleTexture = new Texture("resources/textures/paddle2.png");
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
            this.player1.setPlayersKeys(keysPlayer);
        }
        //----------------------------------------------------------------------------------------------
        public override void move()
        {

            // Moving player's paddle
            movePlayer();
            
            // Moving opponent's paddle
            //moveOpponent();

            // Moving the ball
            int winner = ball.movingPong(deltaTime, paddleL, paddleR);
            
            // Gaining point by player or by opponent
            if (winner == -1)
                player1.score += 1;
            else if (winner == 1)
                playerR.score += 1;
              
            // TODO: Loosing game

        }
        //----------------------------------------------------------------------------------------------
        public void movePlayer()
        {
            // Moving player's paddle
            if (Keyboard.IsKeyPressed((Keyboard.Key)player1.keys[0]) && paddle.Position.Y > 5f)
            {
                paddle.Position += new Vector2f(0f, -paddleSpeed * deltaTime);
            }
            if (Keyboard.IsKeyPressed((Keyboard.Key)player1.keys[1]) && paddle.Position.Y < Settings.HEIGHT - (paddle.Size.Y + 5))
            {
                paddle.Position += new Vector2f(0f, paddleSpeed * deltaTime);
            }
        }
        //----------------------------------------------------------------------------------------------
        public virtual void moveOpponent()
        {
    
            Random rnd = new Random();

            // Delay computer's paddle (after 3 seconds) 
            if (getTime() % 2 == 0)
                delay = rnd.Next(270);

            // Moving computer's paddle
            if (ball.ballShape.Position.Y < paddleR.Position.Y + paddleR.Size.Y / 2 && paddleR.Position.Y > 5f)
                paddleR.Position += new Vector2f(0f, (paddleSpeed - delay) * deltaTime * (-1));
            else if (ball.ballShape.Position.Y > paddleR.Position.Y + paddleR.Size.Y / 2 && paddleR.Position.Y < Settings.HEIGHT - (paddleR.Size.Y + 5))
                paddleR.Position += new Vector2f(0f, (paddleSpeed - delay) * deltaTime * 1);

        }
        //----------------------------------------------------------------------------------------------
        public override void displayRest(RenderWindow window)
        {
            // Display paddles
            window.Draw(paddleL);
            window.Draw(paddleR);

        }
        //----------------------------------------------------------------------------------------------
    }
}
