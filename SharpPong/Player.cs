using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using SFML.System;
using SFML.Window;

namespace SharpPong
{
    class Player
    {
        public string name;
        public int score;
        public ArrayList keys;
        //-------------------------------------------------------------------
        public Player(string name, int score)
        {
            this.name = name;
            this.score = score;
            this.keys = new ArrayList();
        }

        //-------------------------------------------------------------------
        public void setPlayersKeys(string[] keystr)
        {
            keys.Clear();
            for (int i = 0; i < keystr.Length; i++)
            {
                Keyboard.Key key = (Keyboard.Key)Enum.Parse(typeof(Keyboard.Key), keystr[i]);
                this.keys.Add(key);

               /* foreach (Keyboard.Key j in keys)
                {
                    Console.WriteLine(j);
                }*/
            }
            
        }
        //-------------------------------------------------------------------
    }
}
