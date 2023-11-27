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
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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

        public Grid Graph()
        {
            Grid grid = new Grid();
            Line vertL = new Line();
            vertL.X1 = 10;
            vertL.Y1 = 150;
            vertL.X2 = 10;
            vertL.Y2 = 10;
            vertL.Stroke = Brushes.Black;
            grid.Children.Add(vertL);
            Line horL = new Line();
            horL.X1 = 10;
            horL.X2 = 150;
            horL.Y1 = 150;
            horL.Y2 = 150;
            horL.Stroke = Brushes.Black;
            grid.Children.Add(horL);
            for (byte i = 2; i < 14; i++)
            {
                Line a = new Line();
                a.X1 = i * 10;
                a.X2 = i * 10;
                a.Y1 = 155;
                a.Y2 = 145;
                a.Stroke = Brushes.Black;
                grid.Children.Add(a);
            }
            for (byte i = 2; i < 14; i++)
            {
                Line a = new Line();
                a.X1 = 5;
                a.X2 = 15;
                a.Y1 = i * 10;
                a.Y2 = i * 10;
                a.Stroke = Brushes.Black;
                grid.Children.Add(a);
            }
            Polyline vertArr = new Polyline();
            vertArr.Points = new PointCollection();
            vertArr.Points.Add(new Point(5, 15));
            vertArr.Points.Add(new Point(10, 10));
            vertArr.Points.Add(new Point(15, 15));
            vertArr.Stroke = Brushes.Black;
            grid.Children.Add(vertArr);
            Polyline horArr = new Polyline();
            horArr.Points = new PointCollection();
            horArr.Points.Add(new Point(145, 145));
            horArr.Points.Add(new Point(150, 150));
            horArr.Points.Add(new Point(145, 155));
            horArr.Stroke = Brushes.Black;
            grid.Children.Add(horArr);

            return grid;
        }

        public static void Toast(string text) => UpdateNotificationEvent?.Invoke(text);

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));            
        }
    }
}
