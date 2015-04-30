using System;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.Threading;
using Thermo.Data.Hierarchical;
using Thermo.Workflows.Contracts;
using Thermo.Workflows.Contracts.RealTime;

namespace AcquisitionActivities.RealTime
{
    public class ProgressTrackingParticipant : TrackingParticipant
    {
        private long _messageIndex = 0;
        private Func<Exception, string> _formatError;
        readonly ActiveWorkItemsTracingLogs _activeWorkItemsLogs;
        private Action<Guid, WorkItemWithHistory> _onInstanceRemoved;

        public WorkItemProgressObserver WorkItemProgressObserver
        {
            get { return _activeWorkItemsLogs._workItemProgressObserver; }
        }

        public IServiceCallback ProgressObserver { get; set; }

        public ProgressTrackingParticipant(Func<Exception, string> formatError,
            Action<Guid, WorkItemWithHistory> onInstanceRemoved)
        {
            _activeWorkItemsLogs = new ActiveWorkItemsTracingLogs();
            InitializeTrackingProfile();
            _formatError = formatError;
            _onInstanceRemoved = onInstanceRemoved;
        }

        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            lock (this)
            {
                WorkflowInstanceUnhandledExceptionRecord unhandledExceptionRecord =
                    record as WorkflowInstanceUnhandledExceptionRecord;
                if(unhandledExceptionRecord != null)
                {
                    ProcessInstRecord(unhandledExceptionRecord);
                    Console.WriteLine("Exception executing workflow " + unhandledExceptionRecord);
                    return;
                }

                WorkflowInstanceRecord wfInstRecord = record as WorkflowInstanceRecord;
                if(wfInstRecord != null)
                {
                    ProcessInstRecord(wfInstRecord);
                    return;
                }

                ActivityStateRecord activityStateRecord = record as ActivityStateRecord;
                if(activityStateRecord != null)
                {
                    ProcessActivityStateRecord(activityStateRecord);
                    return;
                }

                FaultPropagationRecord faultPropagationRecord = record as FaultPropagationRecord;
                if(faultPropagationRecord != null)
                {
                    ProcessFaultPropagationRecord(faultPropagationRecord);
                    return;
                }

                CustomTrackingRecord customTrackingRecord = record as CustomTrackingRecord;
                if(customTrackingRecord != null)
                {
                    ProcessCustomTrackingRecord(customTrackingRecord);
                    return;
                }
            }
        }

        private void ProcessCustomTrackingRecord(CustomTrackingRecord customTrackingRecord)
        {
            if(customTrackingRecord.Name == WorkItemStatus.Received)
            {
                var lookupTree = customTrackingRecord.GetData<WorkItemWithHistory>(CustomProgressTrackingDataKey.LookupTree);
                _activeWorkItemsLogs.RegisterWorkflowInstanceWithLookupTree(
                    customTrackingRecord.InstanceId, lookupTree, _messageIndex++);

                if(ProgressObserver != null)
                {
                    ProgressObserver.RegisterWorkItemsWithHistoryTree(lookupTree, _messageIndex);
                }
            }
            else
            {
                TaskProgressCallbackMessage progressCallbackMessage = ToProgressCallbackMessage(customTrackingRecord);
                _activeWorkItemsLogs.PushMessage(customTrackingRecord.InstanceId, progressCallbackMessage, _messageIndex++);

                if(ProgressObserver != null)
                {
                    ProgressObserver.WorkItemLogCallback(progressCallbackMessage, _messageIndex);
                }
            }
        }

        private static TaskProgressCallbackMessage ToProgressCallbackMessage(CustomTrackingRecord customTrackingRecord)
        {
            var target = customTrackingRecord.GetData<IWorkItemWithChildren>(CustomProgressTrackingDataKey.Target);
            return new TaskProgressCallbackMessage
                       {
                           MessageType = customTrackingRecord.Name,
                           ActivityName = customTrackingRecord.Activity.Name,
                           Message = customTrackingRecord.GetDataOrDefault<string>(CustomProgressTrackingDataKey.Message),
                           TimeStamp = customTrackingRecord.EventTime.ToLocalTime(),
                           RecordNumber = customTrackingRecord.RecordNumber,
                           RouteToChild = new List<WorkItemId>
                                              {
                                                  new WorkItemId
                                                      {
                                                          Id = target.Id,
                                                          Name = target.Name,
                                                          WorkItemType = target.WorkItemType
                                                      }
                                              }
                       };
        }

        private void ProcessInstRecord(WorkflowInstanceRecord wfInstRecord)
        {
            if(wfInstRecord.State == WorkflowInstanceStates.Completed
                || wfInstRecord.State == WorkflowInstanceStates.UnhandledException
                || wfInstRecord.State == WorkflowInstanceStates.Terminated
                || wfInstRecord.State == WorkflowInstanceStates.Aborted)
            {
                WorkItemWithHistory rootWorkItemForWorkflowInstance =
                    _activeWorkItemsLogs.GetRootWorkItemForWorkflowInstance(wfInstRecord.InstanceId);
                if (rootWorkItemForWorkflowInstance == null) return;

                Guid workItemRootId = rootWorkItemForWorkflowInstance.Id;

                if (_onInstanceRemoved != null)
                {
                    _onInstanceRemoved(workItemRootId, new WorkItemWithHistory());
                }

                _activeWorkItemsLogs.UnregisterWorkflowInstance(wfInstRecord.InstanceId, _messageIndex++);

                if (ProgressObserver != null)
                {
                    //jlin may need to turn on 
                   // ProgressObserver.UnregisterWorkItemsWithHistoryTree(workItemRootId, _messageIndex);
                }
            }
        }

        private readonly Dictionary<String, TaskProgressCallbackMessage> _pendingFaultedMessages = 
            new Dictionary<string, TaskProgressCallbackMessage>();

        private static string FautedMessageKey(ActivityInfo activityInfo)
        {
            return activityInfo.ToString();
        }
        private void ProcessFaultPropagationRecord(FaultPropagationRecord faultPropagationRecord)
        {
            string pendingFaultedKey = FautedMessageKey(faultPropagationRecord.FaultSource);
            if(_pendingFaultedMessages.ContainsKey(pendingFaultedKey))
            {
                TaskProgressCallbackMessage progressCallbackMessage = _pendingFaultedMessages[pendingFaultedKey];
                _pendingFaultedMessages.Remove(pendingFaultedKey);
                progressCallbackMessage.Message = _formatError(faultPropagationRecord.Fault);
                PushProgressMessage(progressCallbackMessage, faultPropagationRecord.InstanceId);
            }
        }

        private void ProcessActivityStateRecord(ActivityStateRecord activityStateRecord)
        {
            if(activityStateRecord.Arguments.ContainsKey(CustomProgressTrackingDataKey.Target))
            {
                TaskProgressCallbackMessage progressCallbackMessage = ToProgressCallbackMessage(activityStateRecord);

                if (activityStateRecord.State == "Faulted")
                {
                    //will be sent by ProcessFaultPropagationRecord when the fault is available
                    _pendingFaultedMessages.Add(FautedMessageKey(activityStateRecord.Activity), progressCallbackMessage);
                    return;
                }

                Guid workflowInstanceId = activityStateRecord.InstanceId;
                PushProgressMessage(progressCallbackMessage, workflowInstanceId);
            }
        }

        private void PushProgressMessage(TaskProgressCallbackMessage progressCallbackMessage, Guid workflowInstanceId)
        {
            _activeWorkItemsLogs.PushMessage(workflowInstanceId, progressCallbackMessage, _messageIndex++);

            if (ProgressObserver != null)
            {
                ProgressObserver.WorkItemLogCallback(progressCallbackMessage, _messageIndex);
            }
        }

        private static TaskProgressCallbackMessage ToProgressCallbackMessage(ActivityStateRecord activityStateRecord)
        {
            var target = (IWorkItemWithChildren)activityStateRecord.Arguments[CustomProgressTrackingDataKey.Target];
            return new TaskProgressCallbackMessage
            {
                MessageType = activityStateRecord.State,
                ActivityName = activityStateRecord.Activity.Name,
                TimeStamp = activityStateRecord.EventTime.ToLocalTime(),
                RecordNumber = activityStateRecord.RecordNumber,
                RouteToChild = new List<WorkItemId>
                                              {
                                                  new WorkItemId
                                                      {
                                                          Id = target.Id,
                                                          Name = target.Name,
                                                          WorkItemType = target.WorkItemType
                                                      }
                                              }
            };
        }

        private void InitializeTrackingProfile()
        {
            TrackingProfile = new TrackingProfile { Name = "Progress tracking profile" };

            TrackingProfile.Queries.Add(
                new WorkflowInstanceQuery
                    {
                        States =
                        {
                            WorkflowInstanceStates.Completed,
                            WorkflowInstanceStates.Aborted,
                            WorkflowInstanceStates.Canceled,
                            WorkflowInstanceStates.Started,
                            WorkflowInstanceStates.Suspended,
                            WorkflowInstanceStates.UnhandledException
                       },
                    });

            TrackingProfile.Queries.Add(new FaultPropagationQuery());

            TrackingProfile.Queries.Add(
                new ActivityStateQuery
                    {
                        ActivityName = "*",
                        Arguments = { CustomProgressTrackingDataKey.Target },
                        States = 
                        {
                            ActivityStates.Executing,
                            ActivityStates.Closed,
                            ActivityStates.Faulted,
                            ActivityStates.Canceled
                        }
                    });

            TrackingProfile.Queries.Add(
                new CustomTrackingQuery
                    {
                        ActivityName = "*",
                        Name = "*",
                    });

        }
    }
}
