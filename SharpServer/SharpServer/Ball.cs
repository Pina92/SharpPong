using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    class Ball
    {

        private float ballX, ballY;
        private int ballSize;
        private int ballSpeed;
        private int horizontal = 1, vertical = 1;
        //------------------------------------------------------------------------------------------------------------------
        public Ball(int ballSize, int ballSpeed)
        {

            this.ballX = Settings.WIDTH / 2;
            this.ballY = Settings.HEIGHT / 2;
            this.ballSize = ballSize;
            this.ballSpeed = ballSpeed;

        }
        //------------------------------------------------------------------------------------------------------------------
        // Moving a ball and returning '1' if player on the right side wins, '-1' if player on the left side wins or '0' if nobody wins.
        public int MovingBall(float paddleLeftX, float paddleLeftY, float paddleRightX, float paddleRightY)
        {
            // Moving a ball.
            ballX += ballSpeed * horizontal;
            ballY += ballSpeed * vertical;

            // Checking collison between the ball and walls.
            // Top wall. 
            if (ballY <= 0f) 
            {
                vertical *= -1;
                ballY = 0f;
            }
            // Bottom wall.
            if (ballY + ballSize + 10 >= Settings.HEIGHT)
            {
                vertical *= -1;
                ballY = Settings.HEIGHT - ballSize - 10;
            }

            // Checking collison between ball and left paddle.
            if (ballX <= paddleLeftX + Settings.paddleSizeX &&
                ballX >= paddleLeftX &&
                ballY >= paddleLeftY - ballSize &&
                ballY <= paddleLeftY + Settings.paddleSizeY)
            {
                horizontal *= -1;
                ballX = paddleLeftX + Settings.paddleSizeX;             
            }

            // Game over for left player.
            if (ballX <= 0f)
            {

                ballX = Settings.WIDTH / 2;
                ballY = Settings.HEIGHT / 2;

                ballSpeed = 8;

                return 1;
            }

            // Checking collison between ball and right paddle. 
            if (ballX + ballSize + 10 >= paddleRightX &&
                ballX + ballSize <= paddleRightX + Settings.paddleSizeX &&
                ballY + ballSize >= paddleRightY &&
                ballY <= paddleRightY + Settings.paddleSizeY)
            {
                horizontal *= -1;
                ballX = paddleRightX - ballSize - 10;
            }

            // Game over for right player.
            if (ballX + ballSize >= Settings.WIDTH)
            {

                ballX = Settings.WIDTH / 2;
                ballY = Settings.HEIGHT / 2;

                ballSpeed = 8;

                return -1;
            }

            return 0;

        }
        //------------------------------------------------------------------------------------------------------------------
        public float GetBallX()
        {
            return ballX;
        }

        public float GetBallY()
        {
            return ballY;
        }
        //------------------------------------------------------------------------------------------------------------------
    }
}
