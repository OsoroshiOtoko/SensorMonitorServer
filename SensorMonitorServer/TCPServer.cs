using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SensorMonitorServer.ViewModel;

namespace SensorMonitorServer
{
    internal class TCPServer
    {
        public static bool isConnected = false;

        public static TcpClient client;
        private static TcpListener listener;
        public static IPAddress address;

        public delegate void UpdateViewDelegate(string text);
        public static event UpdateViewDelegate? UpdateViewEvent;

        public static IPEndPoint ServerStart()
        {
            address = Dns.GetHostAddresses(Dns.GetHostName()).Last(x => x.AddressFamily == AddressFamily.InterNetwork);

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(address.ToString()), 5656);
            listener = new TcpListener(ep);
            listener.Start();

            Task.Run(async () => 
            {
                while (true)
                {
                    Sensor.sensorName = "";
                    Sensor.sensorType = "";
                    Sensor.values = new float[] { 0 };

                    if (client == null || !client.Connected)
                    {
                        SensorViewModel.Toast("Wait client connection...");
                        client = await listener.AcceptTcpClientAsync();
                    }
                    SensorViewModel.Toast("Client connected!");

                    int n = -2;
                    while (client.Connected && n < 0)
                    {
                        try
                        {
                            byte[] lengthBytes = new byte[4];
                            client.GetStream().Read(lengthBytes, 0, 4);

                            int bufferLength = BitConverter.ToInt32(lengthBytes, 0);
                            byte[] buffer = new byte[bufferLength];
                            client.GetStream().Read(buffer, 0, bufferLength);

                            if (bufferLength > 4)
                            {
                                switch (n)
                                {
                                    case -2:
                                        Sensor.sensorName = JsonSerializer.Deserialize<string>(buffer);
                                        n++;
                                        break;
                                    case -1:
                                        Sensor.sensorType = JsonSerializer.Deserialize<string>(buffer);
                                        n++;
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            SensorViewModel.Toast(ex.Message);
                            client.Dispose();
                            client.Close();
                        }
                    }

                    while (n > -1)
                    {
                        try
                        {
                            byte[] lengthBytes = new byte[4];
                            client.GetStream().Read(lengthBytes, 0, 4);
                            int bufferLength = BitConverter.ToInt32(lengthBytes, 0);
                            byte[] buffer = new byte[bufferLength];

                            client.GetStream().Read(buffer, 0, bufferLength);
                            Sensor.values = JsonSerializer.Deserialize<float[]>(buffer);
                        }
                        catch (Exception ex)
                        {
                            SensorViewModel.Toast(ex.Message);
                            await Task.Delay(10_000); //miliseconds
                            n = -2;
                            //client.Dispose();
                            //client.Close();
                        }
                    }
                }
            });

            return ep;
        }
        
    }
}
