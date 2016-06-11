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
        private TcpListener connections; // Listens for connections from TCP network clients

        private StreamReader receivePlayer1, receivePlayer2;
        private StreamWriter sendPlayer1, sendPlayer2;

        private Thread messagesPlayer1, messagesPlayer2;

        private string namePlayer1, namePlayer2;

        bool connected;

        //-------------------------------------------------------------------
        public Connection()
        {
            this.connected = false;
            StartListening();
        }
        //-------------------------------------------------------------------
        public void StartListening()
        {

            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            connections = new TcpListener(ipAddress, 8001);
            connections.Start(); // Start listening at the specified port

            Console.WriteLine("Waiting for a connection...");
            waitingForPlayers();

        }
        //-------------------------------------------------------------------
        public void getMessages1()
        {
            string dataPlayer1;
            try
            {
                while (connected)
                {

                    dataPlayer1 = receivePlayer1.ReadLine();
                    if (player2.Connected == true)
                    {
                        sendPlayer2.WriteLine(dataPlayer1);
                        sendPlayer2.Flush();
                    }
                }
            }
            catch
            {
                // Close the connection and stream
                receivePlayer1.Close();
                sendPlayer1.Close();
                player1.Close();
                Console.WriteLine("Player1 has disconnected.");

                if (player2.Connected == true)
                {
                    sendPlayer2.WriteLine("Stop");
                    sendPlayer2.Flush();

                    connected = false;
                    waitingForPlayers();
                }
            }

        }
        //-------------------------------------------------------------------
        public void getMessages2()
        {
            string dataPlayer2;
            try
            {
                while (connected)
                {
                    dataPlayer2 = receivePlayer2.ReadLine();
                    if (player1.Connected == true)
                    {
                        sendPlayer1.WriteLine(dataPlayer2);
                        sendPlayer1.Flush();
                    }
                }
            }
            catch
            {
                // Close the connection and stream
                receivePlayer2.Close();
                sendPlayer2.Close();
                player2.Close();
                Console.WriteLine("Player2 has disconnected.");

                if (player1.Connected == true)
                {
                    sendPlayer1.WriteLine("Stop");
                    sendPlayer1.Flush();

                    connected = false;
                    waitingForPlayers();
                }

            }

        }
        //-------------------------------------------------------------------
        public void waitingForPlayers()
        {
            while (!connected)
            {
                if (player1 == null || player1.Connected == false)
               {
                    // Player1 joined the game
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
                    // Player2 joined the game
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
                    // Both players are connected and the game can start
                    sendPlayer1.WriteLine("Start");
                    sendPlayer1.Flush();

                    sendPlayer2.WriteLine("Start");
                    sendPlayer2.Flush();

                    Console.WriteLine("Listening...");
                    connected = true;

                    messagesPlayer1 = new Thread(getMessages1);
                    messagesPlayer1.Start();

                    messagesPlayer2 = new Thread(getMessages2);
                    messagesPlayer2.Start();
                   
               }

            }

        }
        //-------------------------------------------------------------------
    }
}
