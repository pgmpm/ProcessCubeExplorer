using System;
using System.Runtime.Serialization;

namespace pgmpm.MatrixSelection.Dimensions
{
    /// <summary>
    /// An abstract datatype that stores an entry of the dimension-tables (one row), which will lateron be used for filtering at a particular dimension-level.
    /// </summary>
    /// <author>Jannik Arndt</author>
    [Serializable()]
    public class DimensionContent : ISerializable
    {

        public string Id { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }

        public DimensionContent(string id = "", string content = "", string description = "")
        {
            Id = id;
            Content = content;
            Description = description;
        }

        /// <summary>
        /// Constructor to deserialize
        /// </summary>
        /// <param Name="info"></param>
        /// <param Name="ctxt"></param>
        /// <author>Jannik Arndt</author>
        public DimensionContent(SerializationInfo info, StreamingContext ctxt)
        {
            Id = (string)info.GetValue("id", typeof(string));
            Content = (string)info.GetValue("content", typeof(string));
            Description = (string)info.GetValue("description", typeof(string));
        }

        /// <summary>
        /// Gets the objects for serialization
        /// </summary>
        /// <param Name="info"></param>
        /// <param Name="ctxt"></param>
        /// <author>Jannik Arndt</author>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("id", Id);
            info.AddValue("content", Content);
            info.AddValue("description", Description);
        }

        public override string ToString()
        {
            return Content;
        }
    }
}
