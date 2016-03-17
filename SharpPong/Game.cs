using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace SharpPong
{
    class Game
    {
        int level;
        Ball ball;
        public RectangleShape paddleL, paddleR;
        Clock clock;

        public Game(int level)
        {
            this.level = level;
            this.clock = new Clock();
            this.ball = new Ball(14, 200); //  temporarily

            // Left paddle
            Texture paddleTexture = new Texture("textures/paddle2.png");
            paddleTexture.Smooth = true;
            this.paddleL = new RectangleShape(new Vector2f(20, 120));
            paddleL.Texture = paddleTexture;
            paddleL.Position = new Vector2f(10, 600 / 2 - paddleL.Size.Y / 2);

            // Right paddle
            this.paddleR = new RectangleShape(new Vector2f(20, 120));
            paddleR.Texture = paddleTexture;
            paddleR.Position = new Vector2f(800- paddleR.Size.X - 10, 600 / 2 - paddleR.Size.Y / 2);
        }

        public void LevelUp()
        {
            level++;
            ball.speed += 20;
            ball.size -= 1;
            
        }


        public void move(float deltaTime)
        {
            float paddleSpeed = 300f;

            // Moving player's paddle
            if (Keyboard.IsKeyPressed(Keyboard.Key.Up) && paddleL.Position.Y > 5f)
            {
                paddleL.Position += new Vector2f(0f, -paddleSpeed * deltaTime);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Down) && paddleL.Position.Y < 600 - (paddleL.Size.Y + 5))
            {
                paddleL.Position += new Vector2f(0f, paddleSpeed * deltaTime);
            }

            // Moving computer's paddle
            paddleR.Position += new Vector2f(0f, paddleSpeed * deltaTime * ball.horizontal);

            ball.moving(deltaTime, paddleL, paddleR);
    
        }


        public string getTime()
        {
            Time time = clock.ElapsedTime;
            String timeS = time.AsSeconds().ToString();

            return timeS;
        }


        public CircleShape getBall()
        {
            return ball.ballShape;
        }

    }
}
