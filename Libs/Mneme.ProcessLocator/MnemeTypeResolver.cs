using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mneme.ProcessLocator
{
    public class MnemeTypeResolver : DataContractResolver
    {

        public override bool TryResolveType(Type dataContractType, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {

            if (knownTypeResolver.TryResolveType(dataContractType, declaredType, null, out typeName, out typeNamespace))
            {
                XmlDictionary dictionary = new XmlDictionary();
                typeName = dictionary.Add(dataContractType.FullName);
                typeNamespace = dictionary.Add(dataContractType.Assembly.FullName);
            }
            return true;
        }


        public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
        {
            Type result = ProcessRunTimeLocator.GetDynamicComponentNodeType(typeName);
            if (result != null)
            {
                return result;
            }
            Type tt = knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null) ?? Type.GetType(typeName + ", " + typeNamespace);
            return tt;
        }
    }
    //http://blogs.msdn.com/b/youssefm/archive/2009/06/05/introducing-a-new-datacontractserializer-feature-the-datacontractresolver.aspx
    //public class SharedTypeResolver : DataContractResolver
    //{
    //    public override bool TryResolveType(Type dataContractType, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
    //    {
    //        if (!knownTypeResolver.TryResolveType(dataContractType, declaredType, null, out typeName, out typeNamespace))
    //        {
    //            XmlDictionary dictionary = new XmlDictionary();
    //            typeName = dictionary.Add(dataContractType.FullName);
    //            typeNamespace = dictionary.Add(dataContractType.Assembly.FullName);
    //        }
    //        return true;
    //    }


    //    public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
    //    {
    //        return knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null) ?? Type.GetType(typeName + ", " + typeNamespace);
    //    }
    //}

    //public class CachingResolver : DataContractResolver
    //{
    //    Dictionary<string, int> serializationDictionary;
    //    Dictionary<int, string> deserializationDictionary;
    //    int serializationIndex = 0;
    //    XmlDictionary dic;


    //    public CachingResolver()
    //    {
    //        serializationDictionary = new Dictionary<string, int>();
    //        deserializationDictionary = new Dictionary<int, string>();
    //        dic = new XmlDictionary();
    //    }


    //    public override bool TryResolveType(Type dataContractType, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
    //    {
    //        if (!knownTypeResolver.TryResolveType(dataContractType, declaredType, null, out typeName, out typeNamespace))
    //        {
    //            return false;
    //        }
    //        int index;
    //        if (serializationDictionary.TryGetValue(typeNamespace.Value, out index))
    //        {
    //            typeNamespace = dic.Add(index.ToString());
    //        }
    //        else
    //        {
    //            serializationDictionary.Add(typeNamespace.Value, serializationIndex);
    //            typeNamespace = dic.Add(serializationIndex++ + "#" + typeNamespace);
    //        }
    //        return true;
    //    }


    //    public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
    //    {
    //        Type type;
    //        int deserializationIndex;
    //        int poundIndex = typeNamespace.IndexOf("#");
    //        if (poundIndex < 0)
    //        {
    //            if (Int32.TryParse(typeNamespace, out deserializationIndex))
    //            {
    //                deserializationDictionary.TryGetValue(deserializationIndex, out typeNamespace);
    //            }
    //            type = knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null);
    //        }
    //        else
    //        {
    //            if (Int32.TryParse(typeNamespace.Substring(0, poundIndex), out deserializationIndex))
    //            {
    //                typeNamespace = typeNamespace.Substring(poundIndex + 1, typeNamespace.Length - poundIndex - 1);
    //                deserializationDictionary.Add(deserializationIndex, typeNamespace);
    //            }
    //            type = knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null);
    //        }
    //        return type;
    //    }
    //}

}
