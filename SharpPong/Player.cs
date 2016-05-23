using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SharpPong
{
    class Player
    {
        public string name;
        public int score;
        protected ArrayList keys;

        public Player(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }
}
