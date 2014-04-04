using System;
using System.Collections.Generic;

namespace pgmpm.MiningAlgorithm
{
    /// <summary>
    /// This class stores all key-value-pairs that any mining-algorithm wants to store.
    /// </summary>
    /// <author>Jannik Arndt</author>
    public static class MinerSettings
    {
        public static Dictionary<string, object> ListOfMinerSettings = new Dictionary<string, object>();
        public static String MinerName = "";
        public static string MinerURI = "";
        public static bool IsAlgorithmSet = false;

        /// <summary>
        /// Checks if a key exists, updates it or otherwise creates and sets it.
        /// </summary>
        /// <param name="key">String value as key. For example Alpha Miner</param>
        /// <param name="value">Object as value. For example Heuristic Miner </param>
        /// <author>Jannik Arndt</author>
        public static void AddOrUpdateKey(String key, Object value)
        {
            if (ListOfMinerSettings.ContainsKey(key))
                ListOfMinerSettings[key] = value;
            else
                ListOfMinerSettings.Add(key, value);
        }

        /// <summary>
        /// Get the value (object) to a given key.
        /// </summary>
        /// <param name="key">The key is necessary to find the value</param>
        /// <returns>Returns the object by the key</returns>
        /// <author>Jannik Arndt</author>
        public static object Get(string key)
        {
            if (ListOfMinerSettings.ContainsKey(key))
                return ListOfMinerSettings[key];
            return null;
        }

        /// <summary>
        /// Returns a dictionary-object as a double
        /// </summary>
        /// <param name="key">The key is necessary to find the value</param>
        /// <returns>Returns a dictionary-object as a double</returns>
        /// <author>Jannik Arndt</author>
        public static double GetAsDouble(string key)
        {
            if (ListOfMinerSettings.ContainsKey(key))
                return Convert.ToDouble(ListOfMinerSettings[key]);
            return -1;
        }

        /// <summary>
        /// Returns a dictionary-object as an integer.
        /// </summary>
        /// <param name="key">The key is necessary to find the object</param>
        /// <returns>Returns a dictionary-object as an integer</returns>
        /// <author>Jannik Arndt</author>
        public static int GetAsInt(string key)
        {
            if (ListOfMinerSettings.ContainsKey(key))
                return Convert.ToInt32(ListOfMinerSettings[key]);
            return -1;
        }

        /// <summary>
        /// Returns a dictionary-object as a String.
        /// </summary>
        /// <param name="key">The key is necessary to find the object</param>
        /// <returns>Returns a dictionary-object as a String</returns>
        /// <author>Krystian Zielonka</author>
        public static String GetAsString(string key)
        {
            if (ListOfMinerSettings.ContainsKey(key))
                return Convert.ToString(ListOfMinerSettings[key]);
            return null;
        }
    }
}
