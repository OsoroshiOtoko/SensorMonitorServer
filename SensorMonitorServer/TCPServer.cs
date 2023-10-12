using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SensorMonitorServer
{
    internal class TCPServer
    {/*
        public static TcpClient client;
        private static TcpListener listener;
        public static IPAddress address;

        static void Main(string[] args)
        {
            address = Dns.GetHostAddresses(Dns.GetHostName()).Last(x => x.AddressFamily == AddressFamily.InterNetwork);

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(address.ToString()), 5656);
            listener = new TcpListener(ep);
            listener.Start();
            
            //ep.Address, ep.Port
            client = listener.AcceptTcpClient();            


            while (client.Connected)
            {
                try
                {
                    byte[] lengthBytes = new byte[4];
                    client.GetStream().Read(lengthBytes, 0, 4);

                    int bufferLength = BitConverter.ToInt32(lengthBytes, 0);
                    byte[] buffer = new byte[bufferLength];
                    client.GetStream().Read(buffer, 0, bufferLength);

                    Sensor? sensor = JsonSerializer.Deserialize<Sensor>(buffer);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    client.Dispose();
                    client.Close();
                }
            }
        }*/

        

    }
}
