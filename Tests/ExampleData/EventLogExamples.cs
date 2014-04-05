using System.Collections.Generic;
using pgmpm.MatrixSelection.Fields;

namespace pgmpm.ExampleData
{
    /// <summary>
    /// Event Log examples for program tests.
    /// </summary>
    /// <author>Markus Holznagel</author>
    public class EventLogExample
    {
        /// <summary>
        /// This method creates an event log with one case.
        /// </summary>
        /// <autor>Andrej Albrecht, Markus Holznagel</autor>
        public static EventLog OneCaseEventLogWithFourOrderedEvents()
        {
            //Create case
            Case ca = new Case();
            ca.CreateEvent("A");
            ca.CreateEvent("B");
            ca.CreateEvent("C");
            ca.CreateEvent("D");

            //Create event log
            EventLog eventLog = new EventLog();

            return eventLog;
        }

        /// <summary>
        /// This method creates an event log with one case.
        /// </summary>
        /// <autor>Andrej Albrecht, Markus Holznagel</autor>
        public static EventLog OneCaseEventLogSwitchTheMiddle()
        {
            //Create case
            Case ca = new Case();
            ca.CreateEvent("A");
            ca.CreateEvent("C");
            ca.CreateEvent("B");
            ca.CreateEvent("D");

            //Create event log
            EventLog eventLog = new EventLog();

            return eventLog;
        }

        /// <summary>
        /// This method creates an event log with two cases.
        /// </summary>
        /// <autor>Markus Holznagel</autor>
        public static EventLog TwoCaseEventLog()
        {
            //Create case1
            Case ca = new Case();
            ca.CreateEvent("A");
            ca.CreateEvent("B");
            ca.CreateEvent("C");
            ca.CreateEvent("D");

            //Create case2
            Case ca2 = new Case();
            ca2.CreateEvent("A");
            ca2.CreateEvent("C");
            ca2.CreateEvent("B");
            ca2.CreateEvent("D");

            //Create event log
            EventLog eventLog = new EventLog();
            eventLog.Cases.Add(ca);
            eventLog.Cases.Add(ca2);

            return eventLog;
        }

        /// <summary>
        /// This method creates an complex event log from the Process Mining book 
        /// of van der Aalst site 195 Table 7.1.
        /// </summary>
        /// <autor>Markus Holznagel</autor>
        public static EventLog ComplexEventLogVanDerAalst()
        {
            //create Case1
            Case ca1 = new Case();
            ca1.CreateEvent("A");
            ca1.CreateEvent("C");
            ca1.CreateEvent("D");
            ca1.CreateEvent("E");
            ca1.CreateEvent("H");

            //create Case2
            Case ca2 = new Case();
            ca2.CreateEvent("A");
            ca2.CreateEvent("B");
            ca2.CreateEvent("D");
            ca2.CreateEvent("E");
            ca2.CreateEvent("G");

            //create Case3
            Case ca3 = new Case();
            ca3.CreateEvent("A");
            ca3.CreateEvent("D");
            ca3.CreateEvent("C");
            ca3.CreateEvent("E");
            ca3.CreateEvent("H");

            //create Case4
            Case ca4 = new Case();
            ca4.CreateEvent("A");
            ca4.CreateEvent("B");
            ca4.CreateEvent("D");
            ca4.CreateEvent("E");
            ca4.CreateEvent("H");

            //create Case5
            Case ca5 = new Case();
            ca5.CreateEvent("A");
            ca5.CreateEvent("C");
            ca5.CreateEvent("D");
            ca5.CreateEvent("E");
            ca5.CreateEvent("G");

            //create Case6
            Case ca6 = new Case();
            ca6.CreateEvent("A");
            ca6.CreateEvent("D");
            ca6.CreateEvent("C");
            ca6.CreateEvent("E");
            ca6.CreateEvent("G");

            //create Case7
            Case ca7 = new Case();
            ca7.CreateEvent("A");
            ca7.CreateEvent("D");
            ca7.CreateEvent("B");
            ca7.CreateEvent("E");
            ca7.CreateEvent("H");

            //create Case8
            Case ca8 = new Case();
            ca8.CreateEvent("A");
            ca8.CreateEvent("C");
            ca8.CreateEvent("D");
            ca8.CreateEvent("E");
            ca8.CreateEvent("F");
            ca8.CreateEvent("D");
            ca8.CreateEvent("B");
            ca8.CreateEvent("E");
            ca8.CreateEvent("H");

            //create Case9
            Case ca9 = new Case();
            ca9.CreateEvent("A");
            ca9.CreateEvent("D");
            ca9.CreateEvent("B");
            ca9.CreateEvent("E");
            ca9.CreateEvent("G");

            //create Case10
            Case ca10 = new Case();
            ca10.CreateEvent("A");
            ca10.CreateEvent("C");
            ca10.CreateEvent("D");
            ca10.CreateEvent("E");
            ca10.CreateEvent("F");
            ca10.CreateEvent("B");
            ca10.CreateEvent("D");
            ca10.CreateEvent("E");
            ca10.CreateEvent("H");

            //create Case11
            Case ca11 = new Case();
            ca11.CreateEvent("A");
            ca11.CreateEvent("C");
            ca11.CreateEvent("D");
            ca11.CreateEvent("E");
            ca11.CreateEvent("F");
            ca11.CreateEvent("B");
            ca11.CreateEvent("D");
            ca11.CreateEvent("E");
            ca11.CreateEvent("G");

            //create Case12
            Case ca12 = new Case();
            ca12.CreateEvent("A");
            ca12.CreateEvent("C");
            ca12.CreateEvent("D");
            ca12.CreateEvent("E");
            ca12.CreateEvent("F");
            ca12.CreateEvent("D");
            ca12.CreateEvent("B");
            ca12.CreateEvent("E");
            ca12.CreateEvent("G");

            //create Case13
            Case ca13 = new Case();
            ca13.CreateEvent("A");
            ca13.CreateEvent("D");
            ca13.CreateEvent("C");
            ca13.CreateEvent("E");
            ca13.CreateEvent("F");
            ca13.CreateEvent("C");
            ca13.CreateEvent("D");
            ca13.CreateEvent("E");
            ca13.CreateEvent("H");

            //create Case14
            Case ca14 = new Case();
            ca14.CreateEvent("A");
            ca14.CreateEvent("D");
            ca14.CreateEvent("C");
            ca14.CreateEvent("E");
            ca14.CreateEvent("F");
            ca14.CreateEvent("D");
            ca14.CreateEvent("B");
            ca14.CreateEvent("E");
            ca14.CreateEvent("H");

            //create Case15
            Case ca15 = new Case();
            ca15.CreateEvent("A");
            ca15.CreateEvent("D");
            ca15.CreateEvent("C");
            ca15.CreateEvent("E");
            ca15.CreateEvent("F");
            ca15.CreateEvent("B");
            ca15.CreateEvent("D");
            ca15.CreateEvent("E");
            ca15.CreateEvent("G");

            //create Case16
            Case ca16 = new Case();
            ca16.CreateEvent("A");
            ca16.CreateEvent("C");
            ca16.CreateEvent("D");
            ca16.CreateEvent("E");
            ca16.CreateEvent("F");
            ca16.CreateEvent("B");
            ca16.CreateEvent("D");
            ca16.CreateEvent("E");
            ca16.CreateEvent("F");
            ca16.CreateEvent("D");
            ca16.CreateEvent("B");
            ca16.CreateEvent("E");
            ca16.CreateEvent("G");

            //create Case17
            Case ca17 = new Case();
            ca17.CreateEvent("A");
            ca17.CreateEvent("D");
            ca17.CreateEvent("C");
            ca17.CreateEvent("E");
            ca17.CreateEvent("F");
            ca17.CreateEvent("D");
            ca17.CreateEvent("B");
            ca17.CreateEvent("E");
            ca17.CreateEvent("G");

            //create Case18
            Case ca18 = new Case();
            ca18.CreateEvent("A");
            ca18.CreateEvent("D");
            ca18.CreateEvent("C");
            ca18.CreateEvent("E");
            ca18.CreateEvent("F");
            ca18.CreateEvent("B");
            ca18.CreateEvent("D");
            ca18.CreateEvent("E");
            ca18.CreateEvent("F");
            ca18.CreateEvent("B");
            ca18.CreateEvent("D");
            ca18.CreateEvent("E");
            ca18.CreateEvent("G");

            //create Case19
            Case ca19 = new Case();
            ca19.CreateEvent("A");
            ca19.CreateEvent("D");
            ca19.CreateEvent("C");
            ca19.CreateEvent("E");
            ca19.CreateEvent("F");
            ca19.CreateEvent("D");
            ca19.CreateEvent("B");
            ca19.CreateEvent("E");
            ca19.CreateEvent("F");
            ca19.CreateEvent("B");
            ca19.CreateEvent("D");
            ca19.CreateEvent("E");
            ca19.CreateEvent("H");

            //create Case20
            Case ca20 = new Case();
            ca20.CreateEvent("A");
            ca20.CreateEvent("D");
            ca20.CreateEvent("B");
            ca20.CreateEvent("E");
            ca20.CreateEvent("F");
            ca20.CreateEvent("B");
            ca20.CreateEvent("D");
            ca20.CreateEvent("E");
            ca20.CreateEvent("F");
            ca20.CreateEvent("D");
            ca20.CreateEvent("B");
            ca20.CreateEvent("E");
            ca20.CreateEvent("G");

            //create Case21
            Case ca21 = new Case();
            ca21.CreateEvent("A");
            ca21.CreateEvent("D");
            ca21.CreateEvent("C");
            ca21.CreateEvent("E");
            ca21.CreateEvent("F");
            ca21.CreateEvent("D");
            ca21.CreateEvent("B");
            ca21.CreateEvent("E");
            ca21.CreateEvent("F");
            ca21.CreateEvent("C");
            ca21.CreateEvent("D");
            ca21.CreateEvent("E");
            ca21.CreateEvent("F");
            ca21.CreateEvent("D");
            ca21.CreateEvent("B");
            ca21.CreateEvent("E");
            ca21.CreateEvent("G");

            //create Event Log
            EventLog eventLog = new EventLog();
            eventLog.Cases.Add(ca1);
            eventLog.Cases.Add(ca2);
            eventLog.Cases.Add(ca3);
            eventLog.Cases.Add(ca4);
            eventLog.Cases.Add(ca5);
            eventLog.Cases.Add(ca6);
            eventLog.Cases.Add(ca7);
            eventLog.Cases.Add(ca8);
            eventLog.Cases.Add(ca9);
            eventLog.Cases.Add(ca10);
            eventLog.Cases.Add(ca11);
            eventLog.Cases.Add(ca12);
            eventLog.Cases.Add(ca13);
            eventLog.Cases.Add(ca14);
            eventLog.Cases.Add(ca15);
            eventLog.Cases.Add(ca16);
            eventLog.Cases.Add(ca17);
            eventLog.Cases.Add(ca18);
            eventLog.Cases.Add(ca19);
            eventLog.Cases.Add(ca20);
            eventLog.Cases.Add(ca21);

            return eventLog;
        }

        /// <summary>
        /// This method creates an event log with three cases.
        /// </summary>
        /// <autor>Markus Holznagel</autor>
        public static EventLog ThreeCaseEventLog()
        {
            // Create Case 1
            Case ca1 = new Case();
            ca1.CreateEvent("A");
            ca1.CreateEvent("B");
            ca1.CreateEvent("D");
            ca1.CreateEvent("E");

            // Create Case 2
            Case ca2 = new Case();
            ca2.CreateEvent("A");
            ca2.CreateEvent("D");
            ca2.CreateEvent("B");
            ca2.CreateEvent("E");

            // Create Case 3
            Case ca3 = new Case();
            ca3.CreateEvent("A");
            ca3.CreateEvent("C");
            ca3.CreateEvent("E");

            //Create Event Log
            EventLog eventLog = new EventLog();
            eventLog.Cases.Add(ca1);
            eventLog.Cases.Add(ca2);
            eventLog.Cases.Add(ca3);

            return eventLog;
        }

        /// <summary>
        /// This method creates an event log for recursive parallelisms.
        /// </summary>
        /// <autor>Markus Holznagel</autor>
        public static EventLog EventLogForRecursiveParallelisms()
        {
            // Create Case 1
            Case ca1 = new Case();
            ca1.CreateEvent("A");
            ca1.CreateEvent("B");
            ca1.CreateEvent("C");
            ca1.CreateEvent("D");
            ca1.CreateEvent("F");
            ca1.CreateEvent("H");
            ca1.CreateEvent("J");
            ca1.CreateEvent("L");
            ca1.CreateEvent("M");
            ca1.CreateEvent("N");

            // Create Case 2
            Case ca2 = new Case();
            ca2.CreateEvent("A");
            ca2.CreateEvent("B");
            ca2.CreateEvent("C");
            ca2.CreateEvent("D");
            ca2.CreateEvent("F");
            ca2.CreateEvent("H");
            ca2.CreateEvent("K");
            ca2.CreateEvent("L");
            ca2.CreateEvent("M");
            ca2.CreateEvent("N");

            // Create Case 3
            Case ca3 = new Case();
            ca3.CreateEvent("A");
            ca3.CreateEvent("B");
            ca3.CreateEvent("C");
            ca3.CreateEvent("D");
            ca3.CreateEvent("F");
            ca3.CreateEvent("I");
            ca3.CreateEvent("J");
            ca3.CreateEvent("L");
            ca3.CreateEvent("M");
            ca3.CreateEvent("N");

            // Create Case 4
            Case ca4 = new Case();
            ca4.CreateEvent("A");
            ca4.CreateEvent("B");
            ca4.CreateEvent("C");
            ca4.CreateEvent("D");
            ca4.CreateEvent("F");
            ca4.CreateEvent("I");
            ca4.CreateEvent("K");
            ca4.CreateEvent("L");
            ca4.CreateEvent("M");
            ca4.CreateEvent("N");

            // Create Case 5
            Case ca5 = new Case();
            ca5.CreateEvent("A");
            ca5.CreateEvent("B");
            ca5.CreateEvent("C");
            ca5.CreateEvent("D");
            ca5.CreateEvent("G");
            ca5.CreateEvent("H");
            ca5.CreateEvent("J");
            ca5.CreateEvent("L");
            ca5.CreateEvent("M");
            ca5.CreateEvent("N");

            // Create Case 6
            Case ca6 = new Case();
            ca6.CreateEvent("A");
            ca6.CreateEvent("B");
            ca6.CreateEvent("C");
            ca6.CreateEvent("D");
            ca6.CreateEvent("G");
            ca6.CreateEvent("H");
            ca6.CreateEvent("K");
            ca6.CreateEvent("L");
            ca6.CreateEvent("M");
            ca6.CreateEvent("N");

            // Create Case 7
            Case ca7 = new Case();
            ca7.CreateEvent("A");
            ca7.CreateEvent("B");
            ca7.CreateEvent("C");
            ca7.CreateEvent("D");
            ca7.CreateEvent("G");
            ca7.CreateEvent("I");
            ca7.CreateEvent("J");
            ca7.CreateEvent("L");
            ca7.CreateEvent("M");
            ca7.CreateEvent("N");

            // Create Case 8
            Case ca8 = new Case();
            ca8.CreateEvent("A");
            ca8.CreateEvent("B");
            ca8.CreateEvent("C");
            ca8.CreateEvent("D");
            ca8.CreateEvent("G");
            ca8.CreateEvent("I");
            ca8.CreateEvent("K");
            ca8.CreateEvent("L");
            ca8.CreateEvent("M");
            ca8.CreateEvent("N");

            // Create Case 9
            Case ca9 = new Case();
            ca9.CreateEvent("A");
            ca9.CreateEvent("B");
            ca9.CreateEvent("C");
            ca9.CreateEvent("E");
            ca9.CreateEvent("F");
            ca9.CreateEvent("H");
            ca9.CreateEvent("J");
            ca9.CreateEvent("L");
            ca9.CreateEvent("M");
            ca9.CreateEvent("N");

            // Create Case 10
            Case ca10 = new Case();
            ca10.CreateEvent("A");
            ca10.CreateEvent("B");
            ca10.CreateEvent("C");
            ca10.CreateEvent("E");
            ca10.CreateEvent("F");
            ca10.CreateEvent("H");
            ca10.CreateEvent("K");
            ca10.CreateEvent("L");
            ca10.CreateEvent("M");
            ca10.CreateEvent("N");

            // Create Case 11
            Case ca11 = new Case();
            ca11.CreateEvent("A");
            ca11.CreateEvent("B");
            ca11.CreateEvent("C");
            ca11.CreateEvent("E");
            ca11.CreateEvent("F");
            ca11.CreateEvent("I");
            ca11.CreateEvent("J");
            ca11.CreateEvent("L");
            ca11.CreateEvent("M");
            ca11.CreateEvent("N");

            // Create Case 12
            Case ca12 = new Case();
            ca12.CreateEvent("A");
            ca12.CreateEvent("B");
            ca12.CreateEvent("C");
            ca12.CreateEvent("E");
            ca12.CreateEvent("F");
            ca12.CreateEvent("I");
            ca12.CreateEvent("K");
            ca12.CreateEvent("L");
            ca12.CreateEvent("M");
            ca12.CreateEvent("N");

            // Create Case 13
            Case ca13 = new Case();
            ca13.CreateEvent("A");
            ca13.CreateEvent("B");
            ca13.CreateEvent("C");
            ca13.CreateEvent("E");
            ca13.CreateEvent("G");
            ca13.CreateEvent("H");
            ca13.CreateEvent("J");
            ca13.CreateEvent("L");
            ca13.CreateEvent("M");
            ca13.CreateEvent("N");

            // Create Case 14
            Case ca14 = new Case();
            ca14.CreateEvent("A");
            ca14.CreateEvent("B");
            ca14.CreateEvent("C");
            ca14.CreateEvent("E");
            ca14.CreateEvent("G");
            ca14.CreateEvent("H");
            ca14.CreateEvent("K");
            ca14.CreateEvent("L");
            ca14.CreateEvent("M");
            ca14.CreateEvent("N");

            // Create Case 15
            Case ca15 = new Case();
            ca15.CreateEvent("A");
            ca15.CreateEvent("B");
            ca15.CreateEvent("C");
            ca15.CreateEvent("E");
            ca15.CreateEvent("G");
            ca15.CreateEvent("I");
            ca15.CreateEvent("J");
            ca15.CreateEvent("L");
            ca15.CreateEvent("M");
            ca15.CreateEvent("N");

            // Create Case 16
            Case ca16 = new Case();
            ca16.CreateEvent("A");
            ca16.CreateEvent("B");
            ca16.CreateEvent("C");
            ca16.CreateEvent("E");
            ca16.CreateEvent("G");
            ca16.CreateEvent("I");
            ca16.CreateEvent("K");
            ca16.CreateEvent("L");
            ca16.CreateEvent("M");
            ca16.CreateEvent("N");

            // Create event log
            EventLog eventLog = new EventLog();
            eventLog.Cases.Add(ca1);
            eventLog.Cases.Add(ca2);
            eventLog.Cases.Add(ca3);
            eventLog.Cases.Add(ca4);
            eventLog.Cases.Add(ca5);
            eventLog.Cases.Add(ca6);
            eventLog.Cases.Add(ca7);
            eventLog.Cases.Add(ca8);
            eventLog.Cases.Add(ca9);
            eventLog.Cases.Add(ca10);
            eventLog.Cases.Add(ca11);
            eventLog.Cases.Add(ca12);
            eventLog.Cases.Add(ca13);
            eventLog.Cases.Add(ca14);
            eventLog.Cases.Add(ca15);
            eventLog.Cases.Add(ca16);

            return eventLog;
        }

        /// <summary>
        /// This method creates an event log with one case.
        /// </summary>
        /// <autor>Thomas Meents</autor>
        public static EventLog OneCaseEventLog()
        {
            Event firstEvent = new Event("first");
            Event secondEvent = new Event("second");
            List<Event> listOfEvents = new List<Event> { firstEvent, secondEvent };
            Case ca = new Case("ExampleCase", listOfEvents);
            EventLog eventLog = new EventLog();
            eventLog.Cases.Add(ca);

            return eventLog;
        }

        /// <summary>
        /// Has a corresponding PetriNetExample: (1) -> [One] -> (2) -> [Two] -> (3)-> [Three] -> (4)-> [Four] -> (5)-> [Five] -> (6)
        /// </summary>
        /// <author>Jannik Arndt</author>
        public static EventLog OneTwoThreeFourFive()
        {
            var eventList = new List<Event>
            {
                new Event("One"),
                new Event("Two"),
                new Event("Three"),
                new Event("Four"),
                new Event("Five")
            };

            var caseList = new List<Case>
            {
                new Case("Case1", eventList),
                new Case("Case2", eventList),
                new Case("Case3", eventList),
                new Case("Case4", eventList),
                new Case("Case5", eventList)
            };

            var eventLog = new EventLog {Cases = caseList};

            return eventLog;
        }

        /// <summary>
        /// Has a corresponding PetriNetExample: (1) -> [One] -> (2) -> [Two] -> (3)-> [Three] -> (4)-> [Four] -> (5)-> [Five] -> (6)
        /// </summary>
        /// <author>Jannik Arndt</author>
        public static EventLog OneTwoThreeFourFiveWithErrors()
        {
            var eventList1 = new List<Event>
            {
                new Event("One"),
                new Event("Two"),
                new Event("Three"),
                new Event("Four"),
                new Event("Five")
            };

            var eventList2 = new List<Event>
            {
                new Event("One"),
                new Event("Two"),
                new Event("Forty-Two"),
                new Event("Four"),
                new Event("Five")
            };

            var eventList3 = new List<Event>
            {
                new Event("One"),
                new Event("Five"),
                new Event("Three"),
                new Event("Four"),
                new Event("Two")
            };

            var caseList = new List<Case>
            {
                new Case("Case1", eventList1),
                new Case("Case2", eventList2),
                new Case("Case3", eventList3)
            };

            var eventLog = new EventLog { Cases = caseList };

            return eventLog;
        }
    }
}
