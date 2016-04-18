using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPong
{
    class Player
    {
        public string name;
        public int score;
        

        public Player(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }
}
