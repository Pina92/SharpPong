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

        //----------------------------------------       
        static void OnClose(object sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }
        //----------------------------------------

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

            // Display menu
            string option = menu(window, background);

            // Creating new game
            if (option == "Pong") {
                Pong pong = new Pong();
                pong.run(window, time, score, background, 2);
            }
            else if (option == "Arkanoid") {
                Arkanoid arkanoid = new Arkanoid();
                arkanoid.run(window, time, score, background, 2);
            }
            else if (option == "Exit")
                window.Close();
        }
        //----------------------------------------
        static string menu(RenderWindow window, RectangleShape background)
        {
            
            // Creating menu 
            Text[] menu = new Text[3];
            String[] list = { "Pong", "Arkanoid", "Exit" };

            for (int j = 0; j < 3; j++)
            {
                Text position = new Text("0.0", new Font("robotastic.ttf"));
                position.DisplayedString = list[j];
                position.Position = new Vector2f(Settings.WIDTH / 2 - 25, 250 + j * 120);
                position.CharacterSize = 25;
                position.Color = new Color(255, 255, 255, 170);

                menu[j] = position;

            }


            bool play = false;
            bool pressedNow = false;
            int i = 0;
            
            // Menu loop
            while (!play)
            {
               
                menu[i].Color = Color.Blue;
                
                if (Keyboard.IsKeyPressed(Keyboard.Key.Up) && i > 0 && !pressedNow)
                {
                    menu[i].Color = Color.White;
                    i--;
                    pressedNow = true;

                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.Down) && i < menu.Length - 1 && !pressedNow)
                {
                    menu[i].Color = Color.White;
                    i++;
                    pressedNow = true;
                }
                if (!Keyboard.IsKeyPressed(Keyboard.Key.Down) && !Keyboard.IsKeyPressed(Keyboard.Key.Up))
                {
                    pressedNow = false;
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.Return))
                {
                    break;
                }

                // Display everything on screen
                window.Draw(background);

                for (int k = 0; k < menu.Length; k++)
                    window.Draw(menu[k]);

                window.Display();

            }

            // Return the name of chosen position in menu
            return menu[i].DisplayedString;
        }
        //----------------------------------------

    }
}
