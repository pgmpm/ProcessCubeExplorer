
namespace pgmpm.Diff.XMLParser.MappingParser
{
    /// <summary>
    /// Semantic Pattern.
    /// </summary>
    /// <author>Carsten Cordes, Thomas Meents</author>
    public class Pattern
    {
        //List of possible Patterns.
        public enum PatternTypes
        {
            Sequence,
            Orsplit,
            Orjoin,
            Andsplit,
            Andjoin,
            Forbidden,
            Xorsplit,
            Xorjoin,
            Loop
        }

        //private readonly Dictionary<TGraphNode, Tuple<int, int>> _allowedCounts;
        //private readonly Graph _pattern;
        public PatternTypes PatternType;

        
    }
}
