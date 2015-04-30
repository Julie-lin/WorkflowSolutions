using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mneme.ProcessLocator
{
    internal class ProxyDomain : MarshalByRefObject
    {
        public Assembly GetAssembly(string AssemblyPath)
        {
            try
            {
                return Assembly.LoadFrom(AssemblyPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
