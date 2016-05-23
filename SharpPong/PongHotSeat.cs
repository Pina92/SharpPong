using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Window;

namespace SharpPong
{
    class PongHotSeat : Pong
    {

        public override void moveOpponent()
        {
            // Moving player's paddle
            if (Keyboard.IsKeyPressed(Keyboard.Key.W) && paddleR.Position.Y > 5f)
            {
                paddleR.Position += new Vector2f(0f, -paddleSpeed * deltaTime);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.S) && paddleR.Position.Y < Settings.HEIGHT - (paddleR.Size.Y + 5))
            {
                paddleR.Position += new Vector2f(0f, paddleSpeed * deltaTime);
            }
        }
    }
}
