using System.Activities;
using System.ComponentModel;

namespace Thermo.Workflows.Activities
{
    [Designer(typeof(GetQueuePriorityTicketDesigner))]
    public sealed class GetQueuePriorityTicket : CodeActivity
    {
        [RequiredArgument]
        public InArgument<string> QueueName { get; set; }
        public OutArgument<long> PriorityTicket { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            metadata.AddDefaultExtensionProvider(() => new CriticalSectionQueueExtension());
        }

        protected override void Execute(CodeActivityContext context)
        {
            string queueName = QueueName.Get(context);
            long priorityTicket = context.GetExtension<CriticalSectionQueueExtension>()
                .GetPriorityTicket(queueName);
            PriorityTicket.Set(context, priorityTicket);
        }
    }
}
