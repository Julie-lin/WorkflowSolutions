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

namespace DiagramDesigner.Views
{
    /// <summary>
    /// Interaction logic for BatchNameWindow.xaml
    /// </summary>
    public partial class BatchNameWindow : Window
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public BatchNameWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Name = this.batchName.Text;
            Description = this.batchDescription.Text;
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
