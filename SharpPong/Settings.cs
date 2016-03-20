using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Settings()
        {

        }

        public void setSettings()
        {
            // Pobranie od gracza ustawień
        }
    }
}
