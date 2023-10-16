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
using System.Collections;
using HelixToolkit.Wpf;
using System.Windows.Threading;
using System.Windows;

namespace SensorMonitorServer.ViewModel
{
    internal class SensorViewModel : INotifyPropertyChanged
    {
        private static Sensor sensor = new Sensor();


        public delegate void UpdateNotificationDelegate(string text);
        public static event UpdateNotificationDelegate? UpdateNotificationEvent;
        public event PropertyChangedEventHandler? PropertyChanged;

        public string host { get; private set; }

        public string sensorType { get => "SensorType: " + Sensor.sensorType; }
        public string sensorName { get => "SensorName: " + Sensor.sensorName; }
        public string values { get => "Values:" + sensor?.GetValuesString(); }



        public SensorViewModel()
        {

            IPEndPoint server = TCPServer.ServerStart();

            host = $"{server.Address}:{server.Port}";

            Sensor.UpdateViewEvent += OnPropertyChanged;

            //TCPServer.UpdateViewEvent += OnPropertyChanged;           

        }


        public static void Toast(string text) => UpdateNotificationEvent?.Invoke(text);

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));            
        }
    }
}
