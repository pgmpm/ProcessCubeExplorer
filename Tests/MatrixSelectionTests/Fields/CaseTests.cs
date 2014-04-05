using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.MatrixSelection.Fields;
using System.Collections.Generic;

namespace pgmpm.MatrixSelectionTests
{
    [TestClass]
    public class CaseTests
    {
        /// <summary>
        /// Test for adding events to a case
        /// and test if there exists
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void CaseTest1()
        {
            //create Case
            Case ca = new Case();
            ca.CreateEvent("A");
            ca.CreateEvent("B");
            ca.CreateEvent("C");
            ca.CreateEvent("D");

            foreach(Event e in ca.EventList)
            {
                if (e.Name.Equals("A"))
                {
                    
                }
                else if (e.Name.Equals("B"))
                {

                }
                else if (e.Name.Equals("C"))
                {

                }
                else if (e.Name.Equals("D"))
                {

                }
                else
                {
                    Assert.Fail("Event not in case");
                }
            }

            ca.EventList.Remove(new Event("C"));

            foreach (Event e in ca.EventList)
            {
                if (e.Name.Equals("C"))
                {
                    Assert.Fail("removed event is in case");
                }

                if (e.Name.Equals("A"))
                {

                }
                else if (e.Name.Equals("B"))
                {

                }
                else if (e.Name.Equals("D"))
                {

                }
                else
                {
                    Assert.Fail("Event not in case");
                }
            }
        }

        /// <summary>
        /// Test for giving a list of events to a case object
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void CaseListContructorTest1()
        {
            List<Event> eventList = new List<Event> {new Event("A"), new Event("B"), new Event("C"), new Event("D")};

            //create Case
            Case ca = new Case {EventList = eventList};

            foreach (Event e in ca.EventList)
            {
                if (e.Name.Equals("A"))
                {

                }
                else if (e.Name.Equals("B"))
                {

                }
                else if (e.Name.Equals("C"))
                {

                }
                else if (e.Name.Equals("D"))
                {

                }
                else
                {
                    Assert.Fail("Event not in case");
                }
            }
        }

        /// <summary>
        /// Test for giving a list of events to the constructor
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void CaseListContructorTest2()
        {
            List<Event> eventList = new List<Event> {new Event("A"), new Event("B"), new Event("C"), new Event("D")};

            //create Case
            Case ca = new Case("CaseName", eventList);

            foreach (Event e in ca.EventList)
            {
                if (e.Name.Equals("A"))
                {

                }
                else if (e.Name.Equals("B"))
                {

                }
                else if (e.Name.Equals("C"))
                {

                }
                else if (e.Name.Equals("D"))
                {

                }
                else
                {
                    Assert.Fail("Event not in case");
                }
            }
        }

        /// <summary>
        /// Test for adding events to a case with 4 variations
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void CaseAddEventTest1()
        {
            List<Event> eventList = new List<Event> {new Event("A")};

            //create Case and add event a
            Case ca = new Case("CaseName", eventList);

            //add event b
            ca.CreateEvent("B");

            //add event c
            ca.EventList.Add(new Event("C"));

            //add event d
            Event eventD = new Event {Name = "D"};
            ca.EventList.Add(eventD);
            
            foreach (Event e in ca.EventList)
            {
                if (e.Name.Equals("A"))
                {

                }
                else if (e.Name.Equals("B"))
                {

                }
                else if (e.Name.Equals("C"))
                {

                }
                else if (e.Name.Equals("D"))
                {

                }
                else
                {
                    Assert.Fail("Event not in case");
                }
            }
        }

        /// <author>Thomas Meents</author>
        [TestMethod()]
        public void CreateEventTest()
        {
            Case ca = new Case();
            ca.CreateEvent("TestEvent");
            Assert.IsTrue(ca.EventList.Where(ev => ev.Name == "TestEvent").Select(ev => ev.Name).Count()==1);
        }

        /// <author>Thomas Meents</author>
        [TestMethod()]
        public void EventFollowsEventCountTest()
        {
            Case ca = new Case();
            Event ev1 = new Event {Name = "first"};
            Event ev2 = new Event {Name = "second"};
            ca.EventList.Add(ev1);
            ca.EventList.Add(ev2);
            Assert.IsTrue(ca.EventFollowsEventCount(ev1, ev2)==1,"Count: " + ca.EventFollowsEventCount(ev1,ev2));
        }

        /// <author>Thomas Meents</author>
        [TestMethod()]
        public void IndexesOfPairTest()
        {
            Case ca = new Case();
            Event ev1 = new Event { Name = "first" };
            Event ev2 = new Event { Name = "second" };
            ca.EventList.Add(ev1);
            ca.EventList.Add(ev2);
            List<int> list = ca.IndexesOfPair(ev1, ev2);
            Assert.IsTrue(list.Count==1);
        }
    }
}
