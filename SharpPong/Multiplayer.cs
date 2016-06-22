﻿using System;
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
           startConnection();
        }

        // Player informations
        private string userName = "Unknown";

        private TcpClient player;
        private StreamWriter send;
        private StreamReader receive;

        private Thread sending, waiting;
        string serverMessage;

        private float ballX, ballY;
        //------------------------------------------------------------------------
        public void startConnection()
        {
            try
            {
                // Establishing the connection to the server
                player = new TcpClient();
                player.NoDelay = true;
                player.Connect("127.0.0.1", 8001);

                send = new StreamWriter(player.GetStream());
                receive = new StreamReader(player.GetStream());

                // Sending username to server
                userName = "Paulina";
                send.WriteLine(userName);
                send.Flush();

                // On with side player has a paddle
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

                // Waiting for the opponent to connect 
                waiting = new Thread(startGame);
                waiting.Start();
            }
            catch
            {
                Console.WriteLine("Server is not responding...");
                // TO-DO: Back to the menu
                gameOn = false;
            }

        }
        //------------------------------------------------------------------------
        public void startGame()
        {
            try
            {
                serverMessage = receive.ReadLine();

                // The game can start
                if (serverMessage == "Start")
                {

                    gameOn = true;

                    // Sending coordinates of player's paddle to server
                    sending = new Thread(new ThreadStart(sendMessage));
                    sending.Start();

                }
            }
            catch
            {
                Console.WriteLine("Server is not responding...");
                // TO-DO: Back to the menu
                gameOn = false;
            }

        }
        //------------------------------------------------------------------------
        public override void move()
        {         

            // Moving player's paddle
            movePlayer();

            // Moving opponent's paddle
            moveOpponent();

            // Moving the ball
            ball.MovingMultiplayer(deltaTime, ballX, ballY);

        }
        //------------------------------------------------------------------------
        // Receiving and updating the position of the opponent paddle
        public override void moveOpponent()
        {
            try
            {
                serverMessage = receive.ReadLine();

                if (serverMessage == "Stop")
                {

                    Console.WriteLine("Your opponent left the game. You are the winner :)");
                    gameOn = false;
                    running = false;
                    playerL.score = 0;
                    playerR.score = 0;
                    timer.Restart();

                    startGame();

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
                Console.WriteLine("Server is not responding...");
                // TO-DO: Back to the menu
                gameOn = false;
            }
        }
        //------------------------------------------------------------------------
        private void sendMessage()
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
            Console.WriteLine("Sending stop2");
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
