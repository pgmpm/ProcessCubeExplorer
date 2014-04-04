using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace pgmpm.MatrixSelection.Fields
{
    /// <summary>
    /// A class for cases.
    /// The <see cref="EventLog"/> class adds cases in a list.
    /// </summary>
    /// <author>Bernhard Bruns, Jannik Arndt</author>
    [Serializable]
    public class Case : ISerializable
    {
        public String Name { get; set; }

        public List<Event> EventList { get; set; }
        public Dictionary<string, string> AdditionalData = new Dictionary<string, string>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventList"></param>
        public Case(String name = "", List<Event> eventList = null)
        {
            EventList = eventList ?? new List<Event>();
            Name = name;
        }

        /// <summary>
        /// Constructor to deserialize
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        /// <author>Jannik Arndt</author>
        public Case(SerializationInfo info, StreamingContext ctxt)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            EventList = (List<Event>)info.GetValue("events", typeof(List<Event>));
            AdditionalData = (Dictionary<string, string>) info.GetValue("AdditionalData", typeof (Dictionary<string, string>));
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
            info.AddValue("events", EventList);
            info.AddValue("AdditionalData", AdditionalData);
        }

        public override string ToString()
        {
            return string.Join(", ", EventList);
        }

        /// <summary>
        /// Creates a new Event and adds it to a <see cref="Case"/>.
        /// </summary>
        /// <param name="name">The Name of the new event</param>
        public Event CreateEvent(String name)
        {
            Event item = new Event(name);
            EventList.Add(item);
            return item;
        }

        /// <summary>
        /// Returns how often e1 is followed by e2.
        /// </summary>
        /// <param name="event1">The first event</param>
        /// <param name="event2">The second event, which must follow the first event directly</param>
        /// <returns>The count of what IndexesOfPair returns.</returns>
        /// <author>Jannik Arndt</author>
        public int EventFollowsEventCount(Event event1, Event event2)
        {
            return IndexesOfPair(event1, event2).Count;
        }

        /// <summary>
        /// Returns a list of integers, which are the indexes of each occurrence of Event e1 and Event e2 directly following each other.
        /// </summary>
        /// <param name="event1">The first event</param>
        /// <param name="event2">The second event, which must follow the first event directly</param>
        /// <returns>A list of indexes</returns>
        /// <author>Jannik Arndt</author>
        public List<int> IndexesOfPair(Event event1, Event event2)
        {
            return Enumerable.Range(0, EventList.Count - 1).Where(i => EventList[i].Name == event1.Name && EventList[i + 1].Name == event2.Name).ToList();
        }
    }
}
