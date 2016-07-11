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
    class Arkanoid : Game
    {

        private Tiles tiles;
        private RectangleShape paddle;
        //----------------------------------------------------------------------------------------------
        public Arkanoid(RenderWindow rw) : base(rw)
        {
            
            // Creating tiles ( 9 in rows, 9 in columns).
            this.tiles = new Tiles( (int)Settings.WIDTH / 9, (int)(Settings.HEIGHT/2) / 9 );
            tiles.ReadTiles();

            // Paddle.
            this.paddle = new RectangleShape(new Vector2f(120, 20));
            paddle.Texture = ResourceManager.GetTexture("resources/textures/paddle1.png");
            paddle.Position = new Vector2f(Settings.WIDTH / 2 - paddle.Size.X / 2, Settings.HEIGHT - paddle.Size.Y - 10);

            // Ball.
            ball.ballShape.Position = new Vector2f(Settings.WIDTH / 2, Settings.HEIGHT / 2 + 25);

        }
        //----------------------------------------------------------------------------------------------
        protected override void Move()
        {

            if (window.HasFocus())
            {
                // Moving player's paddle.
                if (Keyboard.IsKeyPressed(Keyboard.Key.Left) && paddle.Position.X > 5f)
                    paddle.Position += new Vector2f(-paddleSpeed * deltaTime, 0f);

                if (Keyboard.IsKeyPressed(Keyboard.Key.Right) && paddle.Position.X < Settings.WIDTH - (paddle.Size.X + 5))
                    paddle.Position += new Vector2f(paddleSpeed * deltaTime, 0f);
            }

            // Moving the ball.
            running = ball.movingArkanoid(deltaTime, paddle, tiles);

        }
        //----------------------------------------------------------------------------------------------
        protected override void PostRender()
        {
            // Display paddle.
            window.Draw(paddle);

            // Display tiles.
            for (int x = 0; x < tiles.xTab; x++)
                for (int y = 0; y < tiles.yTab; y++)
                    if (tiles.tileMap[x, y] == '1' || tiles.tileMap[x, y] == '2')
                        window.Draw(tiles.tiles[x, y]);

        }
        //----------------------------------------------------------------------------------------------
    }
}
