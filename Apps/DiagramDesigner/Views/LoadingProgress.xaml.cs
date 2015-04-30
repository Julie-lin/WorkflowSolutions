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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DiagramDesigner.Views
{
    /// <summary>
    /// Interaction logic for LoadingProgress.xaml
    /// </summary>
    public partial class LoadingProgress : Window
    {
        private DispatcherTimer splashAnimationTimer;
        private const string Loading = "Loading";
        public LoadingProgress()
        {
            InitializeComponent();
            splashAnimationTimer = new DispatcherTimer();
            splashAnimationTimer.Interval = TimeSpan.FromMilliseconds(500);
            splashAnimationTimer.Tick += new EventHandler(splashAnimationTimer_Tick);
            splashAnimationTimer.Start();


            //m_mainWindow = new MainWindow();

            //m_mainWindow.ReadyToShow += new MainWindow.ReadyToShowDelegate(m_mainWindow_ReadyToShow);

            //m_mainWindow.Closed += new EventHandler(m_mainWindow_Closed);

        }
        void splashAnimationTimer_Tick(object sender, EventArgs e)
        {
            // Show the increasing amount of dots in "Loading...".

            int dotsCount = lblProgress.Content.ToString().Replace(Loading, string.Empty).Length;

            if (dotsCount < 6)
            {
                dotsCount++;
            }
            else
            {
                dotsCount = 0;
            }

            lblProgress.Content = Loading.PadRight(Loading.Length + dotsCount, '.');
        }
    }
}
