using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace pgmpm.MatrixSelection.Dimensions
{
    /// <summary>
    /// A collection of strings that identify the names of Database tables and columns, as well as a collection of sub-ListOfDimensions.
    /// </summary>
    /// <author>Jannik Arndt, Bernhard Bruns, Moritz Eversmann</author>
    [Serializable()]
    public class Dimension : ISerializable, INotifyPropertyChanged
    {
        [XmlIgnore]
        public string Name { get; set; }

        public string Dimensionname { get; set; }

        [XmlIgnore]
        public string FromConstraint { get; set; }

        [XmlIgnore]
        public string FromTable { get; set; }

        [XmlIgnore]
        public string FromColumn { get; set; }

        [XmlIgnore]
        public string ToConstraint { get; set; }

        [XmlIgnore]
        public string ToTable { get; set; }

        public string Level { get; set; }

        [XmlIgnore]
        public string ToColumn { get; set; }

        [XmlIgnore]
        public bool IsEmptyDimension = false;

        [XmlIgnore]
        public bool IsAllDimension = false;

        public List<Dimension> DimensionLevelsList = new List<Dimension>();

        [XmlIgnore]
        public List<DimensionContent> DimensionContentsList = new List<DimensionContent>();

        [XmlIgnore]
        public DimensionColumnNames DimensionColumnNames = new DimensionColumnNames();

        public event PropertyChangedEventHandler PropertyChanged;

        [XmlIgnore]
        public string LevelChanged
        {
            get { return Level; }
            set
            {
                Level = value;
                OnPropertyChanged("Level");
            }
        }

        [XmlIgnore]
        public string DimensionnameChanged
        {
            get { return Dimensionname; }
            set
            {
                Dimensionname = value;
                OnPropertyChanged("Dimensionname");
            }
        }

        /// <summary>
        /// If a property get changed, this method should be call
        /// </summary>
        /// <param name="name"></param>
        /// <author>Bernhard Bruns</author>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }                   
        }


        /// <summary>
        /// Saves a list of all sub-ListOfDimensions, which essentially is the level of this particular dimension, IF you are at the top level.
        /// </summary>
        private List<Dimension> LevelList = new List<Dimension>();
        private bool _levelListIsBuilt = false;

        /// <summary>
        /// Parameterless constructor for the xml-serializer
        /// </summary>
        /// <author>Bernhard Bruns</author>
        private Dimension()
        {

        }

        public Dimension(string name = "", string fromContraint = "", string fromTable = "", string fromColumn = "",
            string toConstraint = "", string toTable = "", string toColumn = "", bool isEmptyDimension = false, bool isAllDimension = false)
        {
            Name = name;
            FromConstraint = fromContraint;
            FromTable = fromTable;
            FromColumn = fromColumn;
            ToConstraint = toConstraint;
            ToTable = toTable;
            ToColumn = toColumn;
            IsEmptyDimension = isEmptyDimension;
            IsAllDimension = isAllDimension;

            Dimensionname = name;
            Level = toTable;
        }

        /// <summary>
        /// Constructor to deserialize
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        /// <author>Jannik Arndt</author>
        public Dimension(SerializationInfo info, StreamingContext ctxt)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            FromConstraint = (string)info.GetValue("FromConstraint", typeof(string));
            FromTable = (string)info.GetValue("FromTable", typeof(string));
            FromColumn = (string)info.GetValue("FromColumn", typeof(string));
            ToConstraint = (string)info.GetValue("ToConstraint", typeof(string));
            ToTable = (string)info.GetValue("ToTable", typeof(string));
            ToColumn = (string)info.GetValue("ToColumn", typeof(string));
            IsEmptyDimension = (bool)info.GetBoolean("IsEmptyDimension");
            DimensionLevelsList = (List<Dimension>)info.GetValue("DimensionLevelsList", typeof(List<Dimension>));
            DimensionContentsList = (List<DimensionContent>)info.GetValue("DimensionContentsList", typeof(List<DimensionContent>));
            DimensionColumnNames = (DimensionColumnNames)info.GetValue("DimensionColumnNames", typeof(DimensionColumnNames));
            LevelList = (List<Dimension>)info.GetValue("LevelList", typeof(List<Dimension>));
            _levelListIsBuilt = (bool)info.GetValue("LevelListIsBuilt", typeof(bool));
        }

        /// <summary>
        /// Gets the objects for serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        /// <author>Jannik Arndt</author>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Name", Name);
            info.AddValue("FromConstraint", FromConstraint);
            info.AddValue("FromTable", FromTable);
            info.AddValue("FromColumn", FromColumn);
            info.AddValue("ToConstraint", ToConstraint);
            info.AddValue("ToTable", ToTable);
            info.AddValue("ToColumn", ToColumn);
            info.AddValue("IsEmptyDimension", IsEmptyDimension);
            info.AddValue("DimensionLevelsList", DimensionLevelsList);
            info.AddValue("DimensionContentsList", DimensionContentsList);
            info.AddValue("DimensionColumnNames", DimensionColumnNames);
            info.AddValue("LevelList", LevelList);
            info.AddValue("LevelListIsBuilt", _levelListIsBuilt);
        }

        /// <summary>
        /// Adds a single Dimension (usually for coarser level).
        /// </summary>
        /// <param Name="dimension">A Dimension-Object</param>
        /// <author>Jannik Arndt</author>
        public void AddDimensionLevel(Dimension dimension)
        {
            DimensionLevelsList.Add(dimension);
        }

        /// <summary>
        /// Adds a a List of Dimensions (usually for coarser level).
        /// </summary>
        /// <param Name="listOfDimensions">A List-Object</param>
        /// <author>Jannik Arndt</author>
        public void AddDimensionLevel(List<Dimension> listOfDimensions)
        {
            DimensionLevelsList.AddRange(listOfDimensions);
        }

        /// <summary>
        /// Returns the List of Dimensions
        /// </summary>
        /// <returns>A list of sub-ListOfDimensions</returns>
        /// <author>Jannik Arndt</author>
        public List<Dimension> GetCoarserDimensionLevel()
        {
            return DimensionLevelsList;
        }

        /// <summary>
        /// Decends into the sub-ListOfDimensions to return the dimension at the specified level
        /// </summary>
        /// <param Name="depth">Usually the level_depth-parameter of a SelectedDimension-Object</param>
        /// <returns>The dimension at the given level</returns>
        /// <author>Jannik Arndt</author>
        public Dimension GetDimensionAtDepth(int depth)
        {
            if (depth > 0 && DimensionLevelsList != null)
            {
                if (DimensionLevelsList.Count > 0)
                    return DimensionLevelsList[0].GetDimensionAtDepth(depth - 1);
                return this;
            }
            return this;
        }

        /// <summary>
        /// Returns the list of sub-ListOfDimensions (=level of this dimension).
        /// If it is not built yet, it will be built.
        /// </summary>
        /// <returns>A List where each entry corresponds to a dimension-level.</returns>
        /// <author>Christopher Licht</author>
        public List<Dimension> GetLevel()
        {
            if (!_levelListIsBuilt)
            {
                //Prevents the elements do not contain multiple in the combo boxes.
                _levelListIsBuilt = true;
                BuildLevelList(this);
            }
            return LevelList;
        }

        /// <summary>
        /// Private method to build the list of sub-ListOfDimensions (=level) recursively.
        /// </summary>
        /// <param Name="dimension">Start with "this".</param>
        /// <author>Jannik Arndt</author>
        private void BuildLevelList(Dimension dimension)
        {
            LevelList.Add(dimension);
            if (dimension.DimensionLevelsList.Count > 0)
                BuildLevelList(dimension.DimensionLevelsList[0]);
        }
    }
}
