using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SharpPong
{
    class Multiplayer : Pong
    {

        public Multiplayer(RenderWindow rw) : base(rw)
        {

            gameOn = false;
            StartConnection();

        }

        // Player informations.
        private string userName = "Unknown";

        private TcpClient player;
        private StreamWriter send;
        private StreamReader receive;

        private Thread sending, waiting;
        private string serverMessage;

        private float ballX, ballY;
        //------------------------------------------------------------------------
        public void StartConnection()
        {

            try
            {
                // Establishing the connection with server.
                player = new TcpClient();
                player.NoDelay = true;
                player.Connect("127.0.0.1", 8001);

                send = new StreamWriter(player.GetStream());
                receive = new StreamReader(player.GetStream());

                // Sending username to server.
                userName = "Paulina";
                send.WriteLine(userName);
                send.Flush();

                // On which side player has a paddle.
                serverMessage = receive.ReadLine();

                if (serverMessage == "Left")
                {
                    paddle = paddleL;
                    paddleOp = paddleR;

                    // this.keysPlayer = new string[2] { "Up", "Down" };
                    // this.player1.setPlayersKeys(keysPlayer);
                }
                else if (serverMessage == "Right")
                {
                    paddle = paddleR;
                    paddleOp = paddleL;

                    // this.keysPlayer = new string[2] { "W", "S" };
                    // this.player1.setPlayersKeys(keysPlayer);        
                }

                // Waiting for the opponent to connect. 
                waiting = new Thread(StartGame);
                waiting.Start();

            }
            catch
            {

                Console.WriteLine("Server is not responding...1");
                gameOn = false;

            }

        }
        //------------------------------------------------------------------------
        public void StartGame()                                        
        {

            try
            {
                while (!gameOn)
                {

                    serverMessage = receive.ReadLine();

                    // The game can start.
                    if (serverMessage == "Start")
                    {

                        gameOn = true;

                        // Sending coordinates of player's paddle to server.
                        sending = new Thread(new ThreadStart(SendMessage));
                        sending.Start();

                    }
                }
            }
            catch
            {

                Console.WriteLine("Server is not responding...2");
                gameOn = false;

            }

        }
        //------------------------------------------------------------------------
        protected override void Move()
        {
            int score;

            if (window.HasFocus())
            {
                // Moving player's paddle.
                MovePlayer();
            }

            // Moving opponent's paddle.
            MoveOpponent();

            // Moving the ball.
            score = ball.MovingMultiplayer(deltaTime, ballX, ballY);

            // Gaining point by player or by opponent.
            if (score == -1)
                playerL.score += 1;
            else if (score == 1)
                playerR.score += 1;

            if (score != 0)
            {

                ball.ballShape.Position = new Vector2f(Settings.WIDTH / 2, Settings.HEIGHT / 2);
                paddleL.Position = new Vector2f(10, Settings.HEIGHT / 2 - paddleL.Size.Y / 2);
                paddleR.Position = new Vector2f(Settings.WIDTH - paddleR.Size.X - 10, Settings.HEIGHT / 2 - paddleR.Size.Y / 2);

                running = false;

            }

        }
        //------------------------------------------------------------------------
        // Receiving and updating the position of the opponent paddle.
        protected override void MoveOpponent()
        {

            try
            {
                serverMessage = receive.ReadLine();

                if (serverMessage == "Waiting")
                {

                    Console.WriteLine("Your opponent left the game. You are the winner :)");
                    playerL.score = 0;
                    playerR.score = 0;

                    ballX = Settings.WIDTH / 2;
                    ballY = Settings.HEIGHT / 2;

                    paddleL.Position = new Vector2f(10, Settings.HEIGHT / 2 - paddleL.Size.Y / 2);
                    paddleR.Position = new Vector2f(Settings.WIDTH - paddleR.Size.X - 10, Settings.HEIGHT / 2 - paddleR.Size.Y / 2);           

                    gameOn = false;
                    running = false;

                    // Waiting for the opponent to connect. 
                    waiting = new Thread(StartGame);
                    waiting.Start();

                }
                else
                {
                    // Getting coordinates of opponent's paddle and ball from server (paddleY|ballX|ballY).
                    string[] coordinations = serverMessage.Split('|'); 

                    float paddleOpY = Convert.ToSingle(coordinations[0]);
                    paddleOp.Position = new Vector2f(paddleOp.Position.X, paddleOpY);

                    ballX = Convert.ToSingle(coordinations[1]);
                    ballY = Convert.ToSingle(coordinations[2]);

                }

            }
            catch
            {

                Console.WriteLine("Server is not responding...3");
                gameOn = false;

            }

        }
        //------------------------------------------------------------------------
        private void SendMessage()
        {

            int seconds2 = DateTime.Now.Millisecond;
            try
            {

                while (gameOn)
                {
                    // Send message after 30 milliseconds.
                    int delay = Math.Abs(DateTime.Now.Millisecond - seconds2);

                    if (delay > 30)
                    {
                        // Sending the position of the player paddle to server.
                        send.WriteLine(paddle.Position.X.ToString() + '|' + paddle.Position.Y.ToString());
                        send.Flush();
                        seconds2 = DateTime.Now.Millisecond;

                    }

                }

            }
            catch
            {
                Console.WriteLine("Sending stop");
            }

        }
        //------------------------------------------------------------------------
        protected override void EscMenu()
        {

            try
            {

                receive.Close();
                send.Close();
                player.Close();

            }
            catch { }

            Menu menu = new Menu(window);
            
        }
        //------------------------------------------------------------------------
    }
}
