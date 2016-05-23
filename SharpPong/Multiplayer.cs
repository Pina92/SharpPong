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

        public Multiplayer()
        {
            startConnection();
        }


        public override void moveOpponent()
        {

        }

        // Player informations
        private string userName = "Unknown";

        private TcpClient player;
        private StreamWriter send;
        private StreamReader receive;

        private Thread messaging, sending;
        //------------------------------------------------------------------------
        public void startConnection()
        {
            // Nawiazanie polaczenia z serverem
            player = new TcpClient();
            player.Connect("192.168.0.108", 8001);

            send = new StreamWriter(player.GetStream());

            // Sending username to server
            userName = "Paulina";
            send.WriteLine(userName);
            send.Flush();

            // Getting messages from server 
            messaging = new Thread(new ThreadStart(getMessage));
            messaging.Start();

            // Sending messages to server
            sending = new Thread(new ThreadStart(sendMessage));
            sending.Start();

        }
        //------------------------------------------------------------------------
        private void getMessage()
        {
            receive = new StreamReader(player.GetStream());
            string serverMessage = receive.ReadLine();

            // Connection was successful
            if (serverMessage == "1")
            {

                while (true)
                {
                    // Receiving and updating the position of the opponent paddle
                    string[] coordinations = receive.ReadLine().Split(' ');
                    float coordinationX = Convert.ToUInt64(coordinations[0]);
                    float coordinationY = Convert.ToUInt64(coordinations[1]);

                    Console.WriteLine(coordinationX + " " + coordinationY);

                    paddleR.Position = new Vector2f(paddleR.Position.X, coordinationY);

                }

            }

        }
        //------------------------------------------------------------------------
        private void sendMessage()
        {
            // Send messages to server
            while (true)
            {
                // Sending the position of the player paddle
                string message = paddleL.Position.X.ToString() + " " + paddleL.Position.Y.ToString();
                send.WriteLine(message);
                send.Flush();

            }

        }
        //------------------------------------------------------------------------


    }
}
