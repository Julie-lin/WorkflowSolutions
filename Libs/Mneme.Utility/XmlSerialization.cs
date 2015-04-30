using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Workflow.Data;

namespace Mneme.Utility
{
    public class XmlSerialization
    {
        public static List<ComponentNode> OpenParameter(string file, Type[] types)
        {
            if (String.IsNullOrEmpty(file))
                return new List<ComponentNode>();
            string path = Path.GetDirectoryName(file);
            string name = Path.GetFileNameWithoutExtension(file);
            string compFile = path + "\\" + name + "_comp" + ".xml";

            List<ComponentNode> test = (List<ComponentNode>)OpenXml(compFile,
                                                                      typeof(List<ComponentNode>),
                                                                      types
                                                                  );


            return test;

        }

        public static List<ComponentNode> OpenWorkflowFile(string file, Type[] types)
        {
            List<ComponentNode> test = (List<ComponentNode>)OpenXml(file,
                                                                      typeof(List<ComponentNode>),
                                                                      types
                                                                  );
            return test;
        }


        public static object OpenXml(string fileName, Type typeToOpen, Type[] types)
        {
            object tempObject = null;

            if (!String.IsNullOrEmpty(fileName))
            {
                FileInfo file = new FileInfo(fileName);
                if (file.Exists)
                {
                    try
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            DeserializeXmlFileToMemoryStream(fileName, memoryStream);

                            memoryStream.Seek(0, SeekOrigin.Begin);
                            tempObject = DeserializeObjectFromStream(memoryStream, typeToOpen, types);
                            //TODO:  WE NEED TO CLEAR ANY DIRTY FLAGS HERE
                            memoryStream.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            return tempObject;
        }

        private static object DeserializeObjectFromStream(Stream ioStream, Type typeOfObject, Type[] types)
        {
            object retVal = null;
            XmlSerializer serializer = new XmlSerializer(typeOfObject, types);
            retVal = serializer.Deserialize(ioStream);
            return retVal;
        }

        private static void DeserializeXmlFileToMemoryStream(string fileName, MemoryStream memoryStream)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileStream);
                    xmlDoc.Save(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    fileStream.Close();
                }
                catch (XmlException ex)
                {
                    throw new SerializationException(
                        String.Format("Invalid Xml file '{0}'.", fileName), ex);
                }
        }

        public static void SaveObjectToXml(string file, object obj, Type type)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (var stream = new FileStream(file, FileMode.CreateNew))
            {
                XmlSerializer serializer = new XmlSerializer(type);
                serializer.Serialize(stream, obj);
            }

        }

        public static void SaveObjectToXML(string file, object obj, Type type, Type[] extraTypes)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (var stream = new FileStream(file, FileMode.CreateNew))
            {
                XmlSerializer serializer = new XmlSerializer(type, extraTypes);   //XmlSerializer(type);
                serializer.Serialize(stream, obj);
            }

        }

        public static void SaveDerivedObjectToXML(string file, object obj, Type type, Type[] extraTypes)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (var stream = new FileStream(file, FileMode.CreateNew))
            {
                XmlSerializer serializer = new XmlSerializer(type, extraTypes);
                serializer.Serialize(stream, obj);
            }

        }
        private static MemoryStream SerializeObjectToMemoryStream(object valueToSerializeToMemoryStream, Type[] types)
        {
            //Converts object [objectToSerialize] of type [typeOfObjectBeingSerialized] to a memory stream  

            MemoryStream retVal = new MemoryStream();
            Type typeToSerialize = valueToSerializeToMemoryStream.GetType();
            XmlSerializer serializer = new XmlSerializer(typeToSerialize, types);
            serializer.Serialize(retVal, valueToSerializeToMemoryStream);

            retVal.Seek(0, SeekOrigin.Begin);
            return retVal;
        }

    }
}
