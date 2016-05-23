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

        private Thread listener, messages;

        private string dataPlayer1, dataPlayer2;

        //-------------------------------------------------------------------
        public Connection()
        {

        }
        //-------------------------------------------------------------------
        public void StartListening()
        {

            IPAddress ipAddress = IPAddress.Parse("192.168.0.108");
            connections = new TcpListener(ipAddress, 8001);         
            connections.Start(); // Start listening at the specified port

            Console.WriteLine("Waiting for a connection...");

            // Start the new thread that hosts the listener
            listener = new Thread(KeepListening);
            listener.Start();

        }
        //-------------------------------------------------------------------
        public void KeepListening()
        {

            while (true)
            {

                player1 = connections.AcceptTcpClient();
                Console.WriteLine("Player1 joined the game.");
                player2 = connections.AcceptTcpClient();
                Console.WriteLine("Player2 joined the game.");

                messages = new Thread(getMessages);
                messages.Start();
 
            }

        }
        //-------------------------------------------------------------------
        public void getMessages()
        {

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
                sendPlayer2.WriteLine("1");
                sendPlayer2.Flush();


                Console.WriteLine(dataPlayer1 + " " + dataPlayer2);
            }

            Console.WriteLine("Listening...");

            while (true)
            {

                dataPlayer1 = receivePlayer1.ReadLine();
                Console.WriteLine(dataPlayer1);

                dataPlayer2 = receivePlayer2.ReadLine();
                Console.WriteLine(dataPlayer2);

                sendPlayer1.WriteLine(dataPlayer2);
                sendPlayer1.Flush();
                sendPlayer2.WriteLine(dataPlayer1);
                sendPlayer2.Flush();

            }
            
        }
        //-------------------------------------------------------------------
    }
}
