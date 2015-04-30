using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DiagramDesigner.Views
{
    public class CollapsePanel : UserControl
    {
        // CONSTRUCTOR
        // --------------------------------------------------------------------------------
        public CollapsePanel()
        {
            Label = "Not Set.";
        }


        // METHODS
        // --------------------------------------------------------------------------------

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Set graphical border, if container is stacked then we don't want to duplicate border
            // thickness on the bottom of this container.
            Border border = GetTemplateChild("ContentBorder") as Border;
            if (Stacked)
                border.BorderThickness = new Thickness(1, 0, 1, 0);
            else
                border.BorderThickness = new Thickness(1, 0, 1, 1);

            // Set Collapse icon visibility if Collapsable is false, don't show the collapse icon.
            Image collapseIcon = GetTemplateChild("CollapseIcon") as Image;
            //if (Collapsable)
            //    collapseIcon.Visibility = ((UIElement) this).Visibility.Visible;
            //else
            //    collapseIcon.Visibility = ((UIElement) this).Visibility.Collapsed;


        }


        // DEPENDENCY PROPERTIES
        // --------------------------------------------------------------------------------

        // Stacked
        public static readonly DependencyProperty StackedProperty =
            DependencyProperty.Register("Stacked", typeof(Boolean), typeof(CollapsePanel), new PropertyMetadata(false));

        public Boolean Stacked
        {
            get { return (Boolean)GetValue(StackedProperty); }
            set { SetValue(StackedProperty, value); }
        }

        // Label
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(String), typeof(CollapsePanel), null);

        public String Label
        {
            get { return GetValue(LabelProperty) as String; }
            set { SetValue(LabelProperty, value); }
        }

        // Collapsable
        public static readonly DependencyProperty CollapsableProperty =
            DependencyProperty.Register("Collapsable", typeof(Boolean), typeof(CollapsePanel), new PropertyMetadata(true));

        public Boolean Collapsable
        {
            get { return (Boolean)GetValue(CollapsableProperty); }
            set { SetValue(CollapsableProperty, value); }
        }

        //ShowWarning
        public static readonly DependencyProperty ShowWarningProperty =
            DependencyProperty.Register("ShowWarning", typeof(Boolean), typeof(CollapsePanel), new FrameworkPropertyMetadata(
                false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange,
                null));



        public Boolean ShowWarning
        {
            get { return (Boolean)GetValue(ShowWarningProperty); }
            set { SetValue(ShowWarningProperty, value); }
        }

        // IsCollapsed
        public static readonly DependencyProperty IsCollapsedProperty =
            DependencyProperty.Register("IsCollapsed", typeof(Boolean), typeof(CollapsePanel), new PropertyMetadata(false, new PropertyChangedCallback(OnIsCollapsedChanged)));

        public Boolean IsCollapsed
        {
            get { return (Boolean)GetValue(IsCollapsedProperty); }
            set { SetValue(IsCollapsedProperty, value); }
        }


        private static void OnIsCollapsedChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            CollapsePanel control = (CollapsePanel)d;
            RoutedPropertyChangedEventArgs<Boolean> e = new RoutedPropertyChangedEventArgs<Boolean>(
               (Boolean)args.OldValue, (Boolean)args.NewValue, IsCollapsedChangedEvent);
            control.OnCollapsedChanged(e);
        }

        // ROUTED EVENTS
        // --------------------------------------------------------------------------------

        /// <summary>
        /// Identifies the CollapsedChanged routed event.
        /// </summary>
        public static readonly RoutedEvent IsCollapsedChangedEvent = EventManager.RegisterRoutedEvent(
            "CollapsedChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<Boolean>), typeof(CollapsePanel));

        /// <summary>
        /// Occurs when the IsCollapsed property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<Boolean> CollapsedChanged
        {
            add { AddHandler(IsCollapsedChangedEvent, value); }
            remove { RemoveHandler(IsCollapsedChangedEvent, value); }
        }

        /// <summary>
        /// Raises the CollapsedChanged event.
        /// </summary>
        /// <param name="args">Arguments associated with the CollapsedChanged event.</param>
        public virtual void OnCollapsedChanged(RoutedPropertyChangedEventArgs<bool> args)
        {
            RaiseEvent(args);
        }


    }   // class
}
