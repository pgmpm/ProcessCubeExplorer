using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using pgmpm.MatrixSelection.Dimensions;
using pgmpm.MatrixSelection.Properties;
using pgmpm.Model;
using pgmpm.Model.PetriNet;

namespace pgmpm.MatrixSelection.Fields
{
    /// <summary>
    /// This abstract datatype stores the information on ONE field in the MatrixPreviewGrid.
    /// </summary>
    /// <author>Jannik Arndt,Thomas Meents, Bernhard Bruns</author>
    [Serializable]
    public class Field : ISerializable, INotifyPropertyChanged
    {
        public Dictionary<string, string> Information;

        public Field()
        {
            EventLog = new EventLog();
            DimensionContent1 = new DimensionContent();
            DimensionContent2 = new DimensionContent();
            AdditionalFiltersList = new List<SelectedDimension>();
            ResetInformation();
        }

        /// <summary>
        /// Constructor to deserialize
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        /// <author>Jannik Arndt</author>
        public Field(SerializationInfo info, StreamingContext ctxt)
        {
            EventLog = (EventLog)info.GetValue("log", typeof(EventLog));
            DimensionContent1 = (DimensionContent)info.GetValue("DimensionContent1", typeof(DimensionContent));
            DimensionContent2 = (DimensionContent)info.GetValue("DimensionContent2", typeof(DimensionContent));
            ContentCounterText = (string)info.GetValue("ContentCounterText", typeof(string));
            AdditionalFiltersList = (List<SelectedDimension>)info.GetValue("AdditionalFiltersList", typeof(List<SelectedDimension>));
            ProcessModel = (ProcessModel)info.GetValue("ProcessModel", typeof(ProcessModel));
            Information = (Dictionary<string, string>)info.GetValue("Information", typeof(Dictionary<string, string>));
        }

        public List<SelectedDimension> AdditionalFiltersList { get; set; }
        public String SqlCode { get; set; }
        public EventLog EventLog { get; set; }
        public DimensionContent DimensionContent1 { get; set; }
        public DimensionContent DimensionContent2 { get; set; }
        public SelectedDimension Dimension1 { get; set; }
        public SelectedDimension Dimension2 { get; set; }
        public ProcessModel ProcessModel { get; set; }
        public string ContentCounterText { get; set; }

        public string ContentCounterTextChanged
        {
            get { return ContentCounterText; }
            set
            {
                ContentCounterText = value;
                OnPropertyChanged("Infotext");
            }
        }

        public string Infotext
        {
            get
            {
                if (DimensionContent1 != null && DimensionContent2 != null && ContentCounterText != null && ContentCounterText != "")
                    return DimensionContent1.Description + ", " + DimensionContent2.Description + ": " + ContentCounterText;
                if (DimensionContent1 != null && DimensionContent2 != null)
                    return DimensionContent1.Description + ", " + DimensionContent2.Description;

                return "";
            }
        }

        /// <summary>
        /// Returns the color of the indicator of the field (green, yellow, red), depending on the events used and the ProcessModelPercentOfQuality-Setting.
        /// </summary>
        /// <author>Thomas Meents, Jannik Arndt</author>
        public String ProcessModelQualityColor
        {
            get
            {
                var thresholdYellow = Convert.ToDouble(Settings.Default.ProcessModelPercentOfQuality) / 100;
                try
                {
                    var petriNet = (PetriNet)ProcessModel;
                    if (ProcessModel == null || petriNet.GetSources().Count() != 1 || petriNet.GetSinks().Count() != 1)
                        return "Red";
                    var array = Information["Events used"].Split(' ');
                    var usedEvents = Convert.ToDouble(array[0]);
                    var totalEvents = Convert.ToDouble(array[2]);
                    var percentEvents = usedEvents / totalEvents;
                    return percentEvents < thresholdYellow ? "Yellow" : "Green";
                }
                catch (Exception)
                {
                    return "Red";
                }
            }
        }

        /// <summary>
        /// Activate the checkbox if comparable
        /// </summary>
        /// <author>Thomas Meents</author>
        public Boolean ProcessModelCompareCheckbox
        {
            get
            {
                var petriNet = (PetriNet)ProcessModel;
                return ProcessModel != null && petriNet.GetSources().Count() == 1 && petriNet.GetSinks().Count() == 1;
            }
        }

        /// <summary>
        /// EventHandler for the Infotext-Property
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the objects for serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        /// <author>Jannik Arndt</author>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("log", EventLog);
            info.AddValue("DimensionContent1", DimensionContent1);
            info.AddValue("DimensionContent2", DimensionContent2);
            info.AddValue("ContentCounterText", ContentCounterText);
            info.AddValue("AdditionalFiltersList", AdditionalFiltersList);
            info.AddValue("ProcessModel", ProcessModel);
            info.AddValue("Information", Information);
        }

        /// <summary>
        /// If a property get changed, this method should be call
        /// </summary>
        /// <param name="name"></param>
        /// <author>Bernhard Bruns</author>
        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Resets the Information-Dictionary and fills it with information from the field
        /// </summary>
        /// <author>Jannik Arndt</author>
        public void ResetInformation()
        {
            try
            {
                Information = new Dictionary<string, string>
                {
                    {"1. " + Dimension1.Dimension.Dimensionname, DimensionContent1.Description},
                    {"2. " + Dimension2.Dimension.Dimensionname, DimensionContent2.Description}
                };

                foreach (var selectedDimension in AdditionalFiltersList)
                    Information.Add(selectedDimension.Dimension.Dimensionname,
                        string.Join(", ", selectedDimension.SelectedFilters.Select(dimensionContent => dimensionContent.Description)));

                Information.Add("Cases in Eventlog", EventLog.Cases.Count.ToString(CultureInfo.InvariantCulture));
                Information.Add("Events in Eventlog", EventLog.CountEvents().ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception)
            {
                Information = new Dictionary<string, string>();
            }
        }
    }
}