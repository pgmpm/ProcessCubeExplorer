
namespace pgmpm.Diff.XMLParser.MappingParser
{
    /// <summary>
    /// Represents a single MappingRule for a single ElementType.
    /// </summary>
    /// <author>Carsten Cordes, Thomas Meents</author>
    class MappingRule
    {
        public string IdentifierName;
        public bool IsEdge;
        public string RepresentationType;
        public string TypeName;

        /// <summary>
        /// Constructor
        /// </summary>
        internal MappingRule()
        {
        }

        /// <summary>
        /// Checks that Mapping rule has every Attribute it needs.
        /// </summary>
        /// <returns></returns>
        /// <author>Carsten Cordes, Thomas Meents</author>
        public bool IsValid()
        {
            //Test needed Attributes
            if (string.IsNullOrEmpty(TypeName))
                return false;
            if (string.IsNullOrEmpty(RepresentationType))
                return false;
            if (string.IsNullOrEmpty(IdentifierName))
                return false;

            return true;
        }
    }
}
