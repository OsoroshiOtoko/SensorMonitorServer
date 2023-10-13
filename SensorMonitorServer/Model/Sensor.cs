using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SensorMonitorServer
{
    internal class Sensor
    {
        public string? sensorType { get; set; }
        public long time { get; set; } 
        public float[] values { get; set; }

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
