using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace pgmpm.MatrixSelection.Fields
{
    /// <summary>
    /// Provides the base-class for Eventlogs which consists of <see cref="Case"/>.
    /// </summary>
    /// <author>Bernhard Bruns</author>
    [Serializable]
    public class EventLog : ISerializable
    {
        public String Name { get; set; }
        public List<Case> Cases { get; set; }

        private int _casesCountCache = -1;
        private List<Event> _listOfUniqueEvents = new List<Event>();

        /// <summary>
        /// Constructor for an EventLog, built from a FactTable and a few pieces of extra information
        /// </summary>
        /// <param name="name"></param>
        /// <param name="listOfCases">A List of ListOfFacts, usually from a FactTable</param>
        /// <author>Jannik Arndt</author>
        public EventLog(String name = "", List<Case> listOfCases = null)
        {
            Name = name;
            Cases = listOfCases ?? new List<Case>();
        }

        /// <summary>
        /// Constructor to deserialize
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        /// <author>Jannik Arndt</author>
        public EventLog(SerializationInfo info, StreamingContext ctxt)
        {
            Cases = (List<Case>)info.GetValue("cases", typeof(List<Case>));
            Name = (string)info.GetValue("Name", typeof(string));
        }

        /// <summary>
        /// This contains each Event that appears in any of the cases exactly one time (=unique!). 
        /// This method uses a HashSet and is the fastest way to get all the events.
        /// The list is cached but automatically updated if the cases change.
        /// </summary>
        /// <author>Jannik Arndt</author>
        public List<Event> ListOfUniqueEvents
        {
            get
            {
                if (_listOfUniqueEvents.Count != _casesCountCache)
                {
                    HashSet<Event> eventSet = new HashSet<Event>();
                    foreach (Case Case in Cases)
                        eventSet.UnionWith(Case.EventList);
                    _listOfUniqueEvents = eventSet.ToList();
                    _casesCountCache = _listOfUniqueEvents.Count;
                }
                return _listOfUniqueEvents;
            }
        }

        /// <summary>
        /// Gets the objects for serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        /// <author>Jannik Arndt</author>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("cases", Cases);
            info.AddValue("Name", Name);
        }

        /// <summary>
        /// Returns the cases of an eventlog.
        /// </summary>
        /// <param name="casename">The Name of the <see cref="Case"/>.</param>
        /// <returns>Returns the Cases</returns>
        /// <author>Thomas Meents, Christopher Licht</author>
        public Case GetCaseByName(String casename)
        {
            return Cases.FirstOrDefault(Case => Case.Name.Equals(casename));
        }

        /// <summary>
        /// Goes through the cases and counts the events
        /// </summary>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        public int CountEvents()
        {
            return Cases.Sum(Case => Case.EventList.Count);
        }

        /// <summary>
        /// Count how often e2 follows e1.
        /// </summary>
        /// <param name="event1"></param>
        /// <param name="event2"></param>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        public int EventFollowsEvent(Event event1, Event event2)
        {
            return Cases.Sum(Case => Case.EventFollowsEventCount(event1, event2));
        }

        public string ConvertToMXML()
        {
            StringBuilder content = new StringBuilder();
            content.Append("");
            content.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\n");
            content.Append("<!-- MXML version 1.1 -->\n");
            content.Append("<!-- Created by Process Cube Explorer, 2014 -->\n");
            content.Append("<WorkflowLog xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://is.tm.tue.nl/research/processmining/WorkflowLog.xsd\" description=\"Process 1 exported by Process Cube Explorer\">\n");
            content.Append("<Data>\n");
            content.Append("	<Attribute name=\"app.name\">Process Cube Explorer</Attribute>\n");
            content.Append("	<Attribute name=\"app.version\">1.0</Attribute>\n");
            content.Append("</Data>\n");
            content.Append("<Source program=\"Process Cube Explorer\"/>\n");

            //Process:
            const int processID = 0;
            content.Append("    <Process id=\"" + Name + "ID" + processID + "\" description=\"" + Name + "\">\n");

            //Case:
            int caseID = 0;
            foreach (Case ca in Cases)
            {
                content.Append("        <ProcessInstance id=\"" + ca.Name + "ID" + caseID + "\">\n");

                foreach (Event ev in ca.EventList)
                {
                    String timestamp = "";
                    //Parse the dictionary with additional informations:
                    foreach (KeyValuePair<string, string> info in ev.Information)
                    {
                        switch (info.Key.ToLower())
                        {
                            case "timestamp":
                                String dateString = info.Value;
                                DateTime dateValue;
                                if (DateTime.TryParse(dateString, out dateValue))
                                {
                                    DateTime.SpecifyKind(dateValue,
                                        DateTimeKind.Local);

                                    //yyyy-MM-dd'T'HH:mm:ss.SSSZ 1970-01-01T00:00:00.000+0000
                                    timestamp = dateValue.ToString("yyyy-MM-dd'T'HH:mm:ss'.000+0100'");
                                }
                                break;
                        }
                    }

                    //Event:
                    content.Append("            <AuditTrailEntry>\n");
                    content.Append("                <WorkflowModelElement>" + ev.Name + "</WorkflowModelElement>\n");
                    content.Append("                <EventType>complete</EventType>\n");
                    content.Append("                <Timestamp>" + timestamp + "</Timestamp>\n");
                    //content.Append("                <Timestamp>2010-11-09T13:03:41.887+01:00</Timestamp>\n");
                    content.Append("                <Originator>UNDEFINED</Originator>\n");
                    content.Append("            </AuditTrailEntry>\n");
                }

                content.Append("        </ProcessInstance>\n");
                caseID++;
            }

            content.Append("	</Process>\n");

            content.Append("</WorkflowLog>\n");

            return content.ToString();
        }
    }
}
