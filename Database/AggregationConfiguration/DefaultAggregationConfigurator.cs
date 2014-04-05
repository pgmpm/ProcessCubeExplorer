using System.Collections.Generic;

namespace pgmpm.Database.AggregationConfiguration
{
    /// <summary>
    /// Default implementation of AggregationConfigurator. 
    /// This class defines the SQL functions which should be used in an aggregation.
    /// (e.g. if our SQL-Statement contains a group by clause we have to use SQL functions in select clause to aggregate attribute values)
    /// </summary>
    /// <author>Roman Bauer</author>
    class DefaultAggregationConfigurator : AbstractAggregationConfigurator
    {
        /// <summary>
        /// Default implementation that can be used with all databases. It associates each given attribute with the SQL function MIN().
        /// If you want to use other functions you have to derive your own class from AbstractAggregationConfigurator and implement this method.
        /// /// <param name="attributes">List with attribute names</param>
        /// </summary>
        protected override void MapFunctionsOnAttributes(List<string> attributes)
        {
            foreach (string attribute in attributes)
            {
                AttributeToFunctionMap.Add(attribute, "MIN");
            }
        }
    }
}
