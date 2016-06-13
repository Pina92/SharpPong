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
        public float size;
        public float speed;
        public float horizontal = 1, vertical = 1;
        double angle;
        Random ran = new Random();
        //------------------------------------------------------------------------------------------------------------------
        public Ball(float size, float speed)
        {
            this.size = size;
            this.speed = speed;
            this.ballShape = new CircleShape(size);
            this.angle = Math.PI * 45 / 180;

            Texture ballTexture = ResourceManager.getTexture("resources/textures/ball.png");
            ballTexture.Smooth = true;
            this.ballShape.Texture = ballTexture;

            this.ballShape.Position = new Vector2f(Settings.WIDTH / 2, Settings.HEIGHT / 2);
        }
        //------------------------------------------------------------------------------------------------------------------
        // Moving a ball + returning '1' if player on the right side wins or '-1' if player on the left side wins ('0' if nobody wins) 
        public int movingPong(float deltaTime, RectangleShape paddleL, RectangleShape paddleR)
        {

            ballShape.Position += new Vector2f(speed * deltaTime * horizontal, speed * deltaTime * vertical);

            // Checking collison between the ball and walls  
            if (ballShape.Position.Y <= 0f) // Top wall
            {
                vertical *= -1;
                ballShape.Position = new Vector2f(ballShape.Position.X, 0f);
            }
            if(ballShape.Position.Y + size + 10 >= Settings.HEIGHT)// Bottom wall
            {
                vertical *= -1;
                ballShape.Position = new Vector2f(ballShape.Position.X, Settings.HEIGHT - size - 10);
            }
            
            // Checking collison between ball and left paddle
            if (ballShape.Position.X <= paddleL.Position.X + paddleL.Size.X  &&
                ballShape.Position.X >= paddleL.Position.X &&
                ballShape.Position.Y >= paddleL.Position.Y - size &&
                ballShape.Position.Y <= paddleL.Position.Y + paddleL.Size.Y)
            {
                horizontal *= -1;
                ballShape.Position = new Vector2f(paddleL.Position.X + paddleL.Size.X, ballShape.Position.Y);
            }
            
            // Game Over (Left Player)
            if (ballShape.Position.X + size <= 0f)
            {
                ballShape.Position = new Vector2f(Settings.WIDTH / 2, Settings.HEIGHT / 2);
                return 1;
            }

            // Checking collison between ball and right paddle 
            if (ballShape.Position.X + size + 10 >= paddleR.Position.X &&
                ballShape.Position.X + size <= paddleR.Position.X + paddleR.Size.X &&
                ballShape.Position.Y >= paddleR.Position.Y - size &&
                ballShape.Position.Y <= paddleR.Position.Y + paddleR.Size.Y)
            {
                horizontal *= -1;
                ballShape.Position = new Vector2f(paddleR.Position.X - ballShape.Radius - 10, ballShape.Position.Y);
            }

            // Game Over (Right Player)
            if (ballShape.Position.X + size >= Settings.WIDTH)
            {
                ballShape.Position = new Vector2f(Settings.WIDTH / 2, Settings.HEIGHT / 2);
                return -1;
            }

            return 0;

        }
        //------------------------------------------------------------------------------------------------------------------
        // Return true if player don't hit the ball and loose 
        public bool movingArkanoid(float deltaTime, RectangleShape paddle, Tiles tiles)
        {            

            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);
            // Updating ball position 
            ballShape.Position += new Vector2f(speed * deltaTime * horizontal * (float)sin , speed * deltaTime * vertical * (float)cos);


            // Checking collison between the ball and walls  
            if (ballShape.Position.X <= 0f) // Left wall
            {
                horizontal *= -1;
                ballShape.Position = new Vector2f(0f, ballShape.Position.Y);
            }
            if (ballShape.Position.X >= Settings.WIDTH - ballShape.Radius) // Right wall
            {
                horizontal *= -1;
                ballShape.Position = new Vector2f(Settings.WIDTH - ballShape.Radius, ballShape.Position.Y);
            }
            if (ballShape.Position.Y <= 0f) // Top wall 
            { 
                vertical *= -1;
                ballShape.Position = new Vector2f(ballShape.Position.X, 0f);
            }

            // Checking collison between the ball and paddle 
            if (ballShape.Position.X <= paddle.Position.X + paddle.Size.X &&
                ballShape.Position.X + ballShape.Radius >= paddle.Position.X &&
                ballShape.Position.Y + ballShape.Radius >= paddle.Position.Y &&
                ballShape.Position.Y + ballShape.Radius <= paddle.Position.Y + paddle.Size.Y)
            {
                // Changing ball angle according to the possition where it hits
                const float MAX_ANGLE = 75;
                float PADDLE_HALF_WIDTH = paddle.Size.X / 2;
                angle = ((paddle.Position.X + PADDLE_HALF_WIDTH) - (ballShape.Position.X + ballShape.Radius)) / PADDLE_HALF_WIDTH * MAX_ANGLE * (float)Math.PI / 180;
                
                horizontal = -1;
                vertical *= -1;

                ballShape.Position = new Vector2f(ballShape.Position.X, paddle.Position.Y - ballShape.Radius);
               
            }
           
         
            // Game over 
            if (ballShape.Position.Y >= Settings.HEIGHT)
            {
                ballShape.Position = new Vector2f(Settings.WIDTH / 2, Settings.HEIGHT / 2);
                paddle.Position = new Vector2f(Settings.WIDTH / 2 - paddle.Size.X / 2, paddle.Position.Y);
                angle = 45 * (float)Math.PI / 180;
                speed = 270;

                return false;                
            }

            
            // Checking if ball hits one of tiles
            int x = (int)Math.Floor(ballShape.Position.X / tiles.sizeX);
            int y = (int)Math.Floor((ballShape.Position.Y - tiles.sizeY) / tiles.sizeY);

            if (x >= 0 && x < tiles.xTab && y >= 0 && y < tiles.yTab && tiles.tileMap[x,y] == 49 )
            {
                tiles.tileMap[x, y] = '0';
                vertical *= -1;
                //horizontal *= -1;
            }
            if (x >= 0 && y < tiles.yTab && y >= 0 && x < tiles.xTab && tiles.tileMap[x, y] == 50)
            {
                tiles.tileMap[x, y] = '1';
                vertical *= -1;
                tiles.tiles[x, y].Texture = ResourceManager.getTexture("resources/textures/brick2.png");
            }

            return true;
        }
        //------------------------------------------------------------------------------------------------------------------
    }
}
