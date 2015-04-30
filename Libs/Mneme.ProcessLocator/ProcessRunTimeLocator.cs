using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Workflow.Data;
using Workflow.Data.Interfaces;

namespace Mneme.ProcessLocator
{
    //http://stackoverflow.com/questions/26733/getting-all-types-that-implement-an-interface-with-c-sharp-3-0
    public static class ProcessRunTimeLocator
    {
        private static Dictionary<string, Type> _executeComponentsCache = new Dictionary<string, Type>();
        private static Dictionary<string, Type> _componentNodeCache = new Dictionary<string, Type>();
        private static Dictionary<string, Type> _componentBatchNodeCache = new Dictionary<string, Type>();
        private static Dictionary<string, Type> _componentGroupNodeCache = new Dictionary<string, Type>();
        private static Dictionary<string, Type> _componentJobNodeCache = new Dictionary<string, Type>();
        private static Dictionary<string, Type> _executeStartUpComponentCache = new Dictionary<string, Type>();
        private static Dictionary<string, Type> _executeBatchComponentsCache = new Dictionary<string, Type>();
        private static Dictionary<string, Type> _executeGroupComponentsCache = new Dictionary<string, Type>();
        private static Dictionary<string, Type> _batchInitializerCache = new Dictionary<string, Type>();
        public static Type[] _appExtraData = null;

        public static Type GetDynamicComponentNodeType(string typeName)
        {
            Type result = null;
            if (_componentNodeCache.Count == 0)
            {
                RegisterMnemeComponent();
            }

            if (_componentNodeCache.ContainsKey(typeName))
            {
                result = _componentNodeCache[typeName];
            }
            if (_componentBatchNodeCache.ContainsKey(typeName))
            {
                result = _componentBatchNodeCache[typeName];
            }
            if (_componentGroupNodeCache.ContainsKey(typeName))
            {
                result = _componentGroupNodeCache[typeName];
            }
            if (_componentJobNodeCache.ContainsKey(typeName))
            {
                result = _componentJobNodeCache[typeName];
            }


            return result;
        }

        public static Type[] GetAppExtraDataTypes()
        {
            if (_appExtraData == null)
            {
                BuildAppExtraData();
            }
            return _appExtraData;
        }

        private static void BuildAppExtraData()
        {
            if (_componentNodeCache.Count == 0)
            {
                RegisterMnemeComponent();
            }
            int count = 0;
            _appExtraData = new Type[_componentNodeCache.Count];
            foreach (string key in _componentNodeCache.Keys)
            {
                _appExtraData[count] = _componentNodeCache[key];
                count++;
            }
        }

        public static void RegisterMnemeComponent()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var di = new DirectoryInfo(path);
            PreLoad(path);
        }

        private static void PreLoad(string p)
        {
            //all try/catch blocks are elided for brevity
            string[] files = null;

            files = Directory.GetFiles(p, "*.dll", SearchOption.AllDirectories);

            AssemblyName a = null;
            AppDomain tempDomain = AppDomain.CreateDomain("tempDomain");
            try
            {
                foreach (var s in files)
                {
                    Debug.WriteLine(s);
                    try
                    {
                        a = AssemblyName.GetAssemblyName(s);
                        if (!tempDomain.GetAssemblies().Any(
                            assembly => AssemblyName.ReferenceMatchesDefinition(
                                assembly.GetName(), a)))
                        {
                            //  Assembly.LoadFrom(s);
                            //if (s.Contains("workflow.data"))
                            //{
                            //    Debug.WriteLine("");
                            //}
                            var nextAssembly = Assembly.LoadFrom(s);
                            //var nextAssembly = Assembly.Load(s);

                            foreach (Type mytype in nextAssembly.GetTypes().Where(mytype => mytype.GetInterfaces().Contains(typeof(IExcuteComponent))))
                            {
                                string typeName = mytype.Name;
                                AddToExecutableComponentMap(typeName, mytype);
                            }
                            foreach (Type mytype in nextAssembly.GetTypes().Where(mytype => mytype.BaseType == (typeof(Workflow.Data.ComponentNode))))
                            {
                                string typeName = mytype.Name;
                                AddToComponentNodeMap(typeName, mytype);
                            }
                            foreach (Type mytype in nextAssembly.GetTypes().Where(mytype => mytype.BaseType == (typeof(Workflow.Data.ProcessBatch))))
                            {
                                string typeName = mytype.Name;
                                AddToBatchComponentNodeMap(typeName, mytype);
                            }
                            foreach (Type mytype in nextAssembly.GetTypes().Where(mytype => mytype.BaseType == (typeof(Workflow.Data.ProcessGroup))))
                            {
                                string typeName = mytype.Name;
                                AddToGroupComponentNodeMap(typeName, mytype);
                            }
                            foreach (Type mytype in nextAssembly.GetTypes().Where(mytype => mytype.BaseType == (typeof(Workflow.Data.ProcessJob))))
                            {
                                string typeName = mytype.Name;
                                AddToJobComponentNodeMap(typeName, mytype);
                            }

                            foreach (Type mytype in nextAssembly.GetTypes().Where(mytype => mytype.GetInterfaces().Contains(typeof(IExecuteStartupComponent))))
                            {
                                string typeName = mytype.Name;
                                AddToExecutableStartUpComponentMap(typeName, mytype);
                            }
                            foreach (Type mytype in nextAssembly.GetTypes().Where(mytype => mytype.GetInterfaces().Contains(typeof(IExecuteBatchComponent))))
                            {
                                string typeName = mytype.Name;
                                AddToExecutableBatchComponentMap(typeName, mytype);
                            }
                            foreach (Type mytype in nextAssembly.GetTypes().Where(mytype => mytype.GetInterfaces().Contains(typeof(IExecuteGroupComponent))))
                            {
                                string typeName = mytype.Name;
                                AddToExecutableGroupComponentMap(typeName, mytype);
                            }

                            foreach (Type mytype in nextAssembly.GetTypes().Where(mytype => mytype.GetInterfaces().Contains(typeof(IInitializeBatch))))
                            {
                                string typeName = mytype.Name;
                                AddToBatchInitializerMap(typeName, mytype);
                            }
                        }
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        foreach (Exception loaderException in ex.LoaderExceptions)
                        {
                            Debug.Write(loaderException.ToString());
//                            Console.WriteLine(loaderException.ToString());
                        }
                        
                       //silent some unknown dll
                        Debug.Write(ex.Message);
                        //throw;
                    }
                }

                //                Assembly testLibrary = tempDomain.Load(LibraryName);
            }
            finally
            {
                AppDomain.Unload(tempDomain);
            }
        }

        public static Type[] GetComponentExtraData()
        {
            Type[] types = new Type[_componentNodeCache.Count()];
            int count = 0;
            foreach (string key in _componentNodeCache.Keys)
            {
                types[count] = _componentNodeCache[key];
            }
            return types;
        }
        public static void AddToExecutableComponentMap(string name, Type type)
        {
            _executeComponentsCache.Add(name, type);
        }

        public static void AddToBatchComponentNodeMap(string name, Type type)
        {
            _componentBatchNodeCache.Add(name, type);
        }
        public static void AddToGroupComponentNodeMap(string name, Type type)
        {
            _componentGroupNodeCache.Add(name, type);
        }

        public static void AddToJobComponentNodeMap(string name, Type type)
        {
            _componentJobNodeCache.Add(name, type);
        }

        public static void AddToComponentNodeMap(string name, Type type)
        {
            _componentNodeCache.Add(name, type);
        }

        public static void AddToExecutableStartUpComponentMap(string name, Type type)
        {
            _executeStartUpComponentCache.Add(name, type);
        }

        public static void AddToExecutableBatchComponentMap(string name, Type type)
        {
            _executeBatchComponentsCache.Add(name, type);
        }
        public static void AddToExecutableGroupComponentMap(string name, Type type)
        {
            _executeGroupComponentsCache.Add(name, type);
        }

        
        public static void AddToBatchInitializerMap(string name, Type type)
        {
            _batchInitializerCache.Add(name, type);
        }
        public static Type GetExecutableType(string name)
        {
            if (_executeComponentsCache.ContainsKey(name))
            {
                return _executeComponentsCache[name];
            }
            return null;
        }

        public static Type GetComponentNodeType(string name)
        {
            if (_componentNodeCache.ContainsKey(name))
            {
                return _componentNodeCache[name];
            }
            return null;
        }
        public static Type GetBatchComponentNodeType(string name)
        {
            if (_componentBatchNodeCache.ContainsKey(name))
            {
                return _componentBatchNodeCache[name];
            }
            return null;
        }

        public static Type GetGroupComponentNodeType(string name)
        {
            if (_componentGroupNodeCache.ContainsKey(name))
            {
                return _componentGroupNodeCache[name];
            }
            return null;
        }

        public static Type GetJobComponentNodeType(string name)
        {
            if (_componentJobNodeCache.ContainsKey(name))
            {
                return _componentJobNodeCache[name];
            }
            return null;
        }

        public static Type GetExecutaleStartupType(string name)
        {
            if (_executeStartUpComponentCache.ContainsKey(name))
            {
                return _executeStartUpComponentCache[name];
            }
            return null;
        }

        public static Type GetExecutableBatchType(string name)
        {
            if (_executeBatchComponentsCache.ContainsKey(name))
            {
                return _executeBatchComponentsCache[name];
            }
            return null;
        }

        public static Type GetExecutableGroupType(string name)
        {
            if (_executeGroupComponentsCache.ContainsKey(name))
            {
                return _executeGroupComponentsCache[name];
            }
            return null;
        }

        public static Type GetBatchInitializerType(string name)
        {
            if (_batchInitializerCache.ContainsKey(name))
            {
                return _batchInitializerCache[name];
            }
            return null;
        }

    }
}
