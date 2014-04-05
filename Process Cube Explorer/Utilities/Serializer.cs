using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace pgmpm.MainV2.Utilities
{
    public static class Serializer
    {
        /// <summary>
        /// Serializes an object
        /// </summary>
        /// <param name="filename">File name</param>
        /// <param name="objectToSerialize">Object</param>
        /// <returns>True if serialization was successful.</returns>
        /// <author>Jannik Arndt</author>
        public static bool Serialize(string filename, object objectToSerialize)
        {
            if (objectToSerialize == null)
                throw new ArgumentNullException("objectToSerialize cannot be null");

            Stream stream = null;

            try
            {
                stream = File.Open(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + filename, FileMode.Create);
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(stream, objectToSerialize);
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }

        /// <summary>
        /// Deserializes an Object
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="filename">File name</param>
        /// <returns>Deserialized object</returns>
        /// <author>Jannik Arndt</author>
        public static T Deserialize<T>(string filename)
        {
            T objectToSerialize;
            Stream stream = null;

            try
            {
                stream = File.Open(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + filename, FileMode.Open);
                BinaryFormatter bFormatter = new BinaryFormatter();
                objectToSerialize = (T)bFormatter.Deserialize(stream);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return objectToSerialize;
        }
    }
}
