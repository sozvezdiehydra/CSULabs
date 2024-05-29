using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using RpnLogic;
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
using System.Net.WebSockets;
using OxyPlot.Wpf;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace rpninterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void drawGraphic(object sender, RoutedEventArgs e)
        {
            string input = tbInput.Text;
            int inputX = int.Parse(tbInputX.Text);

            // Create a line series to represent the function
            var lineSeries = new LineSeries
            {
                StrokeThickness = 2.3,
                //MarkerType = MarkerType.Star,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Black
            };

            // Generate points for the function
            for (int x = -70; x <= 70; x++)
            {
                double y = new RpnCalculator(input, x).Result;
                lineSeries.Points.Add(new DataPoint(x, y));
            }


            var plotModel = new PlotModel();
            plotModel.Series.Add(lineSeries);
            plotModel.PlotAreaBorderThickness = new OxyThickness(0.5);
            plotModel.PlotMargins = new OxyThickness(30);

            var linearAxis = new LinearAxis
            {
                Title = "Y",
                AxislineColor = OxyColors.Black,
                Maximum = 40,
                Minimum = -40,
                PositionAtZeroCrossing = true,
                TickStyle = TickStyle.Crossing
            };
            plotModel.Axes.Add(linearAxis);

            var secondLinearAxis = new LinearAxis
            {
                Title = "X",
                AxislineColor = OxyColors.Black,
                Maximum = 40,
                Minimum = -40,
                PositionAtZeroCrossing = true,
                Position = AxisPosition.Bottom,
                TickStyle = TickStyle.Crossing
            };
            plotModel.Axes.Add(secondLinearAxis);
            plotView.Model = plotModel;

        }
    }
}

