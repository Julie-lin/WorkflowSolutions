using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mneme.ProcessLocator;
using Workflow.Data;
using Workflow.Data.Interfaces;

namespace Workflow.Activities.Activities
{
     public sealed class BatchInitializeActivity : CodeActivity
     {
         [RequiredArgument]
         public InArgument<ProcessBatch> Batch { get; set; }
         [RequiredArgument]
         public InArgument<List<ComponentNode>> ComponentParameters { get; set; }

         protected override void Execute(CodeActivityContext context)
         {
             List<ComponentNode> nodes = ComponentParameters.Get(context);
             var start = nodes[0].FindStartupNode(nodes);
              
             //IInitializeBatch batchInitializer = ProcessObjectLocator.LocateBatchInitializeProcess(start.BatchInitializeExcutionName);
             
             Type tp = ProcessRunTimeLocator.GetBatchInitializerType(start.BatchInitializeExcutionName);
             if (tp == null)
                 return;
             IInitializeBatch batchInitializer = (IInitializeBatch)Activator.CreateInstance(tp);
             batchInitializer.ExecuteBatchInitializer(Batch.Get(context), nodes);

         }
     }
}
