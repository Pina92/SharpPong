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

            Texture ballTexture = new Texture("textures/ball.png");
            ballTexture.Smooth = true;
            this.ballShape.Texture = ballTexture;

            this.ballShape.Position = new Vector2f(Settings.WIDTH / 2, Settings.HEIGHT / 2);
        }
//----------------------------------------------------------------------------------------------------------------------------------------------------
        // Moving a ball + returning '1' if player on the right side wins or '-1' if player on the left side wins ('0' if nobody wins) 
        public int movingPong(float deltaTime, RectangleShape paddleL, RectangleShape paddleR)
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
        //Return true if player don't hit the ball and loose 
        public bool movingArkanoid(float deltaTime, RectangleShape paddle, Tiles tiles)
        {
            ballShape.Position += new Vector2f(speed * deltaTime * horizontal, speed * deltaTime * vertical);

            if (ballShape.Position.X < 0f || ballShape.Position.X > Settings.WIDTH - size)
                horizontal *= -1;

            // Checking collison between ball and paddle or top wall 
            if (ballShape.Position.X < paddle.Position.X + paddle.Size.X &&
                ballShape.Position.X > paddle.Position.X &&
                ballShape.Position.Y + ballShape.Radius > paddle.Position.Y ||
                ballShape.Position.Y < 0f)
                    vertical *= -1;
            
            if (ballShape.Position.Y > Settings.HEIGHT)
            {
                ballShape.Position = new Vector2f(Settings.WIDTH / 2, Settings.HEIGHT / 2);                
                return true;
            }

            // Checking if ball hits one of tiles
            int x = (int)Math.Floor(ballShape.Position.X / tiles.sizeX);
            int y = (int)Math.Floor((ballShape.Position.Y - tiles.sizeY) / tiles.sizeY);

            if (x >= 0 && y < tiles.yTab && y >= 0 && x < tiles.xTab && tiles.tileMap[x,y] == 49 )
            {
                tiles.tileMap[x, y] = 0;
                vertical *= -1;
            }
            if (x >= 0 && y < tiles.yTab && y >= 0 && x < tiles.xTab && tiles.tileMap[x, y] == 50)
            {
                tiles.tileMap[x, y] = 49;
                vertical *= -1;
                tiles.tiles[x, y].Texture = new Texture("textures/brick2.png");
            }

            return false;
        }
//----------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
