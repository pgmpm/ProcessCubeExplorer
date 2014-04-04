using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using pgmpm.MatrixSelection.Dimensions;
using pgmpm.MatrixSelection.Fields;

namespace pgmpm.MatrixSelection
{
    /// <summary>
    /// This class keeps track of what ListOfDimensions, levels and filters are selected. It also requests basic information like count(*)
    /// from the Database and calculates the fields that are displayed in the MatrixPreviewGrid.
    /// </summary>
    /// <author>Jannik Arndt</author>
    [Serializable]
    public class MatrixSelectionModel : ISerializable
    {
        public List<Field> MatrixFields = new List<Field>();
        public List<SelectedDimension> SelectedDimensions = new List<SelectedDimension>();

        /// <summary>
        /// If this is true, data needs to be reloaded from db, otherwise cached data may be used
        /// </summary>
        public bool SelectionHasChangedSinceLastLoading = true;
        int _lastMatrixBuild;

        public MatrixSelectionModel()
        {
        }

        /// <summary>
        /// Constructor to deserialize
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        /// <author>Jannik Arndt</author>
        public MatrixSelectionModel(SerializationInfo info, StreamingContext ctxt)
        {
            SelectedDimensions = (List<SelectedDimension>)info.GetValue("selectedDim", typeof(List<SelectedDimension>));
            MatrixFields = (List<Field>)info.GetValue("fields", typeof(List<Field>));
        }

        /// <summary>
        /// Gets the objects for serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        /// <author>Jannik Arndt</author>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("selectedDim", SelectedDimensions);
            info.AddValue("fields", MatrixFields);
        }

        /// <summary>
        /// Initializes the MatrixSelection-class by filling the SelectedDimensions-List.
        /// </summary>
        /// <param name="selectedDimensionsSize">Should be the same as there are selectors in the GUI.</param>
        public void Init(int selectedDimensionsSize)
        {
            SelectedDimensions.Clear();
            for (int i = 0; i < selectedDimensionsSize; i++)
                SelectedDimensions.Add(new SelectedDimension());
        }

        /// <summary>
        /// Calculates a list of Field-Objects that represents the matrix in consecutive rows. It creates one object for each combination of chosen filters.
        /// </summary>
        /// <returns></returns>
        public void BuildMatrixFields()
        {
            MatrixFields = new List<Field>();
            foreach (DimensionContent dc1 in SelectedDimensions[1].SelectedFilters)
            {
                foreach (DimensionContent dc2 in SelectedDimensions[0].SelectedFilters)
                {
                    Field newField = new Field
                    {
                        Dimension1 = SelectedDimensions[0],
                        Dimension2 = SelectedDimensions[1],
                        DimensionContent1 = dc2, 
                        DimensionContent2 = dc1, 
                        AdditionalFiltersList = new List<SelectedDimension>()
                    };

                    foreach (SelectedDimension filterDimension in SelectedDimensions.GetRange(2, SelectedDimensions.Count - 2))
                        if (filterDimension.Dimension.IsEmptyDimension == false)
                            newField.AdditionalFiltersList.Add(filterDimension);

                    MatrixFields.Add(newField);
                }
            }
        }


        /// <summary>
        /// Returns the MatrixFields, either from cache or newly built if axis 1 or 2 have changed.
        /// </summary>
        /// <returns></returns>
        public List<Field> GetFields()
        {
            if (_lastMatrixBuild != SelectedDimensions[0].SelectedFilters.Count + SelectedDimensions[1].SelectedFilters.Count)
            {
                BuildMatrixFields();
                _lastMatrixBuild = SelectedDimensions[0].SelectedFilters.Count + SelectedDimensions[1].SelectedFilters.Count;
            }
            return MatrixFields;
        }
    }
}
