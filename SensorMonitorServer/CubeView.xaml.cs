using HelixToolkit.Wpf;
using SensorMonitorServer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SensorMonitorServer
{
    /// <summary>
    /// Логика взаимодействия для SensorView.xaml
    /// </summary>
    public partial class CubeView 
    {
        public CubeView()
        {
            InitializeComponent();

            DataContext = new CubeViewModel();

            var model3D = new ModelImporter().Load("F:/VSProjects/Git/SensorMonitorServer/SensorMonitorServer/object/cube.obj");
            modelVisual.Content = model3D;         
        }
    }
}
