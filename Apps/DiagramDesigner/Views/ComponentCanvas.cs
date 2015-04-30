using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using Workflow.Data;


namespace DiagramDesigner.Views
{
    public class ComponentCanvas : Canvas
    {
        public event EventHandler ComponentItemChanged;
        private Point? rubberbandSelectionStartPoint = null;
        private List<ComponentNode> components;

        
        private DesignerItem _currentDesignItem;
        public ComponentCanvas()
        {
            _displayOnly = true;
            //this.Background = System.Windows.Media.GradientBrush; ;//.RelativeTransformProperty;
        }

        public void AddToChildren(Control item)
        {
            this.Children.Add(item);
            SetConnectorDecoratorTemplate((DesignerItem)item);
            
        }
        public void AddToConnectionChildren(Control item)
        {
            this.Children.Add(item);           
        }
        public void RemoveChild(DesignerItem child)
        {
            this.Children.Remove(child);
        }

        public void ClearComponent()
        {
            this.Children.Clear();
        }

        private bool _displayOnly;
        public bool DisplayOnly
        {
            set { _displayOnly = value; }
        }
    
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.Source == this)
            {
                // in case that this click is the start of a 
                // drag operation we cache the start point
                // var point = new Point?(e.GetPosition(this));

                foreach (Control item in Children)
                {
                    Rect itemRect = VisualTreeHelper.GetDescendantBounds(item);
                    Rect itemBounds = item.TransformToAncestor(this).TransformBounds(itemRect);
                    if (itemRect.Contains(e.GetPosition(this)) && item is DesignerItem)
                    {
                        item.Focus();
                    }
                    Focus();
                    e.Handled = true;
                }
            }

        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Source == this)
            {
                // in case that this click is the start of a 
                // drag operation we cache the start point
               // var point = new Point?(e.GetPosition(this));

                foreach (Control item in Children)
                {
                    Rect itemRect = VisualTreeHelper.GetDescendantBounds(item);
                    Rect itemBounds = item.TransformToAncestor(this).TransformBounds(itemRect);
                    if (itemRect.Contains(e.GetPosition(this)) && item is DesignerItem)
                    {
                        item.Focus();
                    }
                    Focus();
                    e.Handled = true;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!_displayOnly && e.Source is DesignerItem )
            {
                if (_currentDesignItem == null)
                {
                    _currentDesignItem = e.Source as DesignerItem;
                   // _currentDesignItem.ChangeColorToWhite();
                }

                if (((DesignerItem)e.Source) != _currentDesignItem)
                {
                    _currentDesignItem.RestoreOriginalColor();
                    _currentDesignItem = e.Source as DesignerItem;
                    _currentDesignItem.ChangeColorToWhite();

                    InvalidateVisual();
                    RaiseComponentItemChanged(new EventArgs());

                }
                //_currentDesignItem.BorderBrush = Brushes.BlueViolet;
                //_currentDesignItem.BorderThickness = new Thickness(10,10,10,10);
                
                //if (_currentDesignItem.Style.TargetType.GetType() == typeof(TextBox))
                //{
                //   // _currentDesignItem
                    
                //}
                
                //_currentDesignItem = "Orange";
                // in case that this click is the start of a 
                // drag operation we cache the start point
                // var point = new Point?(e.GetPosition(this));

                //foreach (Control item in Children)
                //{
                //    Rect itemRect = VisualTreeHelper.GetDescendantBounds(item);
                //    Rect itemBounds = item.TransformToAncestor(this).TransformBounds(itemRect);
                //    if (itemRect.Contains(e.GetPosition(this)) && item is DesignerItem)
                //    {
                //        item.Focus();
                //    }
                //    Focus();
                //    e.Handled = true;
                //}
            }


        }

        public void SetComponentNode(IEnumerable<XElement> itemsXML)
        {
            foreach (XElement itemXML in itemsXML)
            {
                Guid id = new Guid(itemXML.Element("ID").Value);
                DesignerItem item = DeserializeDesignerItem(itemXML, id, 0, 0);
                this.Children.Add(item);
                SetConnectorDecoratorTemplate(item);
            }
        }

        public void SetConnections(IEnumerable<XElement> connectionsXML)
        {
            foreach (XElement connectionXML in connectionsXML)
            {
                Guid sourceID = new Guid(connectionXML.Element("SourceID").Value);
                Guid sinkID = new Guid(connectionXML.Element("SinkID").Value);

                String sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
                String sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

                Connector sourceConnector = GetConnector(sourceID, sourceConnectorName);
                Connector sinkConnector = GetConnector(sinkID, sinkConnectorName);

                Connection connection = new Connection(sourceConnector, sinkConnector);
                Canvas.SetZIndex(connection, Int32.Parse(connectionXML.Element("zIndex").Value));
                this.Children.Add(connection);
            }
            this.InvalidateVisual();
        }
        private Connector GetConnector(Guid itemID, String connectorName)
        {
            DesignerItem designerItem = (from item in this.Children.OfType<DesignerItem>()
                                         where item.ID == itemID
                                         select item).FirstOrDefault();

            Control connectorDecorator = designerItem.Template.FindName("PART_ConnectorDecorator", designerItem) as Control;
            connectorDecorator.ApplyTemplate();

            return connectorDecorator.Template.FindName(connectorName, connectorDecorator) as Connector;
        }
        private static DesignerItem DeserializeDesignerItem(XElement itemXML, Guid id, double OffsetX, double OffsetY)
        {
            DesignerItem item = new DesignerItem(id);
            item.Width = Double.Parse(itemXML.Element("Width").Value, CultureInfo.InvariantCulture)/2;
            item.Height = Double.Parse(itemXML.Element("Height").Value, CultureInfo.InvariantCulture)/2;
            item.ParentID = new Guid(itemXML.Element("ParentID").Value);
            item.IsGroup = Boolean.Parse(itemXML.Element("IsGroup").Value);
            Canvas.SetLeft(item, (Double.Parse(itemXML.Element("Left").Value, CultureInfo.InvariantCulture) + OffsetX)/2);
            Canvas.SetTop(item, (Double.Parse(itemXML.Element("Top").Value, CultureInfo.InvariantCulture) + OffsetY)/2);
            Canvas.SetZIndex(item, Int32.Parse(itemXML.Element("zIndex").Value));
            Object content = XamlReader.Load(XmlReader.Create(new StringReader(itemXML.Element("Content").Value)));
            item.Content = content;
            return item;
        }
        private void SetConnectorDecoratorTemplate(DesignerItem item)
        {
            if (item.ApplyTemplate() && item.Content is UIElement)
            {
                ControlTemplate template = DesignerItem.GetConnectorDecoratorTemplate(item.Content as UIElement);
                Control decorator = item.Template.FindName("PART_ConnectorDecorator", item) as Control;
                if (decorator != null && template != null)
                    decorator.Template = template;
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();

            foreach (UIElement element in this.InternalChildren)
            {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;

                //measure desired size for each child
                element.Measure(constraint);

                Size desiredSize = element.DesiredSize;
                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
                {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }
            // add margin 
            size.Width += 10;
            size.Height += 10;
            return size;
        }

        private void RaiseComponentItemChanged(EventArgs args)
        {
            if (ComponentItemChanged != null)
            {
                CanvasDesignItemEventArg arg = new CanvasDesignItemEventArg()
                                                   {
                                                       ComponentName = _currentDesignItem.ThisComponent.ComponentName,
                                                       TypeName =  _currentDesignItem.ThisComponent.GetType().ToString()

                                                   };
                
                ComponentItemChanged(this, arg);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
