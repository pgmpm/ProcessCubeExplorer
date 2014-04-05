using System;
using System.Collections.Generic;

namespace pgmpm.Consolidation
{
    public static class ConsolidatorSettings
    {
        public static Dictionary<string, object> ListOfConsolidationSettings = new Dictionary<string, object>();
        public static List<String> ConsolidationOptions = new List<string> { "Loop", "Parallelism", "Events", "Min. Number of Events"};
        public static Type ConsolidationType { get; set; }
        public static Type ProcessModelType { get; set; }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="key"></param>
       /// <param name="value"></param>
        public static void AddOrUpdateKey(String key, Object value)
        {
            if (ListOfConsolidationSettings.ContainsKey(key))
                ListOfConsolidationSettings[key] = value;
            else
                ListOfConsolidationSettings.Add(key, value);
        }

     /// <summary>
     /// 
     /// </summary>
     /// <param name="key"></param>
     /// <returns></returns>
        public static object Get(string key)
        {
            if (ListOfConsolidationSettings.ContainsKey(key))
                return ListOfConsolidationSettings[key];
            return null;
        }
    }
}
