using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workflow.Data.Interfaces
{
    public interface IProcess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="extraProcessInfo"></param>
        /// <param name="sequenceId"></param>
        /// <param name="jobId"></param>
        /// <param name="iBatch"></param>
        /// <returns></returns>
        IProcessBatch DoProcess(IProcessBatch iBatch,
                                ICollection<ExtraProcessInfo> extraProcessInfo,
                                Guid sequenceId,
                                Guid jobId);
    }
}
