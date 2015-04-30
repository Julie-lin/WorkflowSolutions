using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using AppData;

using DiagramDesigner.ViewModels;
using Workflow.Data;


namespace DiagramDesigner
{
    public enum TabPageName
    {
        CanvasPage = 0,
        BatchSetupPage,
        SampleSetupPage,
    }
    public class SelectionService : INotifyPropertyChanged
    {
        private DesignerCanvas designerCanvas;

        private List<ISelectable> currentSelection;
        internal List<ISelectable> CurrentSelection
        {
            get
            {
                if (currentSelection == null)
                    currentSelection = new List<ISelectable>();

                return currentSelection;
            }
        }

        private TabPageName _currentSelectedPage = TabPageName.CanvasPage;
        public TabPageName CurrentSelectedPage
        {
            get { return _currentSelectedPage; }
            set
            {
                if (_currentSelectedPage != value)
                {
                    _currentSelectedPage = value;
                    OnPropertyChanged("CurrentSelectedPage");
                }
            }
        }

        private ComponentNode _currentComponentParam;
        public ComponentNode CurrentComponentParam
        {
            get { return _currentComponentParam; }
            set
            {
                if (_currentComponentParam != value)
                {
                    _currentComponentParam = value;
                    OnPropertyChanged("CurrentComponentParam");
                }
            }
            
        }

        public RawFileViewModel RawFileViewModel { get; set; }
        public SampleSetupViewModel SampleSetupViewModel { get; set; }

        public SelectionService(DesignerCanvas canvas)
        {
            this.designerCanvas = canvas;
            RawFileViewModel = new RawFileViewModel();

            SampleSetupViewModel = new SampleSetupViewModel();
        }


        
        internal void SelectItem(ISelectable item)
        {
            this.ClearSelection();
            this.AddToSelection(item);
        }

        internal void AddToSelection(ISelectable item)
        {
            if (item is IGroupable)
            {
                List<IGroupable> groupItems = GetGroupMembers(item as IGroupable);

                foreach (ISelectable groupItem in groupItems)
                {
                    groupItem.IsSelected = true;
                    CurrentSelection.Add(groupItem);
                    CurrentComponentParam = ((DesignerItem)item).ThisComponent;
                }
            }
            else
            {
                item.IsSelected = true;
                CurrentSelection.Add(item);
           
            }
        }

        internal void RemoveFromSelection(ISelectable item)
        {
            if (item is IGroupable)
            {
                List<IGroupable> groupItems = GetGroupMembers(item as IGroupable);

                foreach (ISelectable groupItem in groupItems)
                {
                    groupItem.IsSelected = false;
                    CurrentSelection.Remove(groupItem);
                }
            }
            else
            {
                item.IsSelected = false;
                CurrentSelection.Remove(item);
            }
        }

        internal void ClearSelection()
        {
            CurrentSelection.ForEach(item => item.IsSelected = false);
            CurrentSelection.Clear();
        }

        internal void SelectAll()
        {
            ClearSelection();
            CurrentSelection.AddRange(designerCanvas.Children.OfType<ISelectable>());
            CurrentSelection.ForEach(item => item.IsSelected = true);
        }

        internal List<IGroupable> GetGroupMembers(IGroupable item)
        {
            IEnumerable<IGroupable> list = designerCanvas.Children.OfType<IGroupable>();
            IGroupable rootItem = GetRoot(list, item);
            return GetGroupMembers(list, rootItem);
        }

        internal IGroupable GetGroupRoot(IGroupable item)
        {
            IEnumerable<IGroupable> list = designerCanvas.Children.OfType<IGroupable>();
            return GetRoot(list, item);
        }

        private IGroupable GetRoot(IEnumerable<IGroupable> list, IGroupable node)
        {
            if (node == null || node.ParentID == Guid.Empty)
            {
                return node;
            }
            else
            {
                foreach (IGroupable item in list)
                {
                    if (item.ID == node.ParentID)
                    {
                        return GetRoot(list, item);
                    }
                }
                return null;
            }
        }

        private List<IGroupable> GetGroupMembers(IEnumerable<IGroupable> list, IGroupable parent)
        {
            List<IGroupable> groupMembers = new List<IGroupable>();
            groupMembers.Add(parent);

            var children = list.Where(node => node.ParentID == parent.ID);

            foreach (IGroupable child in children)
            {
                groupMembers.AddRange(GetGroupMembers(list, child));
            }

            return groupMembers;
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
