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
        public double RotationAngleX
        {
            get { return _rotationAngleX; }
            set
            {
                _rotationAngleX = value;
                OnPropertyChanged(nameof(RotationAngleX));
            }
        }

        private double _rotationAngleY;
        public double RotationAngleY
        {
            get { return _rotationAngleY; }
            set
            {
                _rotationAngleY = value;
                OnPropertyChanged(nameof(RotationAngleY));
            }
        }

        private double _rotationAngleZ;
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
                    if (SensorViewModel.val?.Length >= 3)
                    {
                        RotationAngleX = SensorViewModel.val[0];
                        RotationAngleY = SensorViewModel.val[1];
                        RotationAngleZ = SensorViewModel.val[2];
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
