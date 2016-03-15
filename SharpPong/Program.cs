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
    class Program
    {
        static void OnClose(object sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        static void Main(string[] args)
        {
            const int WIDTH = 800;
            const int HEIGHT = 600;

            // WINDOW
            ContextSettings settings = new ContextSettings();
            settings.AntialiasingLevel = 8;

            RenderWindow window = new RenderWindow(new VideoMode(WIDTH, HEIGHT), "#Pong", Styles.Default, settings);
            window.Closed += new EventHandler(OnClose);
            Color windowColor = new Color(0, 192, 255);          

            // TEXT
            Text text = new Text("Hello world", new Font("robotastic.ttf"));
            text.Position = new Vector2f(WIDTH / 2 - 50, 10);                   
            text.CharacterSize = 18;                                   
            text.Color = new Color(255, 255, 255, 170);          
             
            // SPRITES 
            // Left paddle
            Texture paddleTexture = new Texture("textures/paddle2.png");
            paddleTexture.Smooth = true;
            RectangleShape paddleL = new RectangleShape(new Vector2f(20,120));
            paddleL.Texture = paddleTexture;        
            paddleL.Position = new Vector2f(10, HEIGHT / 2 - paddleL.Size.Y / 2);
            
            // Right paddle
            RectangleShape paddleR = new RectangleShape(new Vector2f(20, 120));
            paddleR.Texture = paddleTexture;
            paddleR.Position = new Vector2f(WIDTH - paddleR.Size.X - 10, HEIGHT / 2 - paddleR.Size.Y/2);           

            // Ball
            Texture ballTexture = new Texture("textures/ball2.png");
            ballTexture.Smooth = true;
            CircleShape ball = new CircleShape(15);
            ball.Texture = ballTexture;
            ball.Position = new Vector2f(WIDTH / 2, HEIGHT / 2);

            // Background
            Texture backgroundTexture = new Texture("textures/background.png");
            RectangleShape background = new RectangleShape(new Vector2f(WIDTH, HEIGHT));
            backgroundTexture.Repeated = true;
            background.Texture = backgroundTexture;
            background.TextureRect = new IntRect(0,0,WIDTH,HEIGHT);           

            // CLOCK
            Clock clock = new Clock();
            Clock timer = new Clock();
            float paddleSpeed = 300f;
            float ballSpeed = 200f;
            float horizontal = 1, vertical = 1;

            // GAME LOOP
            while (window.IsOpen)
            {
                float deltaTime = clock.Restart().AsSeconds();

                // Process events
                window.DispatchEvents();

                // Clear screen
                window.Clear(windowColor);
                
                // Moving player's paddle
                if(Keyboard.IsKeyPressed(Keyboard.Key.Up) && paddleL.Position.Y > 5f)
                {
                    paddleL.Position += new Vector2f(0f, -paddleSpeed * deltaTime);
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.Down) && paddleL.Position.Y < HEIGHT - (paddleL.Size.Y + 5))
                {
                    paddleL.Position += new Vector2f(0f, paddleSpeed * deltaTime);
                }

                // Moving computer's paddle
                paddleR.Position += new Vector2f(0f, paddleSpeed * deltaTime * vertical);

                // Moving ball               
                ball.Position += new Vector2f(ballSpeed * deltaTime * horizontal, ballSpeed * deltaTime * vertical);

                if (ball.Position.X > WIDTH)
                     horizontal *= -1;
                if (ball.Position.Y < 10f || ball.Position.Y > HEIGHT - 10)
                    vertical *= -1;


                // Checking collison between ball and left paddle (player)
                if (ball.Position.X < paddleL.Position.X + paddleL.Size.X &&
                    ball.Position.X > paddleL.Position.X &&
                    ball.Position.Y > paddleL.Position.Y && 
                    ball.Position.Y < paddleL.Position.Y + paddleL.Size.Y)
                        horizontal *= -1;
                if (ball.Position.X < 0f)
                    ball.Position = new Vector2f(WIDTH/2, HEIGHT/2);

                
                // Checking collison between ball and right paddle (computer)
                if (ball.Position.X < paddleR.Position.X &&
                    ball.Position.X > paddleR.Position.X + paddleR.Size.X &&
                    ball.Position.Y > paddleR.Position.Y &&
                    ball.Position.Y < paddleR.Position.Y + paddleR.Size.Y)
                        horizontal *= -1;
                if (ball.Position.X > WIDTH)
                    ball.Position = new Vector2f(WIDTH / 2, HEIGHT / 2);
                    

                Time time = timer.ElapsedTime;
                String timeS = time.AsSeconds().ToString();
                text.DisplayedString = timeS;

                // Displaying everything on screen
                window.Draw(background);
                window.Draw(paddleL);
                window.Draw(paddleR);
                window.Draw(ball);
                window.Draw(text);

                // Update the window
                window.Display();                            
            }
        }
    }
}
