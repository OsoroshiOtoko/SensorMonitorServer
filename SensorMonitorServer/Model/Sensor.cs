using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SensorMonitorServer
{
    internal class Sensor
    {
        public delegate void UpdateViewDelegate(string text);
        public static event UpdateViewDelegate? UpdateViewEvent;

        private static string _sensorType = "";
        private static string _sensorName = "";
        private static float[] _values = new float[] { 0 };
        public static string? sensorType 
        {
            get => _sensorType;
            set
            {
                _sensorType = value;
                UpdateViewEvent?.Invoke("sensorType");
            }
        }
        public static string? sensorName 
        {
            get => _sensorName;
            set
            {
                _sensorName = value;
                UpdateViewEvent?.Invoke("sensorName");
            }
        } 
        public static float[] values 
        {
            get => _values;               
            set
            {
                _values = value;
                UpdateViewEvent?.Invoke("values");
            }
        }

        public string GetValuesString()
        {
            string s = "  ";
            foreach (var v in values)
            {
                s += v.ToString() + " | ";
            }
            return s;
        }

        public float[] GetValue() => values;
    }
}
