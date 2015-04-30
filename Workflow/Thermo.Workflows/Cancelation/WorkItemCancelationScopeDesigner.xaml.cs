using System;
using System.Activities;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Thermo.Workflows.Cancelation
{
    // Interaction logic for WorkItemCancelationScopeDesigner.xaml
    public partial class WorkItemCancelationScopeDesigner
    {
        public WorkItemCancelationScopeDesigner()
        {
            InitializeComponent();
        }

        //protected override void OnModelItemChanged(object newItem)
        //{
        //    if (this.ModelItem.Properties["Action"].Value == null)
        //    {
        //        this.ModelItem.Properties["Action"].SetValue(
        //            new ActivityAction());
        //    }
        //    if (this.ModelItem.Properties["Cancelation"].Value == null)
        //    {
        //        this.ModelItem.Properties["Cancelation"].SetValue(
        //            new ActivityAction());
        //    }
        //    base.OnModelItemChanged(newItem);
        //}
    }
}
