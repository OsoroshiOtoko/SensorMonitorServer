using SensorMonitorServer.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SensorMonitorServer
{
    /// <summary>
    /// Логика взаимодействия для SensorView.xaml
    /// </summary>
    public partial class SensorView : StackPanel
    {
        public Grid gridX = new Grid();
        public Grid gridY = new Grid();
        public Grid gridZ = new Grid();
        public Grid gridW = new Grid();

        public static ObservableCollection<float[]> valuesY = new ObservableCollection<float[]>();

        public SensorView()
        {
            InitializeComponent();
            DataContext = new SensorViewModel();

            
        }

        public void InitGraph()
        {
            gridX.Children.Clear();
            gridY.Children.Clear();
            gridZ.Children.Clear();
            gridW.Children.Clear();
            SView.Children.Add(Graph(gridX));
            SView.Children.Add(Graph(gridY));
            SView.Children.Add(Graph(gridZ));
            SView.Children.Add(Graph(gridW));
            Func(valuesY, gridX, 0);
            Func(valuesY, gridY, 1);
            Func(valuesY, gridZ, 2);
            Func(valuesY, gridW, 3);
        }

        private void browseBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            MessageBox.Show(button.Name);
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            try
            {
                StreamReader streamReader = new StreamReader(pathSensorData.Text);
                string json = streamReader.ReadToEnd();
                valuesY = JsonSerializer.Deserialize<ObservableCollection<float[]>>(json);
                InitGraph();
                MessageBox.Show("Load done !");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            try
            {
                string json = JsonSerializer.Serialize(valuesY);
                StreamWriter writer = new StreamWriter(pathSensorData.Text);
                writer.WriteLine(json);
                writer.Close();
                MessageBox.Show("Save done !");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public Grid Graph(Grid grid)
        {
            //Grid grid = new Grid();

            Rectangle fon = new Rectangle
            {
                Width = 500,
                Height = 200,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 5, 0, 5),
                Fill = Brushes.White
            };
            grid.Children.Add(fon);

            Polyline polyline = new Polyline
            {
                Points = new PointCollection() 
                { 
                    new Point(0, 5),
                    new Point(0, 205),
                    new Point(500, 205),
                    new Point(500, 5),
                    new Point(0, 5)
                },
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            grid.Children.Add(polyline);

            int i = 1;
            while (i<5)
            {
                Line line = new Line()
                {
                    X1 = 0,
                    Y1 = (i * 40) + 5,
                    X2 = 500,
                    Y2 = (i * 40) + 5,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 1
                };
                grid.Children.Add(line);
                i++;
            }

            return grid;
        }

        public void Func(ObservableCollection<float[]> val, Grid grid, int v)
        {

            int i = 0;
            float y = 102f;
            int x = 0;

            Polyline line = new Polyline
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 2
            };

            try
            {
                while (i < val.Count)
                {
                    float[] f = val[i];
                    x = i * (500 / val.Count);

                    line.Points.Add(new Point(x, y - f[v]));

                    i++;
                }
                grid.Children.Add(line);
            }
            catch (Exception ex) { }
        }
    }
}
