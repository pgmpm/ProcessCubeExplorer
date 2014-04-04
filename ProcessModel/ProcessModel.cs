using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace pgmpm.Model
{
    /// <summary>
    /// Provides an abstract class that will be implemented by PetriNets, CausalNets and BPMN and will be easy to expand.
    /// </summary>
    /// <author>Jannik Arndt</author>
    public abstract class ProcessModel : ISerializable
    {
        // Abstract methods that must be implemented
        public abstract String IsOfKind();
        public abstract void GetObjectData(SerializationInfo info, StreamingContext context);

        public String Name { get; set; }
        public int DeepCopyHash { get; set; }

        /// <summary>
        /// Creates a deep copy of the petrinet object
        /// </summary>
        /// <returns></returns>
        /// <author>Bernhard Bruns</author>
        public ProcessModel DeepCopy()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, this);
                memoryStream.Position = 0;
                ProcessModel temp =formatter.Deserialize(memoryStream) as ProcessModel;
                DeepCopyHash = temp.GetHashCode();
                return temp;
            }
        }
    }
}
