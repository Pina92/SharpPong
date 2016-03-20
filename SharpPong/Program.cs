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
           
            // Window
            ContextSettings settings = new ContextSettings();
            settings.AntialiasingLevel = 8;

            RenderWindow window = new RenderWindow(new VideoMode(Settings.WIDTH, Settings.HEIGHT), "#Pong", Styles.Default, settings);
            window.Closed += new EventHandler(OnClose);
            Color windowColor = new Color(0, 192, 255);          
            
            // Text
            Text time = new Text("0.0", new Font("robotastic.ttf"));
            time.Position = new Vector2f(Settings.WIDTH - 90, 10);
            time.CharacterSize = 18;
            time.Color = new Color(255, 255, 255, 170);

            Text score = new Text("0:0", new Font("robotastic.ttf"));
            score.Position = new Vector2f(Settings.WIDTH / 2 - 30, 10);
            score.CharacterSize = 22;
            score.Color = new Color(255, 255, 255, 170);

            // Background
            Texture backgroundTexture = new Texture("textures/background.png");
            RectangleShape background = new RectangleShape(new Vector2f(Settings.WIDTH, Settings.HEIGHT));
            backgroundTexture.Repeated = true;
            background.Texture = backgroundTexture;
            background.TextureRect = new IntRect(0, 0, (int)Settings.WIDTH, (int)Settings.HEIGHT);           

            // Creating new game
            Game game = new Game(1);
            game.run(window, time, score, background);

        }
    }
}
