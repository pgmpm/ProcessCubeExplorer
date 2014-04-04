using System;
using System.Collections.Generic;

namespace pgmpm.Diff.XMLParser.MappingParser
{
    class Mapping
    {
        //Attribute declaration:
        private readonly Dictionary<string, MappingRule> _rules;

        public string MappingName = "Untitled";
        internal string Namespace = "";
        public List<Pattern> Patterns;

        /// <summary>
        /// Add Mapping Rule to List if it is ok.
        /// </summary>
        /// <param name="rule">Rule to add</param>
        /// <returns></returns>
        /// <author>Carsten Cordes, Thomas Meents</author>
        public bool AddRule(MappingRule rule)
        {
            //Don't add incorrect rules.
            if (rule == null || !rule.IsValid())
                return false;
            try
            {
                _rules.Add(rule.TypeName.ToLower(), rule);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Add Pattern to list.
        /// </summary>
        /// <param name="pattern">Pattern to add</param>
        /// <author>Carsten Cordes, Thomas Meents</author>
        public void AddPattern(Pattern pattern)
        {
            if (pattern != null)
            {
                Patterns.Add(pattern);
            }
        }

    }
}
