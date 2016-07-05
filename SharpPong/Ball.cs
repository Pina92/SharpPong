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
        public float ballSize;
        public float speed;
        private int horizontal = 1, vertical = 1;
        private float angle;
        private const float MAX_ANGLE = 75;
        //------------------------------------------------------------------------------------------------------------------
        public Ball(float ballSize, float speed)
        {
            this.ballSize = ballSize;
            this.speed = speed;
            this.ballShape = new CircleShape(ballSize);
            this.angle = (float)Math.PI * 45 / 180;

            Texture ballTexture = ResourceManager.GetTexture("resources/textures/ball.png");
            ballTexture.Smooth = true;
            this.ballShape.Texture = ballTexture;

            this.ballShape.Position = new Vector2f(Settings.WIDTH / 2, Settings.HEIGHT / 2);
        }
        //------------------------------------------------------------------------------------------------------------------
        // Moving a ball and returning '1' if player on the right side wins or '-1' if player on the left side wins or '0' if nobody wins. 
        public int movingPong(float deltaTime, RectangleShape paddleL, RectangleShape paddleR)
        {

            float sin = (float) Math.Sin(angle);
            float cos = (float) Math.Cos(angle);

            // Updating ball position. 
            float xDirection = speed * deltaTime * horizontal * cos;
            float yDirection = speed * deltaTime * vertical * sin;
            ballShape.Position += new Vector2f(xDirection, yDirection);

            // Checking collison between the ball and walls.  
            // Top wall.
            if (ballShape.Position.Y <= 0f) 
            {
                vertical *= -1;
                ballShape.Position = new Vector2f(ballShape.Position.X, 0f);
            }
            // Bottom wall.
            if (ballShape.Position.Y + ballSize + 10 >= Settings.HEIGHT) 
            {
                vertical *= -1;
                ballShape.Position = new Vector2f(ballShape.Position.X, Settings.HEIGHT - ballSize - 10);
            }
            
            // Checking collison between ball and left paddle.
            if (ballShape.Position.X <= paddleL.Position.X + paddleL.Size.X  &&
                ballShape.Position.X >= paddleL.Position.X &&
                ballShape.Position.Y + ballSize >= paddleL.Position.Y &&
                ballShape.Position.Y <= paddleL.Position.Y + paddleL.Size.Y)
            {
                // Calculation ball angle according to the possition where it hits.              
                float PADDLE_HALF_WIDTH = paddleL.Size.Y / 2;
                angle = ((paddleL.Position.Y + PADDLE_HALF_WIDTH) - (ballShape.Position.Y + ballSize / 2)) / PADDLE_HALF_WIDTH * MAX_ANGLE;

                if (angle > MAX_ANGLE)
                    angle = MAX_ANGLE;

                // Converting angle to radians.
                angle = angle * (float)Math.PI / 180;

                vertical = -1;
                horizontal *= -1;

                ballShape.Position = new Vector2f(paddleL.Position.X + paddleL.Size.X, ballShape.Position.Y);

            }
            
            // Game over for left player.
            if (ballShape.Position.X <= 0f)
            {
                // Setting paddle on the middle of its side. 
                ballShape.Position = new Vector2f(Settings.WIDTH / 2, Settings.HEIGHT / 2);

                // Changing ball movement to opponent.
                horizontal *= -1;
                vertical *= -1;
                angle = 45 * (float)Math.PI / 180;

                speed = 300;

                return 1;
            }

            // Checking collison between ball and right paddle. 
            if (ballShape.Position.X + ballSize + 10 >= paddleR.Position.X &&
                ballShape.Position.X + ballSize <= paddleR.Position.X + paddleR.Size.X &&
                ballShape.Position.Y >= paddleR.Position.Y - ballSize &&
                ballShape.Position.Y <= paddleR.Position.Y + paddleR.Size.Y)
            {
                // Calculation ball angle according to the possition where it hits.              
                float PADDLE_HALF_WIDTH = paddleR.Size.Y / 2;
                angle = ((paddleR.Position.Y + PADDLE_HALF_WIDTH) - (ballShape.Position.Y + ballSize / 2)) / PADDLE_HALF_WIDTH * MAX_ANGLE;

                if (angle > MAX_ANGLE)
                    angle = MAX_ANGLE;

                // Converting angle to radians.
                angle = angle * (float)Math.PI / 180;

                vertical = -1;
                horizontal *= -1;

                ballShape.Position = new Vector2f(paddleR.Position.X - ballSize - 10, ballShape.Position.Y);

            }

            // Game over for right player.
            if (ballShape.Position.X + ballSize >= Settings.WIDTH)
            {
                // Setting paddle on the middle of its side. 
                ballShape.Position = new Vector2f(Settings.WIDTH / 2, Settings.HEIGHT / 2);
                
                // Changing ball movement to opponent.
                horizontal *= -1;
                vertical *= -1;
                angle = 45 * (float)Math.PI / 180;

                speed = 300;

                return -1;
            }

            return 0;

        }
        //------------------------------------------------------------------------------------------------------------------
        public int MovingMultiplayer(float deltaTime, float ballX, float ballY)
        {

            ballShape.Position = new Vector2f(ballX, ballY);

            // Game over for left player.
            if (ballShape.Position.X <= 0f)
                return 1;
            
            // Game over for right player.
            if (ballShape.Position.X + ballSize >= Settings.WIDTH)
                return -1;

            return 0;

        }
        //------------------------------------------------------------------------------------------------------------------
        // Moving a ball and returning true if player don't hit the ball and loose. 
        public bool movingArkanoid(float deltaTime, RectangleShape paddle, Tiles tiles)
        {            

            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);
            // Updating ball position. 
            ballShape.Position += new Vector2f(speed * deltaTime * horizontal * (float)sin , speed * deltaTime * vertical * (float)cos);


            // Checking collison between the ball and walls.  
            // Left wall.
            if (ballShape.Position.X <= 0f) 
            {
                horizontal *= -1;
                ballShape.Position = new Vector2f(0f, ballShape.Position.Y);
            }
            // Right wall.
            if (ballShape.Position.X >= Settings.WIDTH - ballShape.Radius)
            {
                horizontal *= -1;
                ballShape.Position = new Vector2f(Settings.WIDTH - ballShape.Radius, ballShape.Position.Y);
            }
            // Top wall.
            if (ballShape.Position.Y <= 0f)
            { 
                vertical *= -1;
                ballShape.Position = new Vector2f(ballShape.Position.X, 0f);
            }

            // Checking collison between the ball and paddle. 
            if (ballShape.Position.X <= paddle.Position.X + paddle.Size.X &&
                ballShape.Position.X + ballShape.Radius >= paddle.Position.X &&
                ballShape.Position.Y + ballShape.Radius + 10 >= paddle.Position.Y &&
                ballShape.Position.Y + ballShape.Radius <= paddle.Position.Y + paddle.Size.Y)
            {
                // Calculation ball angle according to the possition where it hits.              
                float PADDLE_HALF_WIDTH = paddle.Size.X / 2;
                angle = ((paddle.Position.X + PADDLE_HALF_WIDTH) - (ballShape.Position.X + ballSize / 2)) / PADDLE_HALF_WIDTH * MAX_ANGLE;

                if (angle > MAX_ANGLE)
                    angle = MAX_ANGLE;

                // Converting angle to radians.
                angle = angle * (float)Math.PI / 180;

                horizontal = -1;
                vertical *= -1;

                ballShape.Position = new Vector2f(ballShape.Position.X, paddle.Position.Y - ballShape.Radius - 10);
               
            }
           
         
            // Game over. 
            if (ballShape.Position.Y >= Settings.HEIGHT)
            {
                ballShape.Position = new Vector2f(Settings.WIDTH / 2, Settings.HEIGHT / 2 + 25);
                paddle.Position = new Vector2f(Settings.WIDTH / 2 - paddle.Size.X / 2, paddle.Position.Y);
                angle = 45 * (float)Math.PI / 180;
                speed = 300;

                return false;                
            }

            
            // Checking if ball hits one of tiles.
            int x = (int)Math.Floor(ballShape.Position.X / tiles.sizeX);
            int y = (int)Math.Floor((ballShape.Position.Y - tiles.sizeY) / tiles.sizeY);

            if (x >= 0 && x < tiles.xTab && y >= 0 && y < tiles.yTab && tiles.tileMap[x,y] == 49 )
            {
                tiles.tileMap[x, y] = '0';
                vertical *= -1;

            }
            if (x >= 0 && y < tiles.yTab && y >= 0 && x < tiles.xTab && tiles.tileMap[x, y] == 50)
            {
                tiles.tileMap[x, y] = '1';
                vertical *= -1;
                tiles.tiles[x, y].Texture = ResourceManager.GetTexture("resources/textures/brick2.png");
            }

            return true;
        }
        //------------------------------------------------------------------------------------------------------------------
    }
}
