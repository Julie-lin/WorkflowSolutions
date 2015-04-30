using System;
using System.Activities.Tracking;
using System.Activities;
using AcquisitionActivities.RealTime;
using Thermo.Data.Hierarchical;

namespace Thermo.Workflows.RealTime
{

    public sealed class PublishWorkItemProgress : CodeActivity
    {
        public PublishWorkItemProgress()
        {
            WorkItemStatus = new InArgument<string>(Contracts.RealTime.WorkItemStatus.Info);
            ProgressMessage = new InArgument<string>(string.Empty);
        }

        public InArgument<string> ProgressMessage { get; set; }
        public InArgument<string> WorkItemStatus { get; set; }
        [RequiredArgument]
        public InArgument<IWorkItemWithChildren> Item { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            context.Track(
               new CustomTrackingRecord(WorkItemStatus.Get(context))
               {
                   Data = 
                        {
                            {CustomProgressTrackingDataKey.Target, Item.Get(context)},
                            {CustomProgressTrackingDataKey.Message, ProgressMessage.Get(context)}
                        }
               });
        }
    }
}
