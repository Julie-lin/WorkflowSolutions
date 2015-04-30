using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mneme.WinService
{
    internal class ExceptionInfo
    {
        public ExceptionInfo(Exception exception)
        {
            ex = exception;
            if (exception.InnerException != null)
            {
                InnerExceptionInfo = new ExceptionInfo(exception.InnerException);
            }
        }

        private Exception ex;

        public Exception Exception
        {
            get { return ex; }
        }

        public string StackTrace
        {
            get
            {
                return ex.StackTrace;
            }
        }

        public string Source
        {
            get
            {
                return ex.Source;
            }
        }

        public string Message
        {
            get
            {
                return ex.Message;
            }
        }

        public string TypeName
        {
            get
            {
                return ex.GetType().Name;
            }
        }

        public ExceptionInfo InnerExceptionInfo
        {
            get;
            private set;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("Machine: {0}", Environment.MachineName));
            sb.AppendLine(String.Format("Time: {0}", DateTime.Now));
            sb.AppendLine();
            WriteInfo(sb, "|");
            return sb.ToString();
        }

        private void WriteInfo(StringBuilder sb, string identation)
        {
            sb.AppendLine(identation + String.Format("Exception type = {0}", TypeName));
            identation += "\t";
            sb.AppendLine(String.Format(identation + "Message = {0}", (Message ?? string.Empty)
                .Replace(Environment.NewLine, Environment.NewLine + identation + "\t")));
            sb.AppendLine(String.Format(identation + "Source = {0}", Source));
            sb.AppendLine(String.Format(identation + "Stack = {0}", (StackTrace ?? string.Empty)
                .Replace(Environment.NewLine, Environment.NewLine + identation)));

            sb.AppendLine(String.Format(identation + "InnerException: {0}", InnerExceptionInfo == null ? "null" : ""));
            if (InnerExceptionInfo != null)
            {
                InnerExceptionInfo.WriteInfo(sb, identation + "|");
            }
        }
    }
}
