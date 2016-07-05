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

        private TcpClient playerLeft, playerRight;
        private TcpListener connections; 

        private StreamReader receivePlayerLeft, receivePlayerRight;
        private StreamWriter sendPlayerLeft, sendPlayerRight;

        private Thread messagesPlayerLeft, messagesPlayerRight, sendMessages, waitingForPlayer;

        private string namePlayerLeft, namePlayerRight;
        private string paddleLeftX, paddleLeftY, paddleRightX, paddleRightY;

        // True if both of players are connected.
        private bool connected, playerLeftConnected, playerRightConnected, counting;

        private Ball ball = new Ball(14, 8);

        public int seconds2;
        //-------------------------------------------------------------------
        public Connection()
        {

            this.connected = false;
            this.counting = true;

            // Listen for connections from TCP network clients.
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            connections = new TcpListener(ipAddress, 8001);
            connections.Start();

            sendMessages = new Thread(SendMessages);
            sendMessages.Start();

            waitingForPlayer = new Thread (WaitingForPlayer);
            waitingForPlayer.Start();

        }
        //-------------------------------------------------------------------
        // Get messages from left player.
        private void GetMessagesLeft()
        {

            string[] dataPlayerLeft;

            try
            {
                while (connected)
                {

                    dataPlayerLeft = receivePlayerLeft.ReadLine().Split('|');
                    paddleLeftX = dataPlayerLeft[0];
                    paddleLeftY = dataPlayerLeft[1];

                }
            }
            catch
            {
                // Close the connection and stream.
                receivePlayerLeft.Close();
                sendPlayerLeft.Close();
                playerLeft.Close();

                playerLeftConnected = false;
                Console.WriteLine("Player left has disconnected.");

                connected = false;

            }

        }
        //-------------------------------------------------------------------
        // Get messages from right player.
        private void GetMessagesRight()
        {
            string[] dataPlayerRight;

            try
            {
                while (connected)
                {

                    dataPlayerRight = receivePlayerRight.ReadLine().Split('|');
                    paddleRightX = dataPlayerRight[0];
                    paddleRightY = dataPlayerRight[1];

                }
            }
            catch
            {
                // Close the connection and stream.
                receivePlayerRight.Close();
                sendPlayerRight.Close();
                playerRight.Close();

                playerRightConnected = false;
                Console.WriteLine("Player right has disconnected.");

                connected = false;

            }

        }
        //-------------------------------------------------------------------
        // Send messages to the both players.
        private void SendMessages()
        {

            int seconds = DateTime.Now.Millisecond;

            while(true)
            {

                try
                {

                    // Send message after 30 milliseconds.
                    int delay = Math.Abs(DateTime.Now.Millisecond - seconds);
                    if (delay > 30)
                    {

                        if (playerLeftConnected == true && playerRightConnected == false)
                        {
                            sendPlayerLeft.WriteLine("Waiting");
                            sendPlayerLeft.Flush();
                        }

                        if (playerRightConnected == true && playerLeftConnected == false)
                        {
                            sendPlayerRight.WriteLine("Waiting");
                            sendPlayerRight.Flush();
                        }

                        if(playerLeftConnected == true && playerRightConnected == true && !connected)
                        {

                            sendPlayerLeft.WriteLine("Start");
                            sendPlayerLeft.Flush();

                            sendPlayerRight.WriteLine("Start");
                            sendPlayerRight.Flush();                      

                            connected = true;
                            counting = true;

                            ball.SetBallX(Settings.WIDTH / 2);
                            ball.SetBallY(Settings.HEIGHT / 2);

                            messagesPlayerLeft = new Thread(GetMessagesLeft);
                            messagesPlayerLeft.Start();

                            messagesPlayerRight = new Thread(GetMessagesRight);
                            messagesPlayerRight.Start();

                            seconds2 = DateTime.Now.Second;

                        }

                        if (connected)
                        {
                            // Wait three seconds before the game begins. 
                            if (counting)
                            {

                                int countingNumber = 3 - Math.Abs(DateTime.Now.Second - seconds2);
                                if (countingNumber == 0 || countingNumber < 0)
                                    counting = false;

                            }
                            else if (!counting)
                            {
                                // Moving ball.
                                int gameOver = ball.MovingBall(Convert.ToSingle(paddleLeftX), Convert.ToSingle(paddleLeftY), Convert.ToSingle(paddleRightX), Convert.ToSingle(paddleRightY));

                                string ballCoordinates = Convert.ToString(ball.GetBallX()) + '|' + Convert.ToString(ball.GetBallY());

                                
                                // Sending players paddle and ball coordinates.
                                if (playerLeft.Connected == true && paddleRightY != null)
                                {
                                    sendPlayerLeft.WriteLine(paddleRightY + '|' + ballCoordinates);
                                    sendPlayerLeft.Flush();
                                }
                                if (playerRight.Connected == true && paddleLeftY != null)
                                {
                                    sendPlayerRight.WriteLine(paddleLeftY + '|' + ballCoordinates);
                                    sendPlayerRight.Flush();
                                }
                          
                                
                                // Checking if one of players wins.
                                if (gameOver != 0)
                                {

                                    paddleRightY = Convert.ToString(Settings.HEIGHT / 2 - Settings.paddleSizeY / 2);
                                    paddleLeftY = Convert.ToString(Settings.HEIGHT / 2 - Settings.paddleSizeY / 2);

                                    ball.SetBallX(Settings.WIDTH / 2);
                                    ball.SetBallY(Settings.HEIGHT / 2);

                                    counting = true;

                                }


                                seconds2 = DateTime.Now.Second;
                            }

                        }

                        seconds = DateTime.Now.Millisecond;

                    }

                }
                // Occurs when one of the players disconnects during waiting for another player.
                catch
                {

                    if (playerLeftConnected == true)
                    {
                        // Close the connection and stream.
                        receivePlayerLeft.Close();
                        sendPlayerLeft.Close();
                        playerLeft.Close();

                        playerLeftConnected = false;
                        Console.WriteLine("Player left has disconnected 2.");

                    }                       

                    if (playerRightConnected == true)
                    {
                        // Close the connection and stream.
                        receivePlayerRight.Close();
                        sendPlayerRight.Close();
                        playerRight.Close();

                        playerRightConnected = false;
                        Console.WriteLine("Player right has disconnected 2.");

                    }

                    counting = true;

                }

            }

        }
        //-------------------------------------------------------------------
        // Waiting for player to connect.
        private void WaitingForPlayer()
        {

            while (true)
            {

                Console.WriteLine("Waiting for player...");
                TcpClient player = connections.AcceptTcpClient();

                // Player left connected.
                if (!playerLeftConnected)
                {

                    playerLeft = player;

                    // Player left joined the game.
                    receivePlayerLeft = new StreamReader(playerLeft.GetStream());
                    sendPlayerLeft = new StreamWriter(playerLeft.GetStream());

                    namePlayerLeft = receivePlayerLeft.ReadLine();
                    Console.WriteLine("Player left joined the game.");

                    sendPlayerLeft.WriteLine("Left");
                    sendPlayerLeft.Flush();

                    playerLeftConnected = true;
                }
                // Player right connected.
                else if (!playerRightConnected)
                {

                    playerRight = player;

                    // Player right joined the game.
                    receivePlayerRight = new StreamReader(playerRight.GetStream());
                    sendPlayerRight = new StreamWriter(playerRight.GetStream());

                    namePlayerRight = receivePlayerRight.ReadLine();
                    Console.WriteLine("Player right joined the game.");

                    sendPlayerRight.WriteLine("Right");
                    sendPlayerRight.Flush();

                    playerRightConnected = true;
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

                counting = true;

            }

        }
        //-------------------------------------------------------------------
    }
}
