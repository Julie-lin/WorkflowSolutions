using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using AppData;
using DiagramDesigner.ViewModels;
using Mneme.Data;
using IdNamePair = Mneme.Data.IdNamePair;


namespace DiagramDesigner.Views
{
    using DBConnectControl;

    /// <summary>
    /// Interaction logic for BatchSetupView.xaml
    /// </summary>
    public partial class BatchSetupView : UserControl
    {
        Point _lastMouseDown;
        IdNamePairProcessItem _draggedItem, _target;

        private DBConnectControl _dbConnectControl;


        private RawFileViewModel _viewMode;
        public BatchSetupView()
        {
            InitializeComponent();
            _dbConnectControl = new DBConnectControl();
            dbConnectionControl.Child = _dbConnectControl;
        }
        
        public void SetViewModel(RawFileViewModel model)
        {
            _viewMode = model;
            _viewMode.WorkflowChanged += this.WorkflowChanged;
            
        }

        public string GetEfConnectionString()
        {
            if (_dbConnectControl.EfConnectionStringBuilder == null) return string.Empty;
            return _dbConnectControl.EfConnectionStringBuilder.ConnectionString;
        }
        public string GetAdoConnectionString()
        {
            if (_dbConnectControl.BaseConnectionStringBuilder == null) return string.Empty;
            return _dbConnectControl.BaseConnectionStringBuilder.ConnectionString;
        }

        public string GetDbMachineName()
        {
            if (string.IsNullOrEmpty(_dbConnectControl.Server)) return string.Empty;
            return _dbConnectControl.Server.Split('\\')[0];
        }
        public void ClearCanvaschildren()
        {
          //  this.ComponentCanvas.ClearComponent();
        }
        public void AddDesignItems(List<DesignerItem> items )
        {
            foreach (var designerItem in items)
            {
                //this.ComponentCanvas.AddToChildren(designerItem);
            }
        }

        public void AddConnectionsToCanvas(IEnumerable<XElement> connectionsXML)
        {
           // this.ComponentCanvas.SetConnections(connectionsXML);

        }
        private void listBox_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void Measurement_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void listBox_Drop(object sender, DragEventArgs e)
        {
            TreeViewItem item = GetNearestContainer(e.OriginalSource as UIElement);
           // string[] files = (string[])e.Data.GetData(DataFormats., false);
            //foreach (string fileName in files)
            //{
            //    _viewMode.AddRawFileJobsToCurrentGroup(fileName);
            //}
            _viewMode.AddRawFileJobsToCurrentGroup(_draggedItem);

        }
        private void TreeView_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem workItemNode = GetParent<TreeViewItem>((DependencyObject)e.OriginalSource);

        }

        private ProcessItem _currentSelectdItem;
        private void TreeItemSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _currentSelectdItem = e.NewValue as ProcessItem;
            if (sender as TreeView == null) return;

            if (_currentSelectdItem.CrtDepthLevel == 1)
                _viewMode.CurrentGroup = _currentSelectdItem.Children;
            else
            {
               
            }

            //WorkItemProgressControl workItemProgressControl = GetParent<WorkItemProgressControl>((DependencyObject)sender);
        }
        private T GetParent<T>(DependencyObject sender) where T : UIElement
        {
            DependencyObject current = sender;
            while (current != null && !(current is T))
            {
                current = VisualTreeHelper.GetParent(current);
            }
            return (T)current;
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var items = ((ListBox)sender).SelectedItems;
                List<ProcessItem> list = new List<ProcessItem>();
                foreach (var item in items)
                {
                    list.Add((IdNamePairProcessItem)item);
                }
                foreach (var processItem in list)
                {
                    if (_viewMode.CurrentGroup != null)
                    {
                        _viewMode.RemoveItemFromCurrentGroup((ProcessItem)processItem);
                    }
                    
                }
                e.Handled = true;
            } 

        }

        private void MeasurementBox_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Delete)
            //{
            //    var item = (ProcessItem)((ListBox)sender).SelectedItem;
            //    if (_viewMode.CurrentGroup != null)
            //    {
            //        _viewMode.RemoveItemFromCurrentGroup(item);
            //    }
            //    e.Handled = true;
            //}

        }

        private void WorkflowChanged(object sender, EventArgs e)
        {
          //  this.ComponentCanvas.Children.Clear();
          ////  this.ComponentCanvas = ((CanvasElementEventArgs) e).Canvas;
          //  foreach (DesignerItem item in ((CanvasElementEventArgs)e).DesignItems)
          //  {
          //      ComponentCanvas.AddToChildren(item);
          //  }

          //  foreach (Connection connection in ((CanvasElementEventArgs)e).Connections)
          //  {
          //      ComponentCanvas.AddToConnectionChildren(connection);
          //  }
          //  ComponentCanvas.InvalidateVisual();

        }
        //private void DesignItemChanged(object sender, EventArgs e)
        //{
        //    this.ComponentCanvas.Children.Clear();
        //    foreach (DesignerItem item in ((CanvasElementEventArgs)e).DesignItems)
        //    {
        //        ComponentCanvas.AddToChildren(item);
        //    }

        //    ComponentCanvas.InvalidateVisual();
        //}

        private void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
            _viewMode.AddGroup();
        }
        private void AddJobButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewMode.CurrentGroup != null && 
                _currentSelectdItem != null &&
                _currentSelectdItem.CrtDepthLevel != 0)
            {
                _viewMode.AddJobToCurrentGroup();
            }
        }

        private void GetSummaryButton_Click(object sender, RoutedEventArgs e)
        {
            _viewMode.GetSummaryButton_Click(sender, e);
            //IRawDataSummary summary = new RawDataSummaryClient();
            //var measurements = summary.GetMeasurementNames();

            //ObservableCollection< Mneme.Data.IdNamePair> list = new ObservableCollection<IdNamePair>();
            //foreach (var v in measurements)
            //{
            //    list.Add(new Mneme.Data.IdNamePair()
            //        {
            //            Id = v.Id,
            //            Name = v.Name
            //        });
            //}
            //_viewMode.Measurements = list;
        }

        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point currentPosition = e.GetPosition(MeasurementTree);


                    if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                        (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                    {
                        _draggedItem = (IdNamePairProcessItem)MeasurementTree.SelectedItem;
                        if (_draggedItem != null)
                        {
                            DragDropEffects finalDropEffect = DragDrop.DoDragDrop(MeasurementTree, MeasurementTree.SelectedValue,
                                DragDropEffects.Move);
                            //Checking target is not null and item is dragging(moving)
                            if ((finalDropEffect == DragDropEffects.Move))// && (_target != null))
                            {
                                // A Move drop was accepted
                                //jlin
                               // if (!draggedItem.Header.ToString().Equals(_target.Header.ToString()))
                                //{
                                //    CopyItem(draggedItem, _target);
                                //    _target = null;
                                //    draggedItem = null;
                                //}

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            try
            {

                Point currentPosition = e.GetPosition(MeasurementTree);


                if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                    (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                {
                    // Verify that this is a valid drop and then store the drop target
                    TreeViewItem item = GetNearestContainer(e.OriginalSource as UIElement);
                    if (CheckDropTarget(_draggedItem, item))
                    {
                        e.Effects = DragDropEffects.Move;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.None;
                    }
                }
                e.Handled = true;
            }
            catch (Exception)
            {
            }
        }

        private TreeViewItem GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            TreeViewItem container = element as TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }
            return container;
        }
        private bool CheckDropTarget(IdNamePairProcessItem _sourceItem, TreeViewItem _targetItem)
        {
            //Check whether the target item is meeting your condition
            bool _isEqual = false;
            //if (!_sourceItem.Header.ToString().Equals(_targetItem.Header.ToString()))
            //{
            //    _isEqual = true;
            //}
            return _isEqual;

        }

        Dictionary<IdNamePairProcessItem, string> selectedItems =
            new Dictionary<IdNamePairProcessItem, string>();


        private void SummaryTreeItemSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem treeViewItem = MeasurementTree.SelectedItem as TreeViewItem;
            //Debug.WriteLine(treeViewItem.Name);

        }
        bool CtrlPressed
        {
            get
            {
                return System.Windows.Input.Keyboard.IsKeyDown(Key.LeftCtrl) ||
                    System.Windows.Input.Keyboard.IsKeyDown(Key.RightCtrl);
            }
        }
        bool ShiftAndDownPressed
        {
            get
            {
                return (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) ||
                    System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)) &&
                    System.Windows.Input.Keyboard.IsKeyDown(Key.Down);
            }
        }
        // deselects the tree item
        void Deselect(IdNamePairProcessItem treeViewItem)
        {
            //treeViewItem.Background = Brushes.White;// change background and foreground colors
            //treeViewItem.Foreground = Brushes.Black;
            selectedItems.Remove(treeViewItem); // remove the item from the selected items set
        }

        // changes the state of the tree item:
        // selects it if it has not been selected and
        // deselects it otherwise
        void ChangeSelectedState(IdNamePairProcessItem treeViewItem)
        {
            if (!selectedItems.ContainsKey(treeViewItem))
            { // select
                //treeViewItem.Background = Brushes.Black; // change background and foreground colors
                //treeViewItem.Foreground = Brushes.White;
                selectedItems.Add(treeViewItem, null); // add the item to selected items
            }
            else
            { // deselect
                Deselect(treeViewItem);
            }
        }

        public void StartProgressBar(double maxi)
        {
            SetProgressBar(maxi);
        }

        public void UpdateProgressBar(double value)
        {
            
        }
        public void ShowLoadingBar(System.Windows.Visibility v)
        {
            ProgressBar1.Visibility = v;
        }
        private double _progressValue = 0;
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        private void SetProgressBar(double maxi)
        {
            //Configure the ProgressBar
            ProgressBar1.Minimum = 0;
            ProgressBar1.Maximum = maxi;  //short.MaxValue;
            ProgressBar1.Value = 0;
            ProgressBar1.Visibility = System.Windows.Visibility.Visible;
            //Stores the value of the ProgressBar
            //double value = 0;

            //Create a new instance of our ProgressBar Delegate that points
            //  to the ProgressBar's SetValue method.
            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(ProgressBar1.SetValue);

            //Tight Loop:  Loop until the ProgressBar.Value reaches the max
            do
            {
                _progressValue += 1;

                /*Update the Value of the ProgressBar:
                  1)  Pass the "updatePbDelegate" delegate that points to the ProgressBar1.SetValue method
                  2)  Set the DispatcherPriority to "Background"
                  3)  Pass an Object() Array containing the property to update (ProgressBar.ValueProperty) and the new value */
                Dispatcher.Invoke(updatePbDelegate,
                    System.Windows.Threading.DispatcherPriority.Background,
                    new object[] { ProgressBar.ValueProperty, _progressValue });

            }
            while (ProgressBar1.Value != ProgressBar1.Maximum);

        }
        
    }
}
