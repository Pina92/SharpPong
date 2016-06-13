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
    class Menu
    {

        private RenderWindow window;
        private RectangleShape background;

        //----------------------------------------
        public Menu(RenderWindow rw)
        {
            this.window = rw;
            
            // Menu background
            this.background = new RectangleShape(new Vector2f(Settings.WIDTH, Settings.HEIGHT));            
            this.background.Texture = ResourceManager.getTexture("resources/textures/background.png");
            this.background.Texture.Repeated = true;
            this.background.TextureRect = new IntRect(0, 0, (int)Settings.WIDTH, (int)Settings.HEIGHT);

            display();

        }
        //----------------------------------------
        private void display()
        {
            // Display menu
            string option = menu();

            // Creating new game
            if (option == "Pong")
            {
                Pong pong = new Pong(window);
                pong.run();
            }
            else if (option == "Hot-Seat")
            {
                PongHotSeat pongHotSeat = new PongHotSeat(window);
                pongHotSeat.run();
            }
            else if (option == "Arkanoid")
            {
                Arkanoid arkanoid = new Arkanoid(window);
                arkanoid.run();
            }
            else if (option == "Multiplayer")
            {
                Multiplayer multiplayer = new Multiplayer(window);
                multiplayer.run();
            }
            else if (option == "Exit")
                window.Close();
        }
        //----------------------------------------
        private string menu()
        {

            // Creating menu 
            Text[] menu = new Text[5];
            String[] list = { "Pong", "Hot-Seat", "Multiplayer", "Arkanoid", "Exit" };
            Color blue = new Color(255, 255, 255, 170);

            for (int j = 0; j < 5; j++)
            {
                Text position = new Text("0.0", ResourceManager.GetFont("resources/robotastic.ttf"));
                position.DisplayedString = list[j];
                position.Position = new Vector2f(Settings.WIDTH / 2 - 25, 50 + j * 120);
                position.CharacterSize = 25;
                position.Color = blue;

                menu[j] = position;

            }


            bool play = false;
            bool pressedNow = false;
            int i = 0;

            // Menu loop
            while (!play)
            {
                // Process events
                window.DispatchEvents();

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
