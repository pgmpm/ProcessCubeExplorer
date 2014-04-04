using pgmpm.MatrixSelection.Dimensions;
using System;
using System.Collections.Generic;

namespace pgmpm.MatrixSelection
{
    /// <summary>
    /// This class keeps track of what event dimensions, levels and filters are selected. It is implement as singleton.
    /// </summary>
    /// <author>Roman Bauer</author>
    [Serializable()]
    public class EventSelectionModel
    {
        private static EventSelectionModel _instance;

        private List<SelectedDimension> _selectedDimensions;

        public List<SelectedDimension> SelectedDimensions
        {
            get { return _selectedDimensions; }
            set { _selectedDimensions = value; }
        }

        /// If this is true, data needs to be reloaded from db, otherwise cached data may be used
        private bool _selectionHasChangedSinceLastLoading = true;

        public bool SelectionHasChangedSinceLastLoading
        {
            get { return _selectionHasChangedSinceLastLoading; }
            set { _selectionHasChangedSinceLastLoading = value; }
        }

        private EventSelectionModel()
        {

        }

        /// <summary>
        /// Return event selection model.
        /// </summary>
        /// <param Name="selectedDimensions"></param>
        public static EventSelectionModel GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EventSelectionModel();
            }

            if (_instance.SelectedDimensions == null)
            {
                _instance.SelectedDimensions = new List<SelectedDimension>();
            }

            return _instance;
        }


        /// <summary>
        /// Add a selected dimension to the event selection model.
        /// </summary>
        /// <param Name="selectedDimension"></param>
        public void AddSelectedDimension(SelectedDimension selectedDimension)
        {
            _instance.SelectedDimensions.Add(selectedDimension);
        }

        /// <summary>
        /// Update internal event selection model.
        /// </summary>
        /// <param Name="axis"></param>
        /// /// <param Name="dim"></param>
        /// /// <param Name="levelDepth"></param>
        /// /// <param Name="dimContent"></param>
        /// <param name="aggregationDepth"></param>
        public void Update(int axis, Dimension dim, int levelDepth, int aggregationDepth, List<DimensionContent> dimContent)
        {
            SelectedDimension showEmptyDimension = null;

            foreach (SelectedDimension selectedDimension in _instance.SelectedDimensions)
            {
                if (selectedDimension.Axis == axis)
                {
                    if (dim.IsEmptyDimension)
                    {
                        showEmptyDimension = selectedDimension;
                    }
                    else
                    {
                        selectedDimension.Dimension = dim;
                        selectedDimension.LevelDepth = levelDepth;
                        selectedDimension.AggregationDepth = aggregationDepth - 1; //-1 exclude the logical no aggregation dimension
                        selectedDimension.SelectedFilters = dimContent;

                        if (allLevelSelected(selectedDimension.Dimension, selectedDimension.LevelDepth))
                        {
                            selectedDimension.IsAllLevelSelected = true;
                        } else {
                            selectedDimension.IsAllLevelSelected = false;
                        }
                    }
                    break;
                }
            }

            if (showEmptyDimension != null)
            {
                _instance.SelectedDimensions.Remove(showEmptyDimension);
            }
            _instance.SelectionHasChangedSinceLastLoading = true;
        }

        /// <summary>
        /// Clear internal event selection model.
        /// </summary>
        public void Clear()
        {
            _instance.SelectedDimensions.Clear();
        }

        public bool isEmpty()
        {
            foreach (SelectedDimension selectedDimension in _instance.SelectedDimensions)
            {
                if (selectedDimension.Dimension.IsEmptyDimension)
                {
                    continue;
                }
                return false;
            }
            return true;
        }

        private bool allLevelSelected(Dimension dimension, int levelDepth)
        {
            if (levelDepth > 1)
            {
                return allLevelSelected(dimension.DimensionLevelsList[0], levelDepth - 1);
            }

            return dimension.FromConstraint == "MPMALLAGGREGATION";
        }
    }
}