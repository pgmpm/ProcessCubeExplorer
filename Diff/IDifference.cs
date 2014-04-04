using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pgmpm.Model;

namespace pgmpm.Diff
{
    /// <summary>
    /// Interface for the difference calculation.
    /// </summary>
    /// <author>Christopher Licht</author>
    public interface IDifference
    {
        /// <summary>
        /// Initialize the process of comparing process models.
        /// </summary>
        /// <param name="listOfChoosenProcessModels">List with mined process models.</param>
        /// <returns>Returns the difference model.</returns>
        /// <author>Christopher Licht</author>
        ProcessModel CompareProcessModels(List<ProcessModel> listOfChoosenProcessModels);
    }
}
