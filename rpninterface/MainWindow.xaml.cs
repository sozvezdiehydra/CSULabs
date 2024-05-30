using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Wpf;
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
            //double inputX = 1; //double.Parse(tbInputX.Text);
            double stepX = double.Parse(tbStep.Text);
            double stepFrom = double.Parse(tbFrom.Text);
            double stepTo = double.Parse(tbTo.Text);


            // Create a line series to represent the function
            var lineSeries = new LineSeries
            {
                StrokeThickness = 2,
                //MarkerType = MarkerType.Star,
                //MarkerSize = 4,
                //MarkerStroke = OxyColors.Red,
                Color = OxyColors.Black
            };

            for (double x = stepFrom; x <= stepTo; x+=stepX)
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
                AxislineColor = OxyColors.Red,
                Maximum = 40,
                Minimum = -40,
                PositionAtZeroCrossing = true,
                TickStyle = TickStyle.Crossing,
                IsAxisVisible = true,
            };
            plotModel.Axes.Add(linearAxis);

            var secondLinearAxis = new LinearAxis
            {
                Title = "X",
                AxislineColor = OxyColors.Red,
                Maximum = 40,
                Minimum = -40,
                PositionAtZeroCrossing = true,
                Position = AxisPosition.Bottom,
                TickStyle = TickStyle.Crossing,
                IsAxisVisible = true,
            };
            plotModel.Axes.Add(secondLinearAxis);
            plotView.Model = plotModel;
        }
    }
}

