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

            // Display menu
            Menu menu = new Menu(window);            

        }
        //----------------------------------------
    }
}
