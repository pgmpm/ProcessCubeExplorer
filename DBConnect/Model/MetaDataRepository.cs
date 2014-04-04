using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using pgmpm.MatrixSelection.Dimensions;

namespace pgmpm.Database.Model
{
    /// <summary>
    /// The FactTable-Class provides a way to store information on a Database-schema, most importantly the ListOfDimensions
    /// </summary>
    /// <author>Jannik Arndt, Bernhard Bruns, Moritz Eversmann</author>

    [Serializable]
    [XmlRoot("Metadata")]
    public class MetaDataRepository
    {
        public String EventClassifier = "";
        [XmlIgnore]
        public string Name { get; set; }
        [XmlIgnore]
        public List<String> ListOfCaseTableColumnNames { get; set; }

        public List<String> ListOfEventsTableColumnNames { get; set; }

        public List<Dimension> ListOfFactDimensions { get; set; }

        public List<Dimension> ListOfEventDimensions { get; set; }

        /// <summary>
        /// Parameterless constructor for the xml-serializer
        /// The serializer doesn't work without this empty constructor!
        /// </summary>
        /// <author>Bernhard Bruns</author>
        private MetaDataRepository()
        {
        }

        /// <summary>
        /// Constructor for a new FactTable, where the DimensionColumnNames for case and event are already known.
        /// </summary>
        /// <param name="name">The Name of this table</param>
        public MetaDataRepository(string name = "")
        {
            Name = name;
            ListOfFactDimensions = new List<Dimension>();
            ListOfEventDimensions = new List<Dimension>();
            ListOfCaseTableColumnNames = new List<String>();
            ListOfEventsTableColumnNames = new List<String>();
        }
    }
}