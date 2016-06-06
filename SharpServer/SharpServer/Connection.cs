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

        //-------------------------------------------------------------------
        public Connection()
        {

        }
        //-------------------------------------------------------------------
        public void StartListening()
        {

            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            connections = new TcpListener(ipAddress, 8001);         
            connections.Start(); // Start listening at the specified port

            Console.WriteLine("Waiting for a connection...");
            
            // Player1 joined the game
            player1 = connections.AcceptTcpClient();

            receivePlayer1 = new StreamReader(player1.GetStream());
            sendPlayer1 = new StreamWriter(player1.GetStream());

            namePlayer1 = receivePlayer1.ReadLine();
            Console.WriteLine(namePlayer1 + " joined the game.");

            sendPlayer1.WriteLine("Left");
            sendPlayer1.Flush();

            // Player2 joined the game
            player2 = connections.AcceptTcpClient();

            receivePlayer2 = new StreamReader(player2.GetStream());
            sendPlayer2 = new StreamWriter(player2.GetStream());

            namePlayer2 = receivePlayer2.ReadLine();
            Console.WriteLine(namePlayer2 + " joined the game.");

            sendPlayer2.WriteLine("Right");
            sendPlayer2.Flush();

                     
            // 1 means both players are connected and the game can start
            sendPlayer1.WriteLine("1");
            sendPlayer1.Flush();

            sendPlayer2.WriteLine("1");
            sendPlayer2.Flush();


            Console.WriteLine("Listening...");

            messagesPlayer1 = new Thread(getMessages1);
            messagesPlayer1.Start();

            messagesPlayer2 = new Thread(getMessages2);
            messagesPlayer2.Start();

        }
        //-------------------------------------------------------------------
        public void getMessages1()
        {
            string dataPlayer1;

            while (true)
            {
                    dataPlayer1 = receivePlayer1.ReadLine();
                    sendPlayer2.WriteLine(dataPlayer1);
                    sendPlayer2.Flush();
            }
            
        }
        //-------------------------------------------------------------------
        public void getMessages2()
        {
            string dataPlayer2;

            while (true)
            {
                dataPlayer2 = receivePlayer2.ReadLine();
                sendPlayer1.WriteLine(dataPlayer2);
                sendPlayer1.Flush();
            }

        }
        //-------------------------------------------------------------------
    }
}
