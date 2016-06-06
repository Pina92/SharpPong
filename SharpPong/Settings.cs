using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace SharpPong
{
    class Settings
    {
        bool multiplayer;
        int level;
        int winningScore;
        string keysP1, keysP2;

        public readonly static uint WIDTH = 800;
        public readonly static uint HEIGHT = 600;

        // Resource Manager holding textures and fonts
        private static ResourceManager resourceManager = new ResourceManager();
        public readonly static Font robotasticF = resourceManager.GetFont("resources/robotastic.ttf");
        public readonly static Texture backgroundT = resourceManager.getTexture("resources/textures/background.png");
        public readonly static Texture paddleT = resourceManager.getTexture("resources/textures/paddle2.png");
        public readonly static Texture ballT = resourceManager.getTexture("resources/textures/ball.png");
        public readonly static Texture brick0T = resourceManager.getTexture("resources/textures/brick0.png");
        public readonly static Texture brick1T = resourceManager.getTexture("resources/textures/brick1.png");
        public readonly static Texture brick2T = resourceManager.getTexture("resources/textures/brick2.png");

        public Settings()
        {

        }

        public void setSettings()
        {
            // Pobranie od gracza ustawień
        }
    }
}
