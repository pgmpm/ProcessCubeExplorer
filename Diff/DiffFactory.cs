using System;

namespace pgmpm.Diff
{
    /// <summary>
    /// The factory which creates the difference object.
    /// </summary>
    /// <author>Christopher Licht</author>
    public static class DiffFactory
    {
        /// <summary>
        /// Creates a new difference object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns>Returns a difference object.</returns>
        /// <author>Christopher Licht</author>
        public static T CreateDifferenceObject<T>(params object[] args)
        {
            return (T)Activator.CreateInstance(typeof(T), args);
        }
    }
}