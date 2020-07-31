using System.Windows;
using TrafficLights.Code;
using TrafficLights.FiniteStateMachine;

namespace TrafficLights
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Pelican _pelican = new Pelican();

        public MainWindow()
        {
            InitializeComponent();

            _pelican[PelicanLights.Red].Image = imgRed;
            _pelican[PelicanLights.Amber].Image = imgAmber;
            _pelican[PelicanLights.Green].Image = imgGreen;

            _pelican[PelicanLights.RedFigure].Image = imgRedFigure;
            _pelican[PelicanLights.GreenFigure].Image = imgGreenFigure;
            
            _pelican[PelicanLights.Wait].Image = imgWait;
        }

        private void WaitClick(object sender, RoutedEventArgs e)
        {
            _pelican.Press();
        }
    }
}
