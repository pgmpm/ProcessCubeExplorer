using System.Collections.Generic;

namespace pgmpm.Database.AggregationConfiguration
{
    /// <summary>
    /// Base implementation of AggregationConfigurator. 
    /// An AggregationConfigurator defines the SQL functions which should be used in an aggregation.
    /// (e.g. if our SQL-Statement contains a group by clause we have to use SQL functions in select clause to aggregate attribute values)
    /// </summary>
    /// <author>Roman Bauer</author>
    abstract class AbstractAggregationConfigurator
    {
        /// <summary>
        /// Map that contains each attribute and its sql function that should be used in an aggregation.
        /// </summary>
        private readonly Dictionary<string, string> _attributeToFunctionMap;

        protected Dictionary<string, string> AttributeToFunctionMap
        {
            get
            {
                return _attributeToFunctionMap;
            }
        }

        /// <summary>
        /// This is constructor is called if a derived class is instantiated.
        /// </summary>
        public AbstractAggregationConfigurator()
        {
            //create new map
            _attributeToFunctionMap = new Dictionary<string, string>();
            //map a function to each attribute
            MapFunctionsOnAttributes(DBWorker.MetaData.ListOfEventsTableColumnNames);
        }

        /// <summary>
        /// Returns the SQL function for the given attribute. The returned SQL function have to be used in an aggregation.
        /// <param name="attributeName">The attributes name</param>
        /// </summary>
        public string GetAggregationFunction(string attributeName)
        {
            if (attributeName != null)
            {
                string aggFunction = _attributeToFunctionMap[attributeName];
                if (aggFunction != null)
                {
                    return aggFunction;
                }
                return null;
            }
            return null;
        }

        /// <summary>
        /// Returns the SQL function that can be used as default function for each attribute.
        /// </summary>
        public string GetDefaultAggregationFunction()
        {
            return "MIN";
        }

        /// <summary>
        /// This method have to be implemented by a derived class. It associates each given attribute with a SQL function.
        /// <param name="attributeNames">List with attribute names</param>
        /// </summary>
        protected abstract void MapFunctionsOnAttributes(List<string> attributeNames);

    }
}
