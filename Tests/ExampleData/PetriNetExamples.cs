using pgmpm.Model.PetriNet;

namespace pgmpm.ExampleData
{
    /// <summary>
    /// Petri net examples for program tests.
    /// </summary>
    /// <author>Markus Holznagel</author>
    public class PetriNetExample
    {
        /// <summary>
        /// This method creates a petri net with five places and four transitions.
        /// </summary>
        /// <autor>Markus Holznagel</autor>
        public static PetriNet PetriNetWithFivePlacesAndFourTransitions()
        {
            PetriNet petriNet = new PetriNet("Petri-Net Name");

            Place pAIn = new Place("PlaceAIn", 0);
            Place pAOut = new Place("PlaceAOut", 0);
            Place pBOut = new Place("PlaceBOut", 0);
            Place pCOut = new Place("PlaceCOut", 0);
            Place pDOut = new Place("PlaceDOut", 0);

            Transition tA = new Transition("TransitionA");
            Transition tB = new Transition("TransitionB");
            Transition tC = new Transition("TransitionC");
            Transition tD = new Transition("TransitionD");

            pAIn.AppendOutgoingTransition(tA);
            pAOut.AppendOutgoingTransition(tB);
            pBOut.AppendOutgoingTransition(tC);
            pCOut.AppendOutgoingTransition(tD);

            tA.AddOutgoingPlace(pAOut);
            tB.AddOutgoingPlace(pBOut);
            tC.AddOutgoingPlace(pCOut);
            tD.AddOutgoingPlace(pDOut);

            // Create petri net
            petriNet.Places.Add(pAIn);
            petriNet.Places.Add(pAOut);
            petriNet.Places.Add(pBOut);
            petriNet.Places.Add(pCOut);
            petriNet.Places.Add(pDOut);

            petriNet.Transitions.Add(tA);
            petriNet.Transitions.Add(tB);
            petriNet.Transitions.Add(tC);
            petriNet.Transitions.Add(tD);

            return petriNet;
        }

        /// <summary>
        /// This method creates a complex petri net with five places and eight transitions.
        /// </summary>
        /// <autor>Andrej Albrecht, Markus Holznagel</autor>
        public static PetriNet PetriNetWithFivePlacesAndEightTransitions()
        {
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

            tA.AddIncomingPlace(pStart);
            tA.AddOutgoingPlace(pC1);
            tA.AddOutgoingPlace(pC2);

            pStart.AppendOutgoingTransition(tA);

            pEnd.AppendIncomingTransition(tG);
            pEnd.AppendIncomingTransition(tH);

            petriNet.Transitions.Add(tA);
            petriNet.Transitions.Add(tB);
            petriNet.Transitions.Add(tC);
            petriNet.Transitions.Add(tD);
            petriNet.Transitions.Add(tE);
            petriNet.Transitions.Add(tF);
            petriNet.Transitions.Add(tG);
            petriNet.Transitions.Add(tH);

            petriNet.Places.Add(pStart);
            petriNet.Places.Add(pC1);
            petriNet.Places.Add(pC2);
            petriNet.Places.Add(pC3);
            petriNet.Places.Add(pC4);
            petriNet.Places.Add(pC5);
            petriNet.Places.Add(pEnd);

            return petriNet;
        }

        /// <summary>
        /// This method creates a complex petri net with six places and five transitions.
        /// </summary>
        /// <autor>Andrej Albrecht, Markus Holznagel</autor>
        public static PetriNet PetriNetWithSixPlacesAndFiveTransitions()
        {
            PetriNet petriNet = new PetriNet("Petri-Net Name");

            Place pStart = new Place("Place Start", 0);
            Place p1 = new Place("c1", 0);
            Place p2 = new Place("c2", 0);
            Place p3 = new Place("c3", 0);
            Place p4 = new Place("c4", 0);
            Place pEnd = new Place("Place End", 0);

            Transition tA = new Transition("A");
            Transition tB = new Transition("B");
            Transition tC = new Transition("C");
            Transition tD = new Transition("D");
            Transition tE = new Transition("E");

            pStart.AppendOutgoingTransition(tA);

            tA.AddIncomingPlace(pStart);
            tA.AddOutgoingPlace(p1);
            tA.AddOutgoingPlace(p2);

            p1.AppendIncomingTransition(tA);
            p1.AppendOutgoingTransition(tB);
            p1.AppendOutgoingTransition(tC);

            p2.AppendIncomingTransition(tA);
            p2.AppendOutgoingTransition(tC);
            p2.AppendOutgoingTransition(tD);

            tB.AddIncomingPlace(p1);
            tB.AddOutgoingPlace(p3);

            tC.AddIncomingPlace(p1);
            tC.AddIncomingPlace(p2);
            tC.AddOutgoingPlace(p3);
            tC.AddOutgoingPlace(p4);

            tD.AddIncomingPlace(p2);
            tD.AddOutgoingPlace(p4);

            p3.AppendIncomingTransition(tB);
            p3.AppendIncomingTransition(tC);
            p3.AppendOutgoingTransition(tE);

            p4.AppendIncomingTransition(tC);
            p4.AppendIncomingTransition(tD);
            p4.AppendOutgoingTransition(tE);

            tE.AddIncomingPlace(p3);
            tE.AddIncomingPlace(p4);
            tE.AddOutgoingPlace(pEnd);

            pEnd.AppendIncomingTransition(tE);

            petriNet.Transitions.Add(tA);
            petriNet.Transitions.Add(tB);
            petriNet.Transitions.Add(tC);
            petriNet.Transitions.Add(tD);
            petriNet.Transitions.Add(tE);

            petriNet.Places.Add(pStart);
            petriNet.Places.Add(p1);
            petriNet.Places.Add(p2);
            petriNet.Places.Add(p3);
            petriNet.Places.Add(p4);
            petriNet.Places.Add(pEnd);

            return petriNet;
        }

        /// <summary>
        /// This method creates a complex petri net with fourteen places and fourteen transitions.
        /// You can choose this for testing recursive parallelisms.
        /// </summary>
        /// <autor>Markus Holznagel</autor>
        public static PetriNet PetriNetWithFourteenPlacesAndFourteenTransitions()
        {
            PetriNet petriNet = new PetriNet("Petri-Net Name");

            Place pStart = new Place("pStart");
            Place p1 = new Place("p1");
            Place p2 = new Place("p2");
            Place p3 = new Place("p3");
            Place p4 = new Place("p4");
            Place p5 = new Place("p5");
            Place p6 = new Place("p6");
            Place p7 = new Place("p7");
            Place p8 = new Place("p8");
            Place p9 = new Place("p9");
            Place p10 = new Place("p10");
            Place p11 = new Place("p11");
            Place p12 = new Place("p12");
            Place pEnd = new Place("pEnd");

            Transition tA = new Transition("A");
            Transition tB = new Transition("B");
            Transition tC = new Transition("C");
            Transition tD = new Transition("D");
            Transition tE = new Transition("E");
            Transition tF = new Transition("F");
            Transition tG = new Transition("G");
            Transition tH = new Transition("H");
            Transition tI = new Transition("I");
            Transition tJ = new Transition("J");
            Transition tK = new Transition("K");
            Transition tL = new Transition("L");
            Transition tM = new Transition("M");
            Transition tN = new Transition("N");

            // Transitions and Places
            pStart.AppendOutgoingTransition(tA);

            tA.AddIncomingPlace(pStart);
            tA.AddOutgoingPlace(p1);
            tA.AddOutgoingPlace(p2);

            p1.AppendIncomingTransition(tA);
            p1.AppendOutgoingTransition(tB);

            tB.AddIncomingPlace(p1);
            tB.AddOutgoingPlace(p3);
            tB.AddOutgoingPlace(p4);

            p2.AppendIncomingTransition(tA);
            p2.AppendOutgoingTransition(tC);

            tC.AddIncomingPlace(p2);
            tC.AddIncomingPlace(p5);
            tC.AddIncomingPlace(p6);

            p3.AppendIncomingTransition(tB);
            p3.AppendOutgoingTransition(tD);
            p3.AppendOutgoingTransition(tE);

            tD.AddIncomingPlace(p3);
            tD.AddOutgoingPlace(p7);

            tE.AddIncomingPlace(p3);
            tE.AddOutgoingPlace(p7);

            p4.AppendIncomingTransition(tB);
            p4.AppendOutgoingTransition(tF);
            p4.AppendOutgoingTransition(tG);

            tF.AddIncomingPlace(p4);
            tF.AddOutgoingPlace(p8);

            tG.AddIncomingPlace(p4);
            tG.AddOutgoingPlace(p8);

            p5.AppendIncomingTransition(tC);
            p5.AppendOutgoingTransition(tH);
            p5.AppendOutgoingTransition(tI);

            tH.AddIncomingPlace(p5);
            tH.AddOutgoingPlace(p9);

            tI.AddIncomingPlace(p5);
            tI.AddOutgoingPlace(p9);

            p6.AppendIncomingTransition(tC);
            p6.AppendOutgoingTransition(tJ);
            p6.AppendOutgoingTransition(tK);

            tJ.AddIncomingPlace(p6);
            tJ.AddOutgoingPlace(p10);

            tK.AddIncomingPlace(p6);
            tK.AddOutgoingPlace(p10);

            p7.AppendIncomingTransition(tD);
            p7.AppendIncomingTransition(tE);
            p7.AppendOutgoingTransition(tL);

            p8.AppendIncomingTransition(tF);
            p8.AppendIncomingTransition(tG);
            p8.AppendOutgoingTransition(tL);

            tL.AddIncomingPlace(p7);
            tL.AddIncomingPlace(p8);
            tL.AddOutgoingPlace(p11);

            p9.AppendIncomingTransition(tH);
            p9.AppendIncomingTransition(tI);
            p9.AppendOutgoingTransition(tM);

            p10.AppendIncomingTransition(tJ);
            p10.AppendIncomingTransition(tK);
            p10.AppendOutgoingTransition(tM);

            tM.AddIncomingPlace(p9);
            tM.AddIncomingPlace(p10);
            tM.AddOutgoingPlace(p12);

            p11.AppendIncomingTransition(tL);
            p11.AppendOutgoingTransition(tN);

            p12.AppendIncomingTransition(tM);
            p12.AppendOutgoingTransition(tN);

            tN.AddIncomingPlace(p11);
            tN.AddIncomingPlace(p12);
            tN.AddOutgoingPlace(pEnd);

            pEnd.AppendIncomingTransition(tN);

            // Add Transitions and places to petrinet
            petriNet.Transitions.Add(tA);
            petriNet.Transitions.Add(tB);
            petriNet.Transitions.Add(tC);
            petriNet.Transitions.Add(tD);
            petriNet.Transitions.Add(tE);
            petriNet.Transitions.Add(tF);
            petriNet.Transitions.Add(tG);
            petriNet.Transitions.Add(tH);
            petriNet.Transitions.Add(tI);
            petriNet.Transitions.Add(tJ);
            petriNet.Transitions.Add(tK);
            petriNet.Transitions.Add(tL);
            petriNet.Transitions.Add(tM);
            petriNet.Transitions.Add(tN);

            petriNet.Places.Add(pStart);
            petriNet.Places.Add(p1);
            petriNet.Places.Add(p2);
            petriNet.Places.Add(p3);
            petriNet.Places.Add(p4);
            petriNet.Places.Add(p5);
            petriNet.Places.Add(p6);
            petriNet.Places.Add(p7);
            petriNet.Places.Add(p8);
            petriNet.Places.Add(p9);
            petriNet.Places.Add(p10);
            petriNet.Places.Add(p11);
            petriNet.Places.Add(p12);
            petriNet.Places.Add(pEnd);

            return petriNet;
        }

        /// <summary>
        /// This method creates a petri net with one loop, three places and four transitions.
        /// </summary>
        /// <autor>Thomas Meents</autor>
        public static PetriNet PetriNetWithOneLoopThreePlacesAndThreeTransitions()
        {
            PetriNet PetriNet = new PetriNet("Petri-Net Name");

            Place paIn = new Place("PlaceAIn", 0);
            Place paOut = new Place("PlaceAOut", 0);
            Place pbOut = new Place("PlaceBOut", 0);

            Transition ta = new Transition("TransitionA");
            Transition tb = new Transition("TransitionB");
            Transition tc = new Transition("");

            tb.IsLoop = true; //loop

            paIn.AppendOutgoingTransition(ta);
            paOut.AppendOutgoingTransition(tb);
            paOut.AppendIncomingTransition(tb); //loop
            pbOut.AppendOutgoingTransition(tc);
            pbOut.AppendOutgoingTransition(tb); //loop

            ta.AddOutgoingPlace(paOut);
            tb.AddOutgoingPlace(pbOut);
            tb.AddIncomingPlace(paIn); //loop

            PetriNet.Places.Add(paIn);
            PetriNet.Places.Add(paOut);
            PetriNet.Places.Add(pbOut);

            PetriNet.Transitions.Add(ta);
            PetriNet.Transitions.Add(tb);
            PetriNet.Transitions.Add(tc);

            return PetriNet;
        }

        /// <summary>
        /// (1) -> [One] -> (2) -> [Two] -> (3)-> [Three] -> (4)-> [Four] -> (5)-> [Five] -> (6)
        /// Has a corresponding EventLog!
        /// </summary>
        /// <author>Jannik Arndt</author>
        public static PetriNet OneTwoThreeFourFive()
        {
            var petriNet = new PetriNet("OneTwoThreeFourFive");

            var place1 = petriNet.AddPlace("1");
            var place2 = petriNet.AddPlace("2");
            var place3 = petriNet.AddPlace("3");
            var place4 = petriNet.AddPlace("4");
            var place5 = petriNet.AddPlace("5");
            var place6 = petriNet.AddPlace("6");

            petriNet.AddTransition("One", incomingPlace: place1, outgoingPlace: place2);
            petriNet.AddTransition("Two", incomingPlace: place2, outgoingPlace: place3);
            petriNet.AddTransition("Three", incomingPlace: place3, outgoingPlace: place4);
            petriNet.AddTransition("Four", incomingPlace: place4, outgoingPlace: place5);
            petriNet.AddTransition("Five", incomingPlace: place5, outgoingPlace: place6);

            return petriNet;
        }

        /// <summary>
        /// (1) -> [One] -> (3)-> [Three] -> (5)-> [Five] -> (6)
        /// Has a corresponding EventLog!
        /// </summary>
        /// <author>Thomas Meents</author>
        public static PetriNet OneThreeFive()
        {
            var petriNet = new PetriNet("OneThreeFive");

            var place1 = petriNet.AddPlace("1");
            var place3 = petriNet.AddPlace("3");
            var place5 = petriNet.AddPlace("5");
            var place6 = petriNet.AddPlace("6");

            petriNet.AddTransition("One", incomingPlace: place1, outgoingPlace: place3);
            petriNet.AddTransition("Three", incomingPlace: place3, outgoingPlace: place5);
            petriNet.AddTransition("Five", incomingPlace: place5, outgoingPlace: place6);

            return petriNet;
        }
    }
}
