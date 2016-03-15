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

            //WINDOW
            // Create the main window
            ContextSettings settings = new ContextSettings();
            settings.AntialiasingLevel = 8;

            RenderWindow window = new RenderWindow(new VideoMode(WIDTH, HEIGHT), "#Pong", Styles.Default, settings);
            window.Closed += new EventHandler(OnClose);
            Color windowColor = new Color(0, 192, 255);          

            //TEXT
            Text text = new Text("Hello world", new Font("robotastic.ttf"));
            text.Position = new Vector2f(WIDTH / 2 - 50, 10);                   
            text.CharacterSize = 18;                                   
            text.Color = new Color(255, 255, 255, 170);          

            //SPRITY 
            Texture paddleTexture = new Texture("paddle.png");
            RectangleShape paddleL = new RectangleShape(new Vector2f(25,100));
            paddleL.Texture = paddleTexture;
            paddleL.Position = new Vector2f(10, HEIGHT / 2);
            //padddle.Origin

            RectangleShape paddleR = new RectangleShape(new Vector2f(25, 100));
            paddleR.Texture = paddleTexture;
            paddleR.Position = new Vector2f(WIDTH - paddleR.Size.X - 10, HEIGHT / 2);

            Texture ballTexture = new Texture("ball.png");
            CircleShape ball = new CircleShape(10);
            ball.Texture = ballTexture;
            ball.Position = new Vector2f(WIDTH / 2, HEIGHT / 2);

            //CLOCK
            Clock clock = new Clock();
            Clock timer = new Clock();
            float paddleSpeed = 300f;
            float ballSpeed = 200f;
            float horizontal = 1, vertical = 1;

            // Start the game loop
            while (window.IsOpen)
            {
                float deltaTime = clock.Restart().AsSeconds();

                // Process events
                window.DispatchEvents();

                // Clear screen
                window.Clear(windowColor);
                
                //MOVING PADDLE
                if(Keyboard.IsKeyPressed(Keyboard.Key.Up) && paddleL.Position.Y > 5f)
                {
                    paddleL.Position += new Vector2f(0f, -paddleSpeed * deltaTime);
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.Down) && paddleL.Position.Y < HEIGHT - (paddleL.Size.Y + 5))
                {
                    paddleL.Position += new Vector2f(0f, paddleSpeed * deltaTime);
                }

                //MOVING BALL                
                ball.Position += new Vector2f(ballSpeed * deltaTime * horizontal, ballSpeed * deltaTime * vertical);

                if (ball.Position.X < 10f || ball.Position.X > WIDTH - 10)
                     horizontal *= -1;
                if (ball.Position.Y < 10f || ball.Position.Y > HEIGHT - 10)
                    vertical *= -1;

                Time time = timer.ElapsedTime;
                
                window.Draw(paddleL);
                window.Draw(paddleR);
                window.Draw(ball);

                //DISPLAYING TIME
                String time2 = time.AsSeconds().ToString();
                text.DisplayedString = time2;
                window.Draw(text);

                // Update the window
                window.Display();
                            
            }
        }
    }
}
