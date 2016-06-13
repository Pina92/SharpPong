using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace SharpPong
{
    class PongHotSeat : Pong
    {
        private string[] keysPlayerR;

        public PongHotSeat(RenderWindow rw) : base(rw)
        {

            // Players keys
            this.keysPlayer = new string[2] { "W", "S" };
            this.playerL.setPlayersKeys(keysPlayer);

            this.keysPlayerR = new string[2] { "Up", "Down" };
            this.playerR.setPlayersKeys(keysPlayerR);

        }
        //-------------------------------------------------
        public override void moveOpponent()
        {
            // Moving player's paddle
            if (Keyboard.IsKeyPressed((Keyboard.Key)playerR.keys[0]) && paddleR.Position.Y > 5f)
            {
                paddleR.Position += new Vector2f(0f, -paddleSpeed * deltaTime);
            }
            if (Keyboard.IsKeyPressed((Keyboard.Key)playerR.keys[1]) && paddleR.Position.Y < Settings.HEIGHT - (paddleR.Size.Y + 5))
            {
                paddleR.Position += new Vector2f(0f, paddleSpeed * deltaTime);
            }

        }
        //-------------------------------------------------

    }
}
