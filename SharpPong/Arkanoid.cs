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
        Tiles tiles;
        public RectangleShape paddle;
        //----------------------------------------------------------------------------------------------
        public Arkanoid()
        {
            // Reading tiles
            this.tiles = new Tiles(97, 35);
            tiles.readTiles();

            // Paddle
            this.paddle = new RectangleShape(new Vector2f(120, 20));
            Texture paddleTexture = new Texture("textures/paddle2.png");
            paddle.Texture = paddleTexture;
            paddle.Position = new Vector2f(Settings.WIDTH / 2 - paddle.Size.X, Settings.HEIGHT - paddle.Size.Y - 10);
        }
        //----------------------------------------------------------------------------------------------
        public override void move()
        {
            // Moving player's paddle
            if (Keyboard.IsKeyPressed(Keyboard.Key.Left) && paddle.Position.X > 5f)
                paddle.Position += new Vector2f(-paddleSpeed * deltaTime, 0f);

            if (Keyboard.IsKeyPressed(Keyboard.Key.Right) && paddle.Position.X < Settings.WIDTH - (paddle.Size.X + 5))
                paddle.Position += new Vector2f(paddleSpeed * deltaTime, 0f);

            // Moving the ball
            loose = ball.movingArkanoid(deltaTime, paddle, tiles);

            seconds = DateTime.Now.Second;
        }
        //----------------------------------------------------------------------------------------------
        public override void displayRest(RenderWindow window)
        {
            // Display paddle
            window.Draw(paddle);

            // Display tiles
            for (int x = 0; x < tiles.xTab; x++)
                for (int y = 0; y < tiles.yTab; y++)
                    if (tiles.tileMap[x, y] == 49 || tiles.tileMap[x, y] == 50)
                        window.Draw(tiles.tiles[x, y]);

        }
        //----------------------------------------------------------------------------------------------
    }
}
