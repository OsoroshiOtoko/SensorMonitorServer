using HelixToolkit.Wpf;
using SensorMonitorServer.ViewModel;
using System.IO;
using Path = System.IO.Path;

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

            var u = Directory.GetCurrentDirectory();
            var model3D = new ModelImporter().Load(Path.Combine(Directory.GetCurrentDirectory(), "cube.obj"));
            modelVisual.Content = model3D;         
        }
    }
}
