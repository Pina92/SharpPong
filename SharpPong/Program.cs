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
           
            // Background
            Texture backgroundTexture = new Texture("textures/background.png");
            RectangleShape background = new RectangleShape(new Vector2f(WIDTH, HEIGHT));
            backgroundTexture.Repeated = true;
            background.Texture = backgroundTexture;
            background.TextureRect = new IntRect(0,0,WIDTH,HEIGHT);           

            // CLOCK
            Clock clock = new Clock();

            // creating new game
            Game game = new Game(1);

            // GAME LOOP
            while (window.IsOpen)
            {
                float deltaTime = clock.Restart().AsSeconds();

                // Process events
                window.DispatchEvents();

                // Clear screen
                window.Clear(windowColor);
 
                game.move(deltaTime);
          
                text.DisplayedString = game.getTime();
                CircleShape ball = game.getBall();
     
                // Displaying everything on screen
                window.Draw(background);
                window.Draw(game.paddleL);
                window.Draw(game.paddleR);
                window.Draw(ball);
                window.Draw(text);

                // Update the window
                window.Display();                            
            }
        }
    }
}
