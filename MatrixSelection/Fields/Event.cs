using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace pgmpm.MatrixSelection.Fields
{
    /// <summary>
    /// A class for events.
    /// The <see cref="Case"/> class adds events in a list.
    /// </summary>
    /// <author>Bernhard Bruns, Jannik Arndt</author>
    [Serializable]
    public class Event : ISerializable
    {
        /// <summary>
        /// The name of the event, which will be displayed when the process model is drawn.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// This dictionary stores all additional information as strings. Use: MyEvent.Information.Add("Event Type", "Procedure");
        /// </summary>
        public Dictionary<string, string> Information { get; set; }

        /// <summary>
        /// Constructor. The name is optional.
        /// </summary>
        /// <param name="name"></param>
        public Event(string name = "")
        {
            Name = name;
            Information = new Dictionary<string, string>();
        }

        /// <summary>
        /// Constructor to deserialize
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        /// <author>Jannik Arndt</author>
        public Event(SerializationInfo info, StreamingContext ctxt)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            Information = (Dictionary<string, string>)info.GetValue("Information", typeof(Dictionary<string, string>));
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
            info.AddValue("Information", Information);
        }

        /// <summary>
        /// Compares two events. They are equal if the descriptions are equal. This is important for the use in a dictionary!
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        public override bool Equals(object obj)
        {
            Event event1 = obj as Event;
            if (obj == null || event1 == null)
                return false;
            return event1.Name == Name;
        }

        /// <summary>
        /// Compares two events. They have the same hashcode if the descriptions are equal. This is important for the use in a dictionary!
        /// </summary>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /// <summary>
        /// Returns the description
        /// </summary>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        public override string ToString()
        {
            return Name;
        }
    }
}
