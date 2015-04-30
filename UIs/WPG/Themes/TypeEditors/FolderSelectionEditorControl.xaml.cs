using System.Windows;
using System.Windows.Forms;
using AppData;
using Microsoft.Win32;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace WPG.Themes.TypeEditors
{
    using System;
    using System.Windows.Interop;

    
    /// <summary>
    /// Interaction logic for FolderSelectionEditorControl.xaml
    /// </summary>
    public partial class FolderSelectionEditorControl : UserControl
    {
        public ThermoFolderSelection FolderSelection
        {
            get { return (ThermoFolderSelection)GetValue(ThermoFolderSelectionProperty); }
            set { SetValue(ThermoFolderSelectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static DependencyProperty ThermoFolderSelectionProperty =
            DependencyProperty.Register("FolderSelection", typeof(ThermoFolderSelection), typeof(FolderSelectionEditorControl), 
            new UIPropertyMetadata(null));


        public FolderSelectionEditorControl()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog();

            System.Windows.Forms.DialogResult result = dlg.ShowDialog(this.GetIWin32Window());

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                FolderSelection.FolderName = dlg.SelectedPath;
            }

        }
        
    }
}
