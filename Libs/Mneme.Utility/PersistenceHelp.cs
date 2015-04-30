using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;

namespace Mneme.Utility
{
    
    public class PersistenceHelp<T>
    {
        #region Public Methods
        /// <summary>
        /// the persisted file.
        /// </summary>
        /// <exception cref="CommonComponentsException">
        /// Thrown if there is a problem loading the object (e.g. File not found). 
        /// </exception>
        /// <param name="path">full path where the persisted object is located.</param>
        /// <returns>
        /// The object of the generic type.
        /// </returns>
        public static T Load(string path)
        {
            try
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    return Load(fs);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to load the data object from a file and ignore the namespace.
        /// </summary>
        /// <param name="path">full path where the persisted object is located.</param>
        /// <param name="namespaceToIgnore">
        /// This is used when we need to ignore the namespace (e.g. the namespace was changed from version
        /// to version).
        /// </param>
        /// <returns>
        /// An instance of the generic type.
        /// </returns>
        /// <exception cref="CommonComponentsException">
        /// Thrown if there is a problem loading the object from the stream (e.g. invalid format). 
        /// The exception is logged.
        /// </exception>
        public static T Load(string path, string namespaceToIgnore)
        {
            var type = typeof(T);
            var serializer = new DataContractSerializer(type, type.Name, string.Empty);
            try
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    var reader = new IgnoreNamespaceXmlReader(new XmlTextReader(fs), namespaceToIgnore);
                    return (T)serializer.ReadObject(reader);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }


        /// <summary>
        /// Method to load data from a file stream.
        /// </summary>
        /// <param name="stream">
        /// The file stream.
        /// </param>
        /// <returns>
        /// An instance of the generic type.
        /// </returns>
        /// <exception cref="CommonComponentsException">
        /// Thrown if there is a problem loading the object from the stream (e.g. invalid format). 
        /// The exception is logged.
        /// </exception>
        public static T Load(Stream stream)
        {
            var serializer = new DataContractSerializer(typeof(T));
            try
            {
                return (T)serializer.ReadObject(stream);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Method that persists the object to a file.
        /// </summary>
        /// <exception cref="CommonComponentsException">
        /// Thrown if there is a problem persisting the object (e.g. no
        /// more disk space).
        /// The exception is logged.
        /// </exception>
        /// <param name="path">
        /// The key used to generate the file name for saving the file.
        /// </param>
        /// <param name="obj">
        /// The object to be persisted.
        /// </param>
        public static void Save(string path, object obj)
        {
            var serializer = new DataContractSerializer(typeof(T));
            try
            {
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    serializer.WriteObject(fs, obj);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Serialize the object to xml.
        /// </summary>
        /// <param name="obj">
        /// The generic object.
        /// </param>
        /// <returns>The object serialized in a string.</returns>
        public static string ToXml(object obj)
        {
            try
            {
                var dataContractSerializer = new DataContractSerializer(typeof(T));

                using (var stringWriter = new StringWriter())
                {
                    using (var xmlTextWriter = new XmlTextWriter(stringWriter))
                    {
                        dataContractSerializer.WriteObject(xmlTextWriter, obj);
                        return stringWriter.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Deserialize the object from xml string.
        /// </summary>
        /// <param name="xmlData">
        /// The xml string representing the object to be deserialized.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        public static T FromXml(string xmlData)
        {
            try
            {
                var dataContractSerializer = new DataContractSerializer(typeof(T));

                using (var stringReader = new StringReader(xmlData))
                {
                    using (var xmlTextReader = new XmlTextReader(stringReader))
                    {
                        return (T)dataContractSerializer.ReadObject(xmlTextReader);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Method that removes the file containing the object.
        /// </summary>
        /// <exception cref="CommonComponentsException">
        /// Thrown if there is a problem removing the object (e.g. Access denied). 
        /// The exception is logged.
        /// </exception>
        /// <param name="path">
        /// The key to identify the file to be deleted.
        /// </param>
        public static void Remove(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("File not found!", path);
                }

                File.Delete(path);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion     
    }
}
