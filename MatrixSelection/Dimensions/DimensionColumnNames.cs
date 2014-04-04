using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace pgmpm.MatrixSelection.Dimensions
{
    /// <summary>
    /// Stores the Name of the columns specific to this dimension, e.g. AGE_ID, AGE and AGE_DESC
    /// </summary>
    /// <author>Jannik Arndt</author>
    [Serializable()]
    [XmlType(TypeName = "DimensionColumnNames")]
    public class DimensionColumnNames : ISerializable
    {
        [XmlAttribute("Col_Id")]
        public string Col_Id { get; set; }

        [XmlAttribute("Col_Content")]
        public string Col_Content { get; set; }

        [XmlAttribute("col_description")]
        public string Col_Description { get; set; }

        public DimensionColumnNames()
        {
        }

        public DimensionColumnNames(string set_id, string set_content, string set_desc)
        {
            Col_Id = set_id;
            Col_Content = set_content;
            Col_Description = set_desc;
        }

        /// <summary>
        /// Constructor to deserialize
        /// </summary>
        /// <param Name="info"></param>
        /// <param Name="ctxt"></param>
        /// <author>Jannik Arndt</author>
        public DimensionColumnNames(SerializationInfo info, StreamingContext ctxt)
        {
            Col_Id = (string)info.GetValue("Col_Id", typeof(string));
            Col_Content = (string)info.GetValue("Col_Content", typeof(string));
            Col_Description = (string)info.GetValue("col_description", typeof(string));
        }

        /// <summary>
        /// Gets the objects for serialization
        /// </summary>
        /// <param Name="info"></param>
        /// <param Name="ctxt"></param>
        /// <author>Jannik Arndt</author>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Col_Id", Col_Id);
            info.AddValue("Col_Content", Col_Content);
            info.AddValue("col_description", Col_Description);
        }
    }
}
