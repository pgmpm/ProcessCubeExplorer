using System;

namespace pgmpm.Consolidation
{
    /// <summary>
    /// The factory which creates the consolidator
    /// </summary>
    /// <author>Bernhard Bruns, Moritz Eversmann, Christopher Licht</author>
    public static class ConsolidatorFactory
    {

        /// <summary>
        /// Creates and returns a new consolidator object
        /// </summary>
        /// <typeparam name="T">Type of the Consolidation Algorithm</typeparam>
        /// <param name="args">Parameter Array</param>
        /// <returns>Consolidator object</returns>
        /// <author>Bernhard Bruns</author>
        public static T CreateConsolidator<T>(params object[] args)
        {
            return (T)Activator.CreateInstance(typeof(T), args);
        }
    }
}