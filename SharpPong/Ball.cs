﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace SharpPong
{
    class Ball
    {
        public CircleShape ballShape;
        public float size { get; set; }
        public float speed { get; set; }
        public float horizontal = 1, vertical = 1;
        
        public Ball(float size, float speed)
        {
            this.size = size;
            this.speed = speed;
            this.ballShape = new CircleShape(size);

            Texture ballTexture = new Texture("textures/ball2.png");
            ballTexture.Smooth = true;
            this.ballShape.Texture = ballTexture;

            this.ballShape.Position = new Vector2f(800 / 2, 600 / 2);

        }

        //poruszanie się piłeczki + zwrócenie true jeśli nastąpi przegrana
        public bool moving(float deltaTime, RectangleShape paddleL, RectangleShape paddleR)
        {
            ballShape.Position += new Vector2f(speed * deltaTime * horizontal, speed * deltaTime * vertical);

            if (ballShape.Position.X > 800)
                horizontal *= -1;
            if (ballShape.Position.Y < 10f || ballShape.Position.Y > 600 - 10)
                vertical *= -1;

            // Checking collison between ball and left paddle (player)
            if (ballShape.Position.X < paddleL.Position.X + paddleL.Size.X &&
                ballShape.Position.X > paddleL.Position.X &&
                ballShape.Position.Y > paddleL.Position.Y &&
                ballShape.Position.Y < paddleL.Position.Y + paddleL.Size.Y)
                    horizontal *= -1;
            if (ballShape.Position.X < 0f)
            {
                ballShape.Position = new Vector2f(800 / 2, 600 / 2);
                return true;
            }


            // Checking collison between ball and right paddle (computer)
            if (ballShape.Position.X < paddleR.Position.X &&
                ballShape.Position.X > paddleR.Position.X + paddleR.Size.X &&
                ballShape.Position.Y > paddleR.Position.Y &&
                ballShape.Position.Y < paddleR.Position.Y + paddleR.Size.Y)
                horizontal *= -1;
            if (ballShape.Position.X > 800)
            {
                ballShape.Position = new Vector2f(800 / 2, 600 / 2);
                return true;
            }

            return false;
        }

    }
}
