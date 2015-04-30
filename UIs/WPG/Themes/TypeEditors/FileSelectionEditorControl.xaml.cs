using System.Windows;
using System.Windows.Controls;
using AppData;
using Microsoft.Win32;

namespace WPG.Themes.TypeEditors
{
    /// <summary>
    /// Interaction logic for FileSelectionEditorControl.xaml
    /// </summary>
    public partial class FileSelectionEditorControl : UserControl
    {
        public ThermoFileSelection FileSelection
        {
            get { return (ThermoFileSelection)GetValue(ThermoFileSelectionProperty); }
            set { SetValue(ThermoFileSelectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static DependencyProperty ThermoFileSelectionProperty =
            DependencyProperty.Register("FileSelection", typeof(ThermoFileSelection), typeof(FileSelectionEditorControl), 
            new UIPropertyMetadata(null));

        public FileSelectionEditorControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string ext = FileSelection.Extension;
            string filter = string.Format("Designer Files (*.{0})|*.{1}|All Files (*.*)|*.*", FileSelection.Extension,
                          FileSelection.Extension);
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = filter;

            if (openFile.ShowDialog() == true)
            {
                FileSelection.FileName = openFile.FileName;
            }
           
        }
    }
}
