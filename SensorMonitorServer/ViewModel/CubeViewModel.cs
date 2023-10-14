using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SensorMonitorServer.ViewModel
{
    internal class CubeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private double _rotationAngleX;
        private double _rotationAngleY;
        private double _rotationAngleZ;

        public double RotationAngleX
        {
            get { return _rotationAngleX; }
            set
            {
                _rotationAngleX = value;
                OnPropertyChanged(nameof(RotationAngleX));
            }
        }
        
        public double RotationAngleY
        {
            get { return _rotationAngleY; }
            set
            {
                _rotationAngleY = value;
                OnPropertyChanged(nameof(RotationAngleY));
            }
        }
        
        public double RotationAngleZ
        {
            get { return _rotationAngleZ; }
            set
            {
                _rotationAngleZ = value;
                OnPropertyChanged(nameof(RotationAngleZ));
            }
        }

        public CubeViewModel()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    float[] rot = Sensor.values;
                    if (rot.Length >= 3)
                    {
                        RotationAngleX = rot[0];
                        RotationAngleY = rot[1];
                        RotationAngleZ = rot[2];
                    }
                }
            });
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
