using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Thermo.Workflows.Contracts.RealTime;

namespace Thermo.Workflows.Contracts
{
    [DataContract]
    [XmlRoot]
    public class WorkItemProgressObserver : INotifyPropertyChanged, IServiceCallback,
        IObservable<WorkItemUpdateData>
    {
        private long _lastMessageIndex = -1;
        [DataMember]
        public long LastMessageIndex
        {
            get { return _lastMessageIndex; }
            set { _lastMessageIndex = value; }
        }


        private ObservableCollection<WorkItemWithHistory> _pendingWorkItems;
        private readonly IServiceCallback _callbackService;
        private object _sinkRoot = new object();
        [OnDeserialized]
        void OnDeserialized(StreamingContext c)
        {
            _sinkRoot = new object();
        }
        

        public WorkItemProgressObserver()
        {
            PendingWorkItems = new ObservableCollection<WorkItemWithHistory>();
        }

        [DataMember]
        [XmlArray]
        public ObservableCollection<WorkItemWithHistory> PendingWorkItems
        {
            get { return _pendingWorkItems; }
            set
            {
                if (value != _pendingWorkItems)
                {
                    _pendingWorkItems = value;
                    InvokePropertyChanged("PendingWorkItems");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void InvokePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        public WorkItemWithHistory AddWorkItemMessage(TaskProgressCallbackMessage value, 
            ObservableCollection<WorkItemWithHistory> parents, int inRouteIndex)
        {
            WorkItemId rootId = value.RouteToChild[inRouteIndex];
            WorkItemWithHistory parent = parents.FirstOrDefault(workItem => workItem.Id == rootId.Id);
            if (parent == null)
            {
                parent = new WorkItemWithHistory
                {
                    Id = rootId.Id,
                    Name = rootId.Name,
                    WorkItemType = rootId.WorkItemType
                };
                parents.Add(parent);
            }
            
            if (inRouteIndex != value.RouteToChild.Count - 1)
            {
                return AddWorkItemMessage(value, parent.Children, inRouteIndex + 1);
            }
            
            ProcessProgressMessage(value, parent);
            return parent;
        }

        private void ProcessProgressMessage(TaskProgressCallbackMessage value, WorkItemWithHistory parent)
        {
            if(value.MessageType == WorkItemStatus.Received) 
                return;

            if(value.MessageType == WorkItemStatus.Canceled)
            {
                CallbackMessageContent previousMessage = parent.GetLastUpdateMessage();
                if(previousMessage != null && previousMessage.MessageType == WorkItemStatus.Faulted) 
                    return;
            }

            UpdateParentDepthLevelAndInsert(parent, value);

        }

        private void UpdateParentDepthLevelAndInsert(WorkItemWithHistory parent, TaskProgressCallbackMessage value)
        {
            if ((value.MessageType == WorkItemStatus.Closed
                 || value.MessageType == WorkItemStatus.Canceled
                 || value.MessageType == WorkItemStatus.Faulted)
                 && parent.CrtDepthLevel > 0)
            {
                parent.CrtDepthLevel--;
            }

            CallbackMessageContent currentCallbackMessage = GetCallbackMessageContent(value, parent);
            int index;
            for (index = parent.MessageHistory.Count - 1; index >= 0; index -- )
            {
                if(parent.MessageHistory[index].RecordNumber < currentCallbackMessage.RecordNumber)
                {
                    break;
                }
            }
            parent.MessageHistory.Insert(index + 1, currentCallbackMessage);

            if(value.MessageType == WorkItemStatus.Executing)
            {
                parent.CrtDepthLevel++;
            }
        }

        private CallbackMessageContent GetCallbackMessageContent(TaskProgressCallbackMessage value, 
            WorkItemWithHistory parent)
        {
            return new CallbackMessageContent
                   {
                       RecordNumber = value.RecordNumber,
                       MessageType = value.MessageType,
                       ActivityName = value.ActivityName,
                       Message = value.Message,
                       TimeStamp = value.TimeStamp,
                       DepthLevel = parent.CrtDepthLevel
                   };
        }

        public static WorkItemProgressObserver FromFile(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(WorkItemProgressObserver));
            using (FileStream fileStream = File.Open(fileName, FileMode.Open))
            {
                return (WorkItemProgressObserver)serializer.Deserialize(fileStream);
            }
        }

        public void ToFile(string fileName)
        {
            XmlWriterSettings settings = new XmlWriterSettings {Indent = true};

            XmlWriter writer = XmlWriter.Create(fileName, settings);
            writer.WriteProcessingInstruction("xml-stylesheet", "type='text/xsl' href='WorkItemProgress.xslt'");

            XmlSerializer serializer = new XmlSerializer(typeof(WorkItemProgressObserver));
            serializer.Serialize(writer, this); 
            writer.Close();
        }

        public void WorkItemLogCallback(TaskProgressCallbackMessage callbackMessage, long messageIndex)
        {
            CheckMessageIndex(
                () =>
                    {
                        lock (_sinkRoot)
                        {
                            WorkItemWithHistory updatedItem = AddWorkItemMessage(callbackMessage, PendingWorkItems, 0);
                            NotifyProgress(new WorkItemUpdateData
                                           {
                                               WorkItemWithHistory = new WorkItemWithHistory
                                                                     {
                                                                         Name = updatedItem.Name,
                                                                         Id=updatedItem.Id,
                                                                         WorkItemType = updatedItem.WorkItemType,
                                                                         CrtDepthLevel = updatedItem.CrtDepthLevel,
                                                                         MessageHistory = new ObservableCollection<CallbackMessageContent>
                                                                             (updatedItem.MessageHistory)
                                                                     },
                                               ProgressCallbackMessage = callbackMessage
                                           });
                        }
                    },
                    messageIndex
                );
        }

        public void RegisterWorkItemsWithHistoryTree(WorkItemWithHistory lookupTree, long messageIndex)
        {
            CheckMessageIndex(
            () =>
                {
                    lock (_sinkRoot)
                    {
                        PendingWorkItems.Add(lookupTree);
                    }
                },
            messageIndex);
        }

        public void UnregisterWorkItemsWithHistoryTree(Guid workItemRootId, long messageIndex)
        {
            CheckMessageIndex(
            () =>
            {
                lock (_sinkRoot)
                {
                    PendingWorkItems.Remove(PendingWorkItems.First(workItemRoot => workItemRoot.Id == workItemRootId));
                }
            },
            messageIndex);
        }

        private void CheckMessageIndex(Action processMessage, long messageIndex)
        {
            if(messageIndex <= _lastMessageIndex) return;

            _lastMessageIndex = messageIndex;
            processMessage();
        }

        private List<ObserverSubscription> _observers;
        private List<ObserverSubscription> Observers
        {
            get
            {
                if(_observers == null)
                {
                    _observers = new List<ObserverSubscription>();
                }
                return _observers;
            }
        }

        public IDisposable Subscribe(IObserver<WorkItemUpdateData> observer)
        {
            ObserverSubscription observerSubscription = new ObserverSubscription(
                observer, Unsubscribe);
            Observers.Add(observerSubscription);
            return observerSubscription;
        }

        private void Unsubscribe(ObserverSubscription observerToUnsubscribe)
        {
            ObserverSubscription reference =
                (from observerReference in Observers
                 where observerReference == observerToUnsubscribe
                 select observerReference).First();
            Observers.Remove(reference);
        }

        private void NotifyProgress(WorkItemUpdateData message)
        {
            foreach (var observerReference in Observers)
            {
                ObserverSubscription reference = observerReference;
                Task.Factory.StartNew(() =>
                                      {
                                          try
                                          {
                                              reference.Observer.OnNext(message);
                                          }
                                          catch (Exception ex)
                                          {
                                              reference.Observer.OnError(ex);
                                          }
                                      });
            }
        }

        private class ObserverSubscription : IDisposable
        {
            private readonly Action<ObserverSubscription> _unsubscribe;
            public ObserverSubscription(
                IObserver<WorkItemUpdateData> observer,
                Action<ObserverSubscription> unsubscribe)
            {
                Observer = observer;
                _unsubscribe = unsubscribe;
            }

            public IObserver<WorkItemUpdateData> Observer { private set; get; }
            public void Dispose()
            {
                _unsubscribe(this);
            }
        }
    }
}
