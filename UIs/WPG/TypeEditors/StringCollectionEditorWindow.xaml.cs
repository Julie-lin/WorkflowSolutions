using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPG.Themes.TypeEditors;

namespace WPG.TypeEditors
{
    /// <summary>
    /// Interaction logic for StringCollectionEditorWindow.xaml
    /// </summary>
    public partial class StringCollectionEditorWindow : Window
    {
        public CollectionEditorControl baseControl { get; set; }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            List<object> newlist = new List<object>();
            foreach (var item in myList.Items)
            {
                newlist.Add(item);
            }
            baseControl.MyProperty.Value = newlist;
            base.OnClosing(e);
           
        }
        public bool HasDefaultConstructor(Type type)
        {
            if (type.IsValueType)
                return true;

            var constructor = type.GetConstructor(Type.EmptyTypes);

            if (constructor == null)
                return false;

            return true;
        }
        public StringCollectionEditorWindow(CollectionEditorControl ctrl)
        {
            InitializeComponent();
            baseControl = ctrl;

            foreach (var tmp in baseControl.NumerableValue)
            {
                myList.Items.Add(tmp);
            }

            //Visibilty of cmdAdd

            //var aa = baseControl.MyProperty.PropertyType.GetGenericArguments();
            if (!HasDefaultConstructor(baseControl.MyProperty.PropertyType.GetGenericArguments()[0]) || baseControl.MyProperty.IsReadOnly)
            {
                cmdAdd.Visibility = Visibility.Collapsed;
            }

            if (baseControl.MyProperty.IsReadOnly)
                cmdRemove.Visibility = Visibility.Collapsed;


        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmdRemove_Click(object sender, RoutedEventArgs e)
        {
            if (myList.SelectedItem != null)
            {
                myList.Items.Remove(myList.SelectedItem);
            }
            //
        }

        private void myLst_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //myGrid.Instance = myList.SelectedItem;
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            //Type t = myList.Items[0].GetType();
            //object newElem = System.Activator.CreateInstance(t);
            ////object newElem = System.Activator.CreateInstance(baseControl.MyProperty.PropertyType.GetGenericArguments()[0]);
            //myList.Items.Add(newElem);
        }

        private void myList_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (string fileName in files)
            {
                myList.Items.Add(fileName);
            }
            this.InvalidateVisual();

        }

    }
}
