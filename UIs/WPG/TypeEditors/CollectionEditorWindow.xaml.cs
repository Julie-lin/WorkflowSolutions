﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using WPG.Themes.TypeEditors;

namespace WPG.TypeEditors
{
    /// <summary>
    /// Interaction logic for CollectionEditor.xaml
    /// </summary>
    public partial class CollectionEditorWindow : Window
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
        public CollectionEditorWindow(CollectionEditorControl ctrl)
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


            //myList.ItemsSource = baseControl.NumerableValue;
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
            myGrid.Instance = null;
        }

        private void myLst_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            myGrid.Instance = myList.SelectedItem;
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            Type t = myList.Items[0].GetType();
            object newElem = System.Activator.CreateInstance(t);
            //object newElem = System.Activator.CreateInstance(baseControl.MyProperty.PropertyType.GetGenericArguments()[0]);
            myList.Items.Add(newElem);
        }




    }
}
