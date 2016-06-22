using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace SharpServer
{
    class Connection
    {

        private TcpClient player1, player2;
        private TcpListener connections; 

        private StreamReader receivePlayer1, receivePlayer2;
        private StreamWriter sendPlayer1, sendPlayer2;

        private Thread messagesPlayer1, messagesPlayer2, sendMessages;

        private string namePlayer1, namePlayer2;
        private string paddleLeftX, paddleLeftY, paddleRightX, paddleRightY;

        private bool connected;

        private Ball ball = new Ball(14, 8);
        //-------------------------------------------------------------------
        public Connection()
        {

            this.connected = false;
            StartListening();

        }
        //-------------------------------------------------------------------
        private void StartListening()
        {
            // Listen for connections from TCP network clients.
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            connections = new TcpListener(ipAddress, 8001);
            connections.Start();

            Console.WriteLine("Waiting for a connection...");
            WaitingForPlayers();

        }
        //-------------------------------------------------------------------
        // Get messages from player1.
        private void GetMessages1()
        {
            string[] dataPlayer1;

            try
            {
                while (connected)
                {

                    dataPlayer1 = receivePlayer1.ReadLine().Split('|');
                    paddleLeftX = dataPlayer1[0];
                    paddleLeftY = dataPlayer1[1];

                }
            }
            catch
            {
                // Close the connection and stream.
                receivePlayer1.Close();
                sendPlayer1.Close();
                player1.Close();
                Console.WriteLine("Player1 has disconnected.");

                if (player2.Connected == true)
                {
                    sendPlayer2.WriteLine("Stop");
                    sendPlayer2.Flush();

                    connected = false;
                    WaitingForPlayers();
                }
            }

        }
        //-------------------------------------------------------------------
        // Get messages from player2.
        private void GetMessages2()
        {
            string[] dataPlayer2;

            try
            {
                while (connected)
                {

                    dataPlayer2 = receivePlayer2.ReadLine().Split('|');
                    paddleRightX = dataPlayer2[0];
                    paddleRightY = dataPlayer2[1];

                }
            }
            catch
            {
                // Close the connection and stream.
                receivePlayer2.Close();
                sendPlayer2.Close();
                player2.Close();
                Console.WriteLine("Player2 has disconnected.");

                if (player1.Connected == true)
                {
                    sendPlayer1.WriteLine("Stop");
                    sendPlayer1.Flush();

                    connected = false;
                    WaitingForPlayers();
                }

            }

        }
        //-------------------------------------------------------------------
        // Send messages to the both players.
        private void SendMessages()
        {

            int seconds2 = DateTime.Now.Millisecond;

            try
            {
                while (connected)
                {                  
                    // Send message after 30 milliseconds.
                    int delay = Math.Abs(DateTime.Now.Millisecond - seconds2);
                    if (delay > 30)
                    {

                        MovingBall();
                        string ballCoordinates = Convert.ToString(ball.GetBallX()) + '|' + Convert.ToString(ball.GetBallY());

                        if (player1.Connected == true && paddleRightY != null)
                        {
                            sendPlayer1.WriteLine(paddleRightY + '|' + ballCoordinates);
                            sendPlayer1.Flush();
                        }

                        if (player2.Connected == true && paddleLeftY != null)
                        {
                            sendPlayer2.WriteLine(paddleLeftY + '|' + ballCoordinates);
                            sendPlayer2.Flush();
                        }

                        seconds2 = DateTime.Now.Millisecond;
                    }
                }
            }
            catch
            {
                Console.WriteLine("Sending stop");
            }
        }
        //-------------------------------------------------------------------
        private void WaitingForPlayers()
        {
            while (!connected)
            {
                if (player1 == null || player1.Connected == false)
               {
                    // Player1 joined the game.
                    player1 = connections.AcceptTcpClient();

                    receivePlayer1 = new StreamReader(player1.GetStream());
                    sendPlayer1 = new StreamWriter(player1.GetStream());

                    namePlayer1 = receivePlayer1.ReadLine();
                    Console.WriteLine(namePlayer1 + " joined the game.");

                    sendPlayer1.WriteLine("Left");
                    sendPlayer1.Flush();

               }
                else if (player2 == null || player2.Connected == false)
                {
                    // Player2 joined the game.
                    player2 = connections.AcceptTcpClient();

                    receivePlayer2 = new StreamReader(player2.GetStream());
                    sendPlayer2 = new StreamWriter(player2.GetStream());

                    namePlayer2 = receivePlayer2.ReadLine();
                    Console.WriteLine(namePlayer2 + " joined the game.");

                    sendPlayer2.WriteLine("Right");
                    sendPlayer2.Flush();

               }
               else if (player1.Connected == true && player2.Connected == true)
               {
                    // Both players are connected and the game can start.
                    sendPlayer1.WriteLine("Start");
                    sendPlayer1.Flush();

                    sendPlayer2.WriteLine("Start");
                    sendPlayer2.Flush();

                    Console.WriteLine("Listening...");
                    connected = true;

                    messagesPlayer1 = new Thread(GetMessages1);
                    messagesPlayer1.Start();

                    messagesPlayer2 = new Thread(GetMessages2);
                    messagesPlayer2.Start();

                    sendMessages = new Thread(SendMessages);
                    sendMessages.Start();

               }

            }

        }
        //-------------------------------------------------------------------
        private void MovingBall()
        {

            int gameOver = ball.MovingBall(Convert.ToSingle(paddleLeftX), Convert.ToSingle(paddleLeftY), Convert.ToSingle(paddleRightX), Convert.ToSingle(paddleRightY));

            if(gameOver != 0)
            {
                   
                paddleRightY = Convert.ToString(Settings.HEIGHT / 2 - Settings.paddleSizeY / 2);
                paddleLeftY = Convert.ToString(Settings.HEIGHT / 2 - Settings.paddleSizeY / 2);

            }

        }
        //-------------------------------------------------------------------
    }
}
