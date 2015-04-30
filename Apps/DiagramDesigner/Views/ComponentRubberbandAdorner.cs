using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace DiagramDesigner.Views
{
    public class ComponentRubberbandAdorner : Adorner
    {
        private Point? _startPoint;
        private Point? _endPoint;

        private ComponentCanvas _canvas;
        public ComponentRubberbandAdorner(ComponentCanvas canvas, Point? dragStartPoint)
            : base(canvas)
        {
            _canvas = canvas;
            _startPoint = dragStartPoint;
        }

        private void GetComponentItem()
        {
            //_canvas.SelectionService.ClearSelection();

            //Rect rubberBand = new Rect(_startPoint.Value, _endPoint.Value);
            foreach (Control item in _canvas.Children)
            {
                Rect itemRect = VisualTreeHelper.GetDescendantBounds(item);
                Rect itemBounds = item.TransformToAncestor(_canvas).TransformBounds(itemRect);
            }
            //    if (itemBounds.Contains(_startPoint.Value))
            //    {
            //        return item;
            //    }

            //}
        }

    }
}
