﻿using System;
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
        public RectangleShape paddleL, paddleR;
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
        }
        //----------------------------------------------------------------------------------------------
        public override void move()
        {

            // Moving player's paddle
            if (Keyboard.IsKeyPressed(Keyboard.Key.Up) && paddleL.Position.Y > 5f)
            {
                paddleL.Position += new Vector2f(0f, -paddleSpeed * deltaTime);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Down) && paddleL.Position.Y < Settings.HEIGHT - (paddleL.Size.Y + 5))
            {
                paddleL.Position += new Vector2f(0f, paddleSpeed * deltaTime);
            }

            // Moving opponent's paddle
            moveOpponent();

            // Moving the ball
            int winner = ball.movingPong(deltaTime, paddleL, paddleR);
            
            // Gaining point
            if (winner == -1)
                playerL.score += 1;
            else if (winner == 1)
                playerR.score += 1;

            // TODO: Loosing game

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
