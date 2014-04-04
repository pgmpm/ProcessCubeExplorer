using System.Collections.Generic;
using pgmpm.MatrixSelection.Fields;

namespace pgmpm.Consolidation
{
    /// <summary>
    /// Interface for the Consolidation
    /// </summary>
    /// <author>Bernhard Bruns, Moritz Eversmann, Christopher Licht</author>
    public interface IConsolidator
    {
        HashSet<Field> Consolidate();
    }
}