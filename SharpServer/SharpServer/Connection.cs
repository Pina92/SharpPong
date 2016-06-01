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

        private string dataPlayer1, dataPlayer2;

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

            player1 = connections.AcceptTcpClient();
            Console.WriteLine("Player1 joined the game.");

            player2 = connections.AcceptTcpClient();
            Console.WriteLine("Player2 joined the game.");



            receivePlayer1 = new StreamReader(player1.GetStream());
            sendPlayer1 = new StreamWriter(player1.GetStream());

            receivePlayer2 = new StreamReader(player2.GetStream());
            sendPlayer2 = new StreamWriter(player2.GetStream());

            dataPlayer1 = receivePlayer1.ReadLine();
            dataPlayer2 = receivePlayer2.ReadLine();

            if (dataPlayer1 != "" && dataPlayer2 != "")
            {
                // 1 means connected successfully
                sendPlayer1.WriteLine("1");
                sendPlayer1.Flush();
                sendPlayer1.WriteLine("Left");
                sendPlayer1.Flush();
                sendPlayer2.WriteLine("1");
                sendPlayer2.Flush();
                sendPlayer2.WriteLine("Right");
                sendPlayer2.Flush();

            }


            messagesPlayer1 = new Thread(getMessages1);
            messagesPlayer1.Start();

            messagesPlayer2 = new Thread(getMessages2);
            messagesPlayer2.Start();

        }
        //-------------------------------------------------------------------
        public void getMessages1()
        {

            Console.WriteLine("Listening...");

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

            Console.WriteLine("Listening...");

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
