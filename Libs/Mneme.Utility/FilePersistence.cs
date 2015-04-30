using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;



namespace Mneme.Utility
{
    [DataContract(Namespace = "")]
    public abstract class FilePersistenceBase<T> 
    {
        #region Public Static methods for loading and file management
        /// <summary>
        /// Retrieves an object instance. This is static so that we don't have to
        /// create the object, call this method, and then assign it.
        /// </summary>
        /// <param name="folder">
        /// Folder where the persisted objects are located.
        /// </param>
        /// <param name="key">
        /// Key to retrieve the object.
        /// </param>
        /// <returns>
        /// Instance referenced by the key. Null if the object is not found. 
        /// In case of errors, the exception is logged
        /// by the <see cref="PersistenceHelp{T}"/>.
        /// </returns>
        public static T Retrieve(string folder, string key)
        {
            return PersistenceHelp<T>.Load(GetFilePath(folder, key));
        }

        /// <summary>
        /// Retrieves an object instance. This is static so that we don't have to
        /// create the object, call this method, and then assign it.
        /// </summary>
        /// <param name="folder">
        /// The folder.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="namespaceToIgnore">
        /// This is used when we need to ignore the namespace (e.g. the namespace was changed from version
        /// to version).
        /// </param>
        /// <returns>
        /// Instance referenced by the key. Null if the object is not found. 
        /// In case of errors, the exception is logged
        /// by the <see cref="PersistenceHelp{T}"/>.
        /// </returns>
        public static T Retrieve(string folder, string key, string namespaceToIgnore)
        {
            return PersistenceHelp<T>.Load(GetFilePath(folder, key), namespaceToIgnore);
        }

        /// <summary>
        /// Deserialize the object from an xml string.
        /// </summary>
        /// <param name="xml">
        /// The xml to deserialize.
        /// </param>
        /// <returns>
        /// The generic object or null if there is a failure.
        /// </returns>
        public static T FromXml(string xml)
        {
            T obj;

            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            try
            {
                obj = PersistenceHelp<T>.FromXml(xml);
            }
            catch
            {
                return default(T);
            }

            return obj;
        }

        /// <summary>
        /// The method deserializes all the files in the path and returns a list of the generic typed objects.
        /// </summary>
        /// <param name="path">
        /// The path where the persisted objects are located.
        /// </param>
        /// <returns>
        /// A list of the generic typed objects
        /// </returns>
        public static IList<T> GetAllPersistedObjects(string path)
        {
            var listOfObjects = new List<T>();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var directory = new DirectoryInfo(path);
            var fileInfos = directory.GetFiles().OrderBy(f => f.LastWriteTime);
            foreach (var file in fileInfos)
            {
                try
                {
                    var persistedObject = PersistenceHelp<T>.Load(file.FullName);

                    listOfObjects.Add(persistedObject);
                }
                catch (Exception ex)
                {
                    var msg = string.Format("Cannot Load '{0}'. Reason: {1}", file.Name, ex.Message);

                    // TODO: Log the exception when we integrate with the logger.                
                }
            }

            return listOfObjects;
        }

        /// <summary>
        /// The method clears all persisted objects.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>The number of objects deleted.</returns>
        public static int ClearAllPersistedObjects(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            var results = 0;
            foreach (var file in directoryInfo.GetFiles())
            {
                try
                {
                    file.Delete();
                    results++;
                }
                catch (Exception ex)
                {
                    var msg = string.Format("Cannot delete '{0}'. Reason: {1}", file.Name, ex.Message);

                    // TODO: Log the exception when we integrate with the logger.
                }
            }

            return results;
        }

        /// <summary>
        /// The method clears all persisted objects.
        /// </summary>
        /// <param name="filePath">
        /// The path.
        /// </param>
        /// <returns>The number of objects deleted.</returns>
        public static bool ClearPersistedObject(string filePath)
        {
            bool returnValue;
            var fileInfo = new FileInfo(filePath);
            try
            {
                fileInfo.Delete();
                returnValue = true;
            }
            catch (Exception ex)
            {
                var msg = string.Format("Cannot delete '{0}'. Reason: {1}", filePath, ex.Message);
                returnValue = false;
            }

            return returnValue;
        }
        #endregion

        /// <summary>
        /// Method that persists the instance. It should support
        /// both new instance creation as well as updates.
        /// </summary>
        /// <param name="folder">
        /// Folder where the persisted objects are located.
        /// </param>
        /// <param name="key">
        /// key for object instance to save.
        /// </param>
        /// <exception cref="CommonComponentsException">Throw if there is a problem saving the file.</exception>
        public void Save(string folder, string key)
        {
            PersistenceHelp<T>.Save(GetFilePath(folder, key), this);
        }

        /// <summary>
        /// Method to remove the persisted instance of type T from the persistence mechanism.
        /// </summary>
        /// <param name="folder">
        /// Folder where the persisted objects are located.
        /// </param>
        /// <param name="key">
        /// Key for object instance to remove.
        /// </param>
        /// <exception cref="CommonComponentsException">Throw if there is a problem removing the file.</exception>
        public void Remove(string folder, string key)
        {
            PersistenceHelp<T>.Remove(GetFilePath(folder, key));
        }

        /// <summary>
        /// Serialize the object to an xml string.
        /// </summary>
        /// <returns>
        /// The xml string of the serialized object. If there is a failure,
        /// the exception is logged and the xml string will be string.Empty.
        /// </returns>
        public string ToXml()
        {
            string xml;
            try
            {
                xml = PersistenceHelp<T>.ToXml(this);
            }
            catch
            {
                return string.Empty;
            }

            return xml;
        }

        #region Private Helpers
        /// <summary>
        /// Creates the full persisted file path from the key.
        /// </summary>
        /// <param name="folder">Folder where the persisted objects are located.</param>
        /// <param name="key">
        /// The key that will uniquely identify the object to be persisted.
        /// </param>
        /// <returns>
        /// Full File Path.
        /// </returns>
        private static string GetFilePath(string folder, string key)
        {
            // we want to remove the invalid path characters from
            // the key so that we can creat a legal file path.
            var invalidChars = Path.GetInvalidFileNameChars();

            try
            {
                key = invalidChars.Aggregate(key, (current, ch) => current.Replace(ch, '-'));

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var fileName = Path.Combine(folder, key);
                if (string.IsNullOrWhiteSpace(Path.GetExtension(key)))
                {
                    fileName += ".xml";
                }
                return fileName;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion
    }

}
