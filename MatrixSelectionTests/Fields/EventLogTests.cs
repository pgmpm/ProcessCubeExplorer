using System.Linq;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.MatrixSelection.Fields;
using EventLog = pgmpm.MatrixSelection.Fields.EventLog;

namespace pgmpm.MatrixSelectionTests.Fields
{
    [TestClass()]
    public class EventLogTests
    {
        /// <author>Thomas Meents</author>
        [TestMethod()]
        public void CountEventsTest()
        {
            EventLog log = ExampleData.EventLogExample.OneCaseEventLogWithFourOrderedEvents();
            Assert.IsTrue(log.Cases.Count.Equals(log.CountEvents()));
        }

        /// <author>Thomas Meents</author>
        [TestMethod()]
        public void GetCaseByNameTest()
        {
            EventLog log = ExampleData.EventLogExample.OneCaseEventLog();
            Case examplecase = log.GetCaseByName("ExampleCase");
            Assert.IsTrue(examplecase.Name == "ExampleCase");
        }

        /// <author>Thomas Meents</author>
        [TestMethod()]
        public void EventFollowsEventTest()
        {
            EventLog log = ExampleData.EventLogExample.OneCaseEventLog();
            Event first = new Event();
            Event second = new Event();
            foreach (Case cases in log.Cases)
            {
                first = cases.EventList.First();
                second = cases.EventList.Last();
                break;
            }
            Assert.IsTrue(log.EventFollowsEvent(first, second) == 1,"Count: " + log.EventFollowsEvent(first, second));
        }

        /// <summary>
        /// A method to test the constructor of the class EventLog.
        /// </summary>
        [TestMethod]
        public void EventLogConstructorTest()
        {
            string eventLogName = "EventLog1";

            EventLog el = new EventLog(name: eventLogName);

            Assert.IsNotNull(el);
        }

        /// <summary>
        /// A method to test the add and get Case-Methods of the class EventLog.
        /// And with an overwrite test
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void EventLogAddAndGetCaseTest3()
        {
            EventLog el = new EventLog(name: "EventLogName");

            //create Case1
            Case ca = new Case("Case1");
            ca.CreateEvent("A");
            ca.CreateEvent("B");
            ca.CreateEvent("C");
            ca.CreateEvent("D");

            //create Case2
            Case ca2 = new Case("Case2");
            ca2.CreateEvent("AA");
            ca2.CreateEvent("CC");
            ca2.CreateEvent("BB");
            ca2.CreateEvent("DD");

            Case ca3 = new Case("Case3");
            ca3.CreateEvent("AAA");
            ca3.CreateEvent("CCC");
            ca3.CreateEvent("BBB");
            ca3.CreateEvent("DDD");

            el.Cases.Add(ca);
            el.Cases.Add(ca2);
            el.Cases.Add(ca3);


            Case caseGet2 = el.GetCaseByName("Case2");
            int countEvents = 0;
            foreach (Event e in caseGet2.EventList)
            {
                if (e.Name.Equals("AA")
                    || e.Name.Equals("BB")
                    || e.Name.Equals("CC")
                    || e.Name.Equals("DD"))
                {
                    countEvents++;
                }
                else
                {
                    Assert.Fail("Event not in cases and not in eventlog");
                }
            }

            if (countEvents != 4) Assert.Fail("not the right number of cases in the event log countEvents:" + countEvents);

            //
            // Overwrite test:
            //
            Case caNew3 = new Case("Case3");
            caNew3.CreateEvent("NewAAA");
            caNew3.CreateEvent("NewCCC");
            caNew3.CreateEvent("NewBBB");
            caNew3.CreateEvent("NewDDD");

            el.Cases.Add(caNew3);

            Case caseGet3 = el.GetCaseByName("Case3");

            countEvents = 0;
            foreach (Event e in caseGet3.EventList)
            {
                Debug.WriteLine("e.Name:" + e.Name);

                if (e.Name.Equals("AAA")
                    || e.Name.Equals("CCC")
                    || e.Name.Equals("BBB")
                    || e.Name.Equals("DDD"))
                {
                    countEvents++;
                }
                else if (e.Name.Equals("NewAAA")
                    || e.Name.Equals("NewCCC")
                    || e.Name.Equals("NewBBB")
                    || e.Name.Equals("NewDDD"))
                {
                    Assert.Fail("Overwrite test failed");
                }
            }

            if (countEvents != 4) Assert.Fail("not the right number of cases in the event log (countEvents:" + countEvents + ")");
        }
    }
}
