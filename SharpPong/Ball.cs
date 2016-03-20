using System;
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
//----------------------------------------------------------------------------------------------------------------------------------------------------       
        public Ball(float size, float speed)
        {
            this.size = size;
            this.speed = speed;
            this.ballShape = new CircleShape(size);

            Texture ballTexture = new Texture("textures/ball2.png");
            ballTexture.Smooth = true;
            this.ballShape.Texture = ballTexture;

            this.ballShape.Position = new Vector2f(Settings.WIDTH / 2, Settings.HEIGHT / 2);
        }
//----------------------------------------------------------------------------------------------------------------------------------------------------
        // Moving a ball + returning '1' if player on the right side wins or '-1' if player on the left side wins ('0' if nobody wins) 
        public int moving(float deltaTime, RectangleShape paddleL, RectangleShape paddleR)
        {
            ballShape.Position += new Vector2f(speed * deltaTime * horizontal, speed * deltaTime * vertical);

            if (ballShape.Position.Y < 10f || ballShape.Position.Y > Settings.HEIGHT - 10 - size)
                vertical *= -1;

            // Checking collison between ball and left paddle (player)
            if (ballShape.Position.X < paddleL.Position.X + paddleL.Size.X  &&
                ballShape.Position.Y > paddleL.Position.Y &&
                ballShape.Position.Y < paddleL.Position.Y + paddleL.Size.Y)
            {
                horizontal *= -1;
            }
            if (ballShape.Position.X < 0f)
            {
                ballShape.Position = new Vector2f(Settings.WIDTH / 2, Settings.HEIGHT / 2);
                return 1;
            }

            // Checking collison between ball and right paddle (computer)
            if (ballShape.Position.X + size > paddleR.Position.X &&
                ballShape.Position.Y > paddleR.Position.Y &&
                ballShape.Position.Y < paddleR.Position.Y + paddleR.Size.Y)
            {
                horizontal *= -1;
            }
            if (ballShape.Position.X > Settings.WIDTH)
            {
                ballShape.Position = new Vector2f(Settings.WIDTH / 2, Settings.HEIGHT / 2);
                return -1;
            }

            return 0;
        }
//----------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
