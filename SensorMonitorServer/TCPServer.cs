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
                while (listener != null)
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

                    Sensor.sensorName = JsonSerializer.Deserialize<string>(Read());
                    Sensor.sensorType = JsonSerializer.Deserialize<string>(Read());                    

                    while (client.Connected)
                    {
                        try 
                        { 
                            Sensor.values = JsonSerializer.Deserialize<float[]>(Read()); 
                        }
                        catch (Exception ex)
                        {
                            SensorViewModel.Toast(ex.Message);
                            await Task.Delay(1_000); //miliseconds
                            break;
                            //client.Dispose();
                            //client.Close();
                        }

                    }
                }                
            });

            return ep;
        }
        
        public static byte[] Read()
        {
            NetworkStream stream = client.GetStream();
            byte[] lengthBytes = new byte[4];
            int bufferLength;
            byte[] buffer;
            byte[] code = new byte[4];

            try
            {
                stream.Read(code, 0, 4);
                if (code[0] == 0) client.Close();
                switch (code[0]) 
                { 
                    case 7:
                        {
                            stream.WriteByte(8);

                            stream.Read(lengthBytes, 0, 4);
                            Task.Delay(3);
                            bufferLength = BitConverter.ToInt32(lengthBytes, 0);
                            buffer = new byte[bufferLength];
                            stream.Read(buffer, 0, bufferLength);
                            return buffer;
                        }
                    case 2:
                        {
                            return new byte[1] { 0 };
                        }
                }
            }
            catch (Exception ex)
            {
                SensorViewModel.Toast(ex.Message);
                stream.WriteByte(1);
                client.Dispose();
                client.Close();
            }
            return new byte[1] { 0 };
        }

        public static void ServerStop()
        {
            try
            {
                //client.GetStream().WriteByte(1);
                //client.Dispose();
                client.Close();
            }
            finally
            {
                listener.Stop();
            }
        }
    }
}
