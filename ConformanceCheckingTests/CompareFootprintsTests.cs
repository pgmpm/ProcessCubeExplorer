using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.ConformanceChecking;
using pgmpm.MatrixSelection.Fields;
using pgmpm.Model.PetriNet;
using System;
using EventLog = pgmpm.MatrixSelection.Fields.EventLog;

namespace pgmpm.ConformanceCheckingTests
{
    /// <summary>
    /// Tests for the Comparing Footprint Algorithm
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    [TestClass]
    public class CompareFootprintsTests
    {
        /// <summary>
        /// Compare footprints from an eventLog and a petri net.
        /// (Compensation process of an airline)
        /// </summary>
        /// <autor>Andrej Albrecht; Naby Moussa Sow</autor>
        [TestMethod]
        public void CompareEventLogAndPetriNetTest1()
        {
            //
            //EventLog
            //
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


            //
            //PetriNet
            //
            PetriNet petriNet = new PetriNet("Petri-Net Name");

            Place pStart = new Place("Place Start", 0);
            Place pC1 = new Place("c1", 0);
            Place pC2 = new Place("c2", 0);
            Place pC3 = new Place("c3", 0);
            Place pC4 = new Place("c4", 0);
            Place pC5 = new Place("c5", 0);
            Place pEnd = new Place("Place End", 0);

            Transition tA = new Transition("A");
            Transition tB = new Transition("B");
            Transition tC = new Transition("C");
            Transition tD = new Transition("D");
            Transition tE = new Transition("E");
            Transition tF = new Transition("F");
            Transition tG = new Transition("G");
            Transition tH = new Transition("H");

            tG.AddOutgoingPlace(pEnd);
            tG.AddIncomingPlace(pC5);

            tH.AddOutgoingPlace(pEnd);
            tH.AddIncomingPlace(pC5);

            tE.AddIncomingPlace(pC3);
            tE.AddIncomingPlace(pC4);
            tE.AddOutgoingPlace(pC5);

            pC3.AppendIncomingTransition(tB);
            pC3.AppendIncomingTransition(tC);
            pC3.AppendOutgoingTransition(tE);

            pC4.AppendIncomingTransition(tD);
            pC4.AppendOutgoingTransition(tE);

            tB.AddIncomingPlace(pC1);
            tB.AddOutgoingPlace(pC3);

            tC.AddIncomingPlace(pC1);
            tC.AddOutgoingPlace(pC3);

            tD.AddIncomingPlace(pC2);
            tD.AddOutgoingPlace(pC4);

            pC1.AppendIncomingTransition(tA);
            pC1.AppendOutgoingTransition(tB);
            pC1.AppendOutgoingTransition(tC);

            pC2.AppendIncomingTransition(tA);
            pC2.AppendOutgoingTransition(tD);

            tF.AddIncomingPlace(pC5);
            tF.AddOutgoingPlace(pC1);
            tF.AddOutgoingPlace(pC2);

            //
            tA.AddIncomingPlace(pStart);
            tA.AddOutgoingPlace(pC1);
            tA.AddOutgoingPlace(pC2);

            pStart.AppendOutgoingTransition(tA);

            pEnd.AppendIncomingTransition(tG);
            pEnd.AppendIncomingTransition(tH);

            ////
            petriNet.Transitions.Add(tA);
            petriNet.Transitions.Add(tB);
            petriNet.Transitions.Add(tC);
            petriNet.Transitions.Add(tD);
            petriNet.Transitions.Add(tE);
            petriNet.Transitions.Add(tF);
            petriNet.Transitions.Add(tG);
            petriNet.Transitions.Add(tH);

            ////
            petriNet.Places.Add(pStart);
            petriNet.Places.Add(pC1);
            petriNet.Places.Add(pC2);
            petriNet.Places.Add(pC3);
            petriNet.Places.Add(pC4);
            petriNet.Places.Add(pC5);
            petriNet.Places.Add(pEnd);

            ComparingFootprint footprintEventLog = ComparingFootprintAlgorithm.CreateFootprint(eventLog);
            ComparingFootprint footprintPetriNet = ComparingFootprintAlgorithm.CreateFootprint(petriNet);

            ComparingFootprintResultMatrix footprintResultMatrix = new ComparingFootprintResultMatrix(footprintEventLog, footprintPetriNet);

            if (footprintResultMatrix.HeaderWithEventNames.Count != 8)
            {
                Assert.Fail("");
            }

            int y = 0;
            foreach (String headerNameY in footprintResultMatrix.HeaderWithEventNames)
            {
                int x = 0;
                foreach (String headerNameX in footprintResultMatrix.HeaderWithEventNames)
                {
                    ResultCellType resultCellType = footprintResultMatrix.ResultMatrix[y, x];
                    Console.WriteLine("Test headerNameY: " + headerNameY + ", headerNameX: " + headerNameX + " [" + y + "," + x + "].CellType:" + resultCellType);

                    
                    if (!resultCellType.Equals(ResultCellType.NoDifferences))
                    {
                        Assert.Fail("Test headerNameY: " + headerNameY + ", headerNameX: " + headerNameX + " [" + y + "," + x + "].CellType:" + resultCellType);
                    }
                    
                    

                    x++;
                }
                y++;
            }

            //Arrange (Create Footprints)
            ComparingFootprint footPrintEventlog = ComparingFootprintAlgorithm.CreateFootprint(eventLog);
            ComparingFootprint footPrintPetrinet = ComparingFootprintAlgorithm.CreateFootprint(petriNet);
            ComparingFootprintResultMatrix resultFootprint = new ComparingFootprintResultMatrix(footPrintEventlog, footPrintPetrinet);

            // Act (Calculate Fitness)
            double fitnessComparingFootprint = ComparingFootprintAlgorithm.CalculateFitness(resultFootprint.GetNumberOfDifferences(), resultFootprint.GetNumberOfOpportunities());


            if (fitnessComparingFootprint != 1.0)
            {
                // Assert
                Assert.Fail("Fitness not correct! (" + fitnessComparingFootprint + ")");
            }
        }


    }


}
