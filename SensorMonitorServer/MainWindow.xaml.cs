using SensorMonitorServer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SensorMonitorServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            sensor.Content = new SensorView();
            box.Content = new CubeView();

            SensorViewModel.UpdateNotificationEvent += (text) =>
            {
                Dispatcher.Invoke(() =>
                {
                    toast.Text = text;
                });
            };

            Closing += (sender, e) =>
            {
                TCPServer.ServerStop();
            };
        }
    }
}
