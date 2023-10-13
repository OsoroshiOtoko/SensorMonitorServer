using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SensorMonitorServer.ViewModel
{
    internal class SensorViewModel : INotifyPropertyChanged
    {
        public static TcpClient? client;
        private static TcpListener? listener;
        private static IPAddress? address;
        private static Sensor? sensor;

        public delegate void UpdateNotificationDelegate(string text);
        public static event UpdateNotificationDelegate? UpdateNotificationEvent;
        public event PropertyChangedEventHandler? PropertyChanged;

        public string host { get; private set; }
        public string sensorType { get => "SensorType: " + sensor?.sensorType; }
        public string timeStap { get => "TimeStap: " + sensor?.time.ToString(); }
        public string values { get => "Values:" + sensor?.GetValuesString(); }

        public static float[] val = sensor?.values;


        public SensorViewModel()
        {
            address = Dns.GetHostAddresses(Dns.GetHostName()).Last(x => x.AddressFamily == AddressFamily.InterNetwork);

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(address.ToString()), 5656);
            listener = new TcpListener(ep);
            listener.Start();

            host = $"{ep.Address}:{ep.Port}";


            Task.Run(() =>
            {
                Exception? exc = null;
                while (true)
                {

                    client = listener.AcceptTcpClient();
                    if (client.Connected) UpdateNotificationEvent?.Invoke("Client connected!");

                    while (client.Connected)
                    {
                        try
                        {
                            byte[] lengthBytes = new byte[4];
                            client.GetStream().Read(lengthBytes, 0, 4);

                            int bufferLength = BitConverter.ToInt32(lengthBytes, 0);
                            byte[] buffer = new byte[bufferLength];
                            client.GetStream().Read(buffer, 0, bufferLength);

                            sensor = JsonSerializer.Deserialize<Sensor>(buffer);

                            val = sensor?.values;

                            OnPropertyChanged(nameof(sensorType));
                            OnPropertyChanged(nameof(timeStap));
                            OnPropertyChanged(nameof(values));
                        }
                        catch (Exception ex)
                        {
                            exc = ex;
                            UpdateNotificationEvent?.Invoke(ex.Message);
                            client.Dispose();
                            client.Close();
                            //UpdateNotificationEvent?.Invoke("Client disconnected!");
                        }
                    }
                    if (exc == null) UpdateNotificationEvent?.Invoke("Client disconnected!");
                }
            });
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));            
        }
    }
}
