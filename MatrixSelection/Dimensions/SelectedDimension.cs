using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace pgmpm.MatrixSelection.Dimensions
{
    /// <summary>
    /// An abstract datatype to store the information the User inputs for one dimension
    /// </summary>
    /// <author>Jannik Arndt</author>
    [Serializable()]
    public class SelectedDimension : ISerializable
    {
        /// <summary>
        /// The axis on which this dimension is shown
        /// </summary>
        public int Axis { get; set; }
        /// <summary>
        /// The dimension-object in its finest level (e.g. day, age in years)
        /// </summary>
        public Dimension Dimension { get; set; }
        /// <summary>
        /// The selected depth of level steps, which is the selectedItem-Value of the ComboBox
        /// </summary>
        public int LevelDepth { get; set; }
        /// <summary>
        /// The selected depth of level steps, which is the selectedItem-Value of the ComboBox
        /// </summary>
        public int AggregationDepth { get; set; }
        /// <summary>
        /// Stores if any filters are applied
        /// </summary>
        public bool AreFiltersSelected = false;
        /// <summary>
        /// The selected filters. Check applyFilters first!
        /// </summary>
        public List<DimensionContent> SelectedFilters = new List<DimensionContent>();
        /// <summary>
        /// Helper for the logical all aggregation
        /// </summary>
        public bool IsAllLevelSelected = false;

        public SelectedDimension(int axis = 0, Dimension dimension = null, int levelDepth = 0, int aggregationDepth = 0, bool areFiltersSelected = false, List<DimensionContent> selectedFilters = null, bool isAllLevel = false)
        {
            IsAllLevelSelected = isAllLevel;
            Axis = axis;
            Dimension = dimension ?? new Dimension();
            LevelDepth = levelDepth;
            AggregationDepth = aggregationDepth;
            AreFiltersSelected = areFiltersSelected;
            SelectedFilters = selectedFilters ?? new List<DimensionContent>();
        }

        /// <summary>
        /// Constructor to deserialize
        /// </summary>
        /// <param Name="info"></param>
        /// <param Name="ctxt"></param>
        /// <author>Jannik Arndt</author>
        public SelectedDimension(SerializationInfo info, StreamingContext ctxt)
        {
            Axis = (int)info.GetValue("axis", typeof(int));
            Dimension = (Dimension)info.GetValue("dimension", typeof(Dimension));
            LevelDepth = (int)info.GetValue("level_depth", typeof(int));
            AreFiltersSelected = (bool)info.GetValue("applyFilters", typeof(bool));
            SelectedFilters = (List<DimensionContent>)info.GetValue("SelectedFilters", typeof(List<DimensionContent>));
        }

        /// <summary>
        /// Gets the objects for serialization
        /// </summary>
        /// <param Name="info"></param>
        /// <param Name="ctxt"></param>
        /// <author>Jannik Arndt</author>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("axis", Axis);
            info.AddValue("dimension", Dimension);
            info.AddValue("level_depth", LevelDepth);
            info.AddValue("applyFilters", AreFiltersSelected);
            info.AddValue("SelectedFilters", SelectedFilters);
        }
    }
}
