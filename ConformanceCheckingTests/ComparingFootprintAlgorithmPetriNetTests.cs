using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.ConformanceChecking;
using pgmpm.ExampleData;
using pgmpm.Model.PetriNet;

namespace pgmpm.ConformanceCheckingTests
{
    /// <summary>
    /// Tests for the Comparing Footprint Algorithm
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    [TestClass]
    public class ComparingFootprintAlgorithmPetriNetTests
    {
        /// <summary>
        /// Test for the methods who extract the headernames (Eventnames) from a petrinet
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void ComparingFootprintHeaderNamesFromPetriNetTest1()
        {
            ComparingFootprint footprintResult = ComparingFootprintAlgorithm.CreateFootprint(PetriNetExample.PetriNetWithFivePlacesAndFourTransitions());

            List<String> headerNames = footprintResult.HeaderWithEventNames;

            if (headerNames.Count != 4) Assert.Fail();

            int index = 0;
            foreach (String hName in headerNames)
            {
                if (!hName.Equals("TransitionA") && index == 0)
                {
                    Assert.Fail();
                }

                if (!hName.Equals("TransitionB") && index == 1)
                {
                    Assert.Fail();
                }

                if (!hName.Equals("TransitionC") && index == 2)
                {
                    Assert.Fail();
                }

                if (!hName.Equals("TransitionD") && index == 3)
                {
                    Assert.Fail();
                }
                index++;
            }

        }

        /// <summary>
        /// Test for the creation of a footprint from an simple eventlog
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void ComparingFootprintFromPetriNetTest1()
        {
            ComparingFootprint footprintResult = ComparingFootprintAlgorithm.CreateFootprint(PetriNetExample.PetriNetWithFivePlacesAndFourTransitions());
            CellType[,] resultMatrix = footprintResult.ResultMatrix;

            List<String> headerNames = footprintResult.HeaderWithEventNames;

            if (headerNames.Count != 4) Assert.Fail();

            int y = 0;
            foreach (String hNameY in headerNames)
            {
                int x = 0;
                foreach (String hNameX in headerNames)
                {
                    if (hNameY.Equals("TransitionA") && hNameX.Equals("TransitionA"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("TransitionA") && hNameX.Equals("TransitionB"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("TransitionA") && hNameX.Equals("TransitionC"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("TransitionA") && hNameX.Equals("TransitionD"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("TransitionB") && hNameX.Equals("TransitionA"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("TransitionB") && hNameX.Equals("TransitionB"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("TransitionB") && hNameX.Equals("TransitionC"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("TransitionB") && hNameX.Equals("TransitionD"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("TransitionC") && hNameX.Equals("TransitionA"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("TransitionC") && hNameX.Equals("TransitionB"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("TransitionC") && hNameX.Equals("TransitionC"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("TransitionC") && hNameX.Equals("TransitionD"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("TransitionD") && hNameX.Equals("TransitionA"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("TransitionD") && hNameX.Equals("TransitionB"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("TransitionD") && hNameX.Equals("TransitionC"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("TransitionD") && hNameX.Equals("TransitionD"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else
                    {
                        Assert.Fail("HNameY:" + hNameY + " HNameX:" + hNameX + " ResultMatrix["+y+", "+x+"]:" + resultMatrix[y, x]);
                    }
                    x++;
                }
                y++;
            }
        }

        /// <summary>
        /// Test for the creation of a footprint from a simple petrinet
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void ComparingFootprintFromPetriNetTest2()
        {
            //generate Footprint
            ComparingFootprint footprintResult = ComparingFootprintAlgorithm.CreateFootprint(PetriNetExample.PetriNetWithFivePlacesAndEightTransitions());
            CellType[,] resultMatrix = footprintResult.ResultMatrix;

            List<String> headerNames = footprintResult.HeaderWithEventNames;

            if (headerNames.Count != 8) Assert.Fail("ComparingFootprints header Names Count != 6");

            int y = 0;
            foreach (String hNameY in headerNames)
            {
                int x = 0;
                foreach (String hNameX in headerNames)
                {
                    if (hNameY.Equals("A") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("E"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("F"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("G"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("H"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("D"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Parallel))
                        {
                            //System.Diagnostics.Debug.WriteLine("Assert: " + hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("E"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);

                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("F"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            //System.Diagnostics.Debug.WriteLine("Assert: " + hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("G"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("H"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("A"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("B"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("C"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("D"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Parallel))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("E"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("F"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("G"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("H"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("A"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("B"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Parallel))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("C"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Parallel))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("D"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("E"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("F"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("G"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("H"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("B"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("C"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("D"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("E"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("F"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("G"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("H"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("F") && hNameX.Equals("A"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("F") && hNameX.Equals("B"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("F") && hNameX.Equals("C"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("F") && hNameX.Equals("D"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("F") && hNameX.Equals("E"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("F") && hNameX.Equals("F"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("F") && hNameX.Equals("G"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("F") && hNameX.Equals("H"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("G") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("G") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("G") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("G") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("G") && hNameX.Equals("E"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("G") && hNameX.Equals("F"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("G") && hNameX.Equals("G"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("G") && hNameX.Equals("H"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("H") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("H") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("H") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("H") && hNameX.Equals("D"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("H") && hNameX.Equals("E"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("H") && hNameX.Equals("F"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("H") && hNameX.Equals("G"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("H") && hNameX.Equals("H"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else
                    {
                        Assert.Fail("ComparingFootprintFromPetriNetTest2");
                    }
                    x++;
                }
                y++;
            }
        }


        /// <summary>
        /// Further test for the creation of a footprint from a complex petrinet
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void ComparingFootprintFromPetriNetTest3()
        {
            ComparingFootprint footprintResult = new ComparingFootprint();
            footprintResult = ComparingFootprintAlgorithm.CreateFootprint(PetriNetExample.PetriNetWithSixPlacesAndFiveTransitions());
            CellType[,] resultMatrix = footprintResult.ResultMatrix;

            List<String> headerNames = footprintResult.HeaderWithEventNames;

            if (headerNames.Count != 5) Assert.Fail("ComparingFootprints header Names Count != 6");

            int y = 0;
            foreach (String hNameY in headerNames)
            {
                int x = 0;
                foreach (String hNameX in headerNames)
                {
                    if (hNameY.Equals("A") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("E"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }

                    else if (hNameY.Equals("B") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("D"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Parallel))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("E"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("A"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("B"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("C"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("D"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Parallel))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("E"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("A"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("B"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Parallel))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("C"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Parallel))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("D"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("E"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }

                    else if (hNameY.Equals("E") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("B"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("C"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("D"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("E"))
                    {

                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail(hNameY + "->" + hNameX + " " + resultMatrix[y, x]);
                        }
                    }
                    else
                    {
                        Assert.Fail("ComparingFootprintFromPetriNetTest3");
                    }
                    x++;
                }
                y++;
            }
        }
    }
}
