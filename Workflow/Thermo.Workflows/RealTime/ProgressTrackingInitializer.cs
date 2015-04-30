using System;
using System.Activities;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Thermo.Data.Hierarchical;
using Thermo.Workflows.Cancelation;
using Thermo.Workflows.Contracts;
using Thermo.Workflows.Contracts.RealTime;
using Thermo.Workflows.RealTime;

namespace AcquisitionActivities.RealTime
{
    [Designer(typeof(ProgressTrackingInitializerDesigner))]
    public class ProgressTrackingInitializer : NativeActivity
    {
        public const string WorkItemCancellationBookmark = "WorkItemCancellationBookmark";
        public const string CancellationBookmark = "CancellationBookmark";

        [RequiredArgument]
        public InArgument<IWorkItemWithChildren> RootWorkItem { set; get; }
        public Activity ProgressTrackingScope { get; set; }
        
        protected override bool CanInduceIdle { get{ return true; } }

        protected override void Execute(NativeActivityContext context)
        {
            context.Track(CreateLookupTreeTrackingRecord(RootWorkItem.Get(context).CreateLookupTree()));

            context.CreateBookmark(CancellationBookmark, OnCancellationRequested, BookmarkOptions.MultipleResume);
            context.CreateBookmark(WorkItemCancellationBookmark, OnWorkItemCancellationRequested, BookmarkOptions.MultipleResume);

            context.ScheduleActivity(ProgressTrackingScope, OnProgressActivityCompleted);
        }

        private void OnProgressActivityCompleted(NativeActivityContext context, ActivityInstance completedinstance)
        {
            context.RemoveAllBookmarks();
        }

        private void OnCancellationRequested(NativeActivityContext context, Bookmark bookmark, object value)
        {
            IList<Guid> parthToChild = value as IList<Guid>;
            if (parthToChild == null) return;

            CancelChildWorkItem(parthToChild, context);
        }

        private void OnWorkItemCancellationRequested(NativeActivityContext context, Bookmark bookmark, object value)
        {
            IWorkItemWithChildren workflowRoot = RootWorkItem.Get(context);
            List<Guid> pathToChild = workflowRoot.RouteFromParentToChild((Guid) value).Select(workItem => workItem.Id).ToList();
            CancelChildWorkItem(pathToChild, context);
        }

        private void CancelChildWorkItem(IList<Guid> parthToChild, NativeActivityContext context)
        {
            ICancellableWorkItemWithChildren cancellationRoot = RootWorkItem.Get(context).GoToChild(parthToChild) 
                                                                as ICancellableWorkItemWithChildren;

            if (cancellationRoot != null)
            {
                BookmarkResumptionResult cancelationRequestResult = context.ResumeBookmark(
                    new Bookmark(WorkItemCancelationScope.CancelableWorkItemBookmarkName(cancellationRoot.Id)),
                    cancellationRoot);

                if (cancelationRequestResult != BookmarkResumptionResult.Success)
                {
                    WorkItemCancelationScope.MarkTasksAsCancelled(cancellationRoot, context, "Because cancellation request.");
                }
            }
        }

        private CustomTrackingRecord CreateLookupTreeTrackingRecord(WorkItemWithHistory lookupTree)
        {
            return new CustomTrackingRecord(WorkItemStatus.Received)
                        {
                            Data = 
                            {
                                {CustomProgressTrackingDataKey.LookupTree, lookupTree}
                            }
                        };
        }
    }
}
