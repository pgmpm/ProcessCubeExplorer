using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.ConformanceChecking;
using pgmpm.ExampleData;
using pgmpm.MatrixSelection.Fields;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using EventLog = pgmpm.MatrixSelection.Fields.EventLog;

namespace pgmpm.ConformanceCheckingTests
{
    /// <summary>
    /// Footprint tests
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    [TestClass]
    public class ComparingFootprintAlgorithmTests
    {
        /// <summary>
        /// AddEventHeader Test
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void AddEventHeaderTest()
        {
            ComparingFootprint footprint = new ComparingFootprint();
            footprint.AddEventHeader("A");
            footprint.AddEventHeader("B");

            Assert.IsTrue(footprint.HeaderWithEventNames.Contains("A"));
            Assert.IsTrue(footprint.HeaderWithEventNames.Contains("B"));
        }

        /// <summary>
        /// Test for the extraction of the eventnames (headernames) from an eventlog
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void CaseToFootprintTestHeaderWithEventNames1()
        {
            //create Footprint
            ComparingFootprint foot = ComparingFootprintAlgorithm.CreateFootprint(EventLogExample.OneCaseEventLogWithFourOrderedEvents());

            //getHeaderNames
            List<String> headerNames = foot.HeaderWithEventNames;

            int i = 0;
            foreach (String hName in headerNames)
            {
                Debug.WriteLine("hName: " + hName);

                if (hName.Equals("A") && i == 0)
                {
                }
                else if (hName.Equals("B") && i == 1)
                {
                }
                else if (hName.Equals("C") && i == 2)
                {
                }
                else if (hName.Equals("D") && i == 3)
                {
                }
                else
                {
                    Assert.Fail();
                }

                i++;
            }
        }

        /// <summary>
        /// Test for the generation of a footprint from a simple eventlog
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void CaseToFootprintResultMatrixTest1()
        {
            //create Footprint
            ComparingFootprint foot = ComparingFootprintAlgorithm.CreateFootprint(EventLogExample.OneCaseEventLogWithFourOrderedEvents());

            CellType[,] resultMatrix = foot.ResultMatrix;

            List<String> headerNames = foot.HeaderWithEventNames;

            int y = 0;
            foreach (String hNameY in headerNames)
            {
                int x = 0;
                foreach (String hNameX in headerNames)
                {
                    //For Debugging:
                    if (resultMatrix[y, x] != null)
                        Debug.WriteLine("hNameY: " + hNameY + ", hNameX: " + hNameX + " [" + y + "," + x + "].CellType:" + resultMatrix[y, x]);

                    if (hNameY.Equals("A") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else
                    {
                        Assert.Fail();
                    }
                    x++;
                }
                y++;
            }
        }

        /// <summary>
        /// This test compares the footprint of a given eventlog
        /// with the Result Footprint of the ComparingFootprintAlgorithm.
        /// </summary>
        /// <author>Markus Holznagel</author>
        [TestMethod]
        public void CaseToFootprintResultMatrixTest1A()
        {
            // Create Footprint
            ComparingFootprint foot = ComparingFootprintAlgorithm.CreateFootprint(EventLogExample.OneCaseEventLogWithFourOrderedEvents());

            CellType[,] resultMatrix = foot.ResultMatrix;

            CellType nothing = CellType.Nothing;
            CellType right = CellType.Right;
            CellType left = CellType.Left;

            // Given Footprint
            CellType[,] resultFootprint =
            {
                // A,       B,       C,       D
                {nothing, right,   nothing, nothing}, // A
                {left,    nothing, right,   nothing}, // B
                {nothing, left,    nothing, right},   // C
                {nothing, nothing, left,    nothing}  // D
            };

            for (int y = 0; y < resultMatrix.GetLength(0); ++y)
            {
                Console.WriteLine(resultMatrix.Length);
                for (int x = 0; x < resultMatrix.GetLength(1); ++x)
                {
                    //For debugging:
                    //Console.WriteLine("y :" + y + "x :" + x + " " + resultMatrix[y, x]);
                    if (resultMatrix[y, x] != resultFootprint[y, x])
                    {
                        Assert.Fail("y: " + y + "x: " + x + " " + resultMatrix[y, x]);
                    }
                }
            }
        }

        /// <summary>
        /// Further test for the generation of a footprint from an eventlog
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void CaseToFootprintResultMatrixTest2()
        {
            //create Footprint
            ComparingFootprint foot = ComparingFootprintAlgorithm.CreateFootprint(EventLogExample.OneCaseEventLogSwitchTheMiddle());

            CellType[,] resultMatrix = foot.ResultMatrix;

            List<String> headerNames = foot.HeaderWithEventNames;

            int y = 0;
            foreach (String hNameY in headerNames)
            {
                int x = 0;
                foreach (String hNameX in headerNames)
                {
                    //For debugging:
                    //if (resultMatrix[y, x] != null)
                    //    Debug.WriteLine("hNameY: " + hNameY + ", hNameX: " + hNameX + " [" + y + "," + x + "].CellType:" + resultMatrix[y, x]);

                    if (hNameY.Equals("A") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else
                    {
                        Assert.Fail();
                    }
                    x++;
                }
                y++;
            }
        }

        /// <summary>
        /// This method tests, if the Footprint of the event log is right.
        /// The event log is from the Process Mining book of van der Aalst site 195 Table 7.1.
        /// </summary>
        /// <autor>Markus Holznagel</autor>
        [TestMethod]
        public void CaseToFootprintResultMatrixTest3()
        {
            //create Footprint
            ComparingFootprint foot = ComparingFootprintAlgorithm.CreateFootprint(EventLogExample.ComplexEventLogVanDerAalst());

            CellType[,] resultMatrix = foot.ResultMatrix;

            List<String> headerNames = foot.HeaderWithEventNames;

            int y = 0;
            foreach (String hNameY in headerNames)
            {
                int x = 0;
                foreach (String hNameX in headerNames)
                {
                    //For debugging:
                    //if (resultMatrix[y, x] != null)
                    //    Debug.WriteLine("hNameY: " + hNameY + ", hNameX: " + hNameX + " [" + y + "," + x + "].CellType:" + resultMatrix[y, x]);
                    if (hNameY.Equals("A") && hNameX.Equals("A"))
                    {
                        // The test failed if the celltype of the matrix cell at the position x:A and y:A are not nothing
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("B"))
                    {
                        // The result of A and B will store in c.

                        // If the result in c isn't right, the test failed.
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("C"))
                    {
                        // The result of A and C will store in c.

                        // If the result in c isn't right, the test failed.
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("D"))
                    {
                        // The result of A and D will store in c.

                        // If the result in c isn't right, the test failed.
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("E"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("F"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("G"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("H"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Parallel))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("E"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("F"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("G"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("H"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Parallel))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("E"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("F"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("G"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("H"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Parallel))
                        {
                            Assert.Fail("hNameY:" + hNameY + " hNameX" + hNameX + " celltype:" + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Parallel))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("E"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("F"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("G"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("H"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("E"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("F"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("G"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("E") && hNameX.Equals("H"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("F") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("F") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("F") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("F") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("F") && hNameX.Equals("E"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail("hNameY:" + hNameY + " hNameX" + hNameX + " celltype:" + resultMatrix[y, x]);
                        }
                    }
                    else if (hNameY.Equals("F") && hNameX.Equals("F"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("F") && hNameX.Equals("G"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("F") && hNameX.Equals("H"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("G") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("G") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("G") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("G") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("G") && hNameX.Equals("E"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("G") && hNameX.Equals("F"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("G") && hNameX.Equals("G"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("G") && hNameX.Equals("H"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("H") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("H") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("H") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("H") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("H") && hNameX.Equals("E"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("H") && hNameX.Equals("F"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("H") && hNameX.Equals("G"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("H") && hNameX.Equals("H"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else
                    {
                        Assert.Fail();
                    }
                    x++;
                }
                y++;
            }
        }

        /// <summary>
        /// Complex Test: Generation of an eventlog with three list of cases to a footprint
        /// </summary>
        /// <autor>Markus Holznagel</autor>
        [TestMethod]
        public void CaseToFootprintFromPetriNetTest4()
        {
            // Create Footprint
            ComparingFootprint foot = ComparingFootprintAlgorithm.CreateFootprint(EventLogExample.ThreeCaseEventLog());

            CellType[,] resultMatrix = foot.ResultMatrix;

            List<String> headerNames = foot.HeaderWithEventNames;

            if (headerNames.Count != 5) Assert.Fail("ComparingFootprints header Names Count != 5");

            int y = 0;
            foreach (String hNameY in headerNames)
            {
                int x = 0;
                foreach (String hNameX in headerNames)
                {
                    //For debugging:
                    //if (resultMatrix[y, x] != null)
                    //    Debug.WriteLine("Test Y: " + hNameY + " X: " + hNameX + " [" + y + "," + x + "].CellType:" + resultMatrix[y, x]);

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
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
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
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
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
                        Assert.Fail("ComparingFootprintFromPetriNetTest4");
                    }
                    x++;
                }
                y++;
            }
        }

        /// <summary>
        /// less lines of code
        /// Complex Test: Generation of an eventlog with three list of cases to a footprint
        /// </summary>
        /// <author>Markus Holznagel</author>
        [TestMethod]
        public void CaseToFootprintFromPetriNetTest4a()
        {
            ComparingFootprint foot = ComparingFootprintAlgorithm.CreateFootprint(EventLogExample.ThreeCaseEventLog());

            CellType[,] resultMatrix = foot.ResultMatrix;

            CellType nothing = CellType.Nothing;
            CellType left = CellType.Left;
            CellType right = CellType.Right;
            CellType parallel = CellType.Parallel;

            // Footprint
            CellType[,] array =
            {
                // A,       B,       D,        E        C
                {nothing, right,    right,    nothing, right},   // A
                {left,    nothing,  parallel, right,   nothing}, // B
                {left,    parallel, nothing,  right,   nothing}, // D
                {nothing, left,     left,     nothing, left},    // E
                {left,    nothing,  nothing,  right, nothing}  // C
            };

            //   A,       B,        C,       D,        E
            //{nothing, right,    right,   right,    nothing}, A
            //{left,    nothing,  nothing, IsParallel, right},   B
            //{left,    nothing,  nothing, nothing,  right},   C
            //{left,    IsParallel, nothing, nothing,  right},   D
            //{nothing, left,     left,    left,     nothing}  E

            for (int y = 0; y < 5; ++y)
            {
                for (int x = 0; x < 5; ++x)
                {
                    //For debugging:
                    //Console.WriteLine("y: " + y + "x: " + x + " " + resultMatrix[y, x]);
                    if (resultMatrix[y, x] != array[y, x])
                    {
                        Assert.Fail("y: " + y + "x: " + x + " " + resultMatrix[y, x]);
                    }
                }
            }
        }

        /// <summeray>
        /// This method test a bigger footprint
        /// </summeray>
        /// <author>Markus Holznagel</author>
        [TestMethod]
        public void CaseToFootprintFromPetriNetTest5()
        {
            // Create Footprint
            ComparingFootprint foot = ComparingFootprintAlgorithm.CreateFootprint(EventLogExample.EventLogForRecursiveParallelisms());

            CellType[,] resultMatrix = foot.ResultMatrix;
            foreach (String x in foot.HeaderWithEventNames)
            {
                Console.WriteLine(x);
            }
            foreach (String y in foot.HeaderWithEventNames)
            {
                Console.WriteLine(y);
            }

            List<String> headerNames = foot.HeaderWithEventNames;

            if (headerNames.Count != 14) Assert.Fail("ComparingFootprints headerNames != 14");

            CellType nothing = CellType.Nothing;
            CellType right = CellType.Right;
            CellType left = CellType.Left;
            CellType parallel = CellType.Parallel;

            CellType[,] resultFootprint =
            {
                // A,       B,        C,        D,        F,        H,        J,        L,        M,        N,       K,        I,        G,        E
                {nothing, right,    nothing,  nothing,  nothing,  nothing,  nothing,  nothing,  nothing,  nothing, nothing,  nothing,  nothing,  nothing},  // A
                {left,    nothing,  right,    nothing,  nothing,  nothing,  nothing,  nothing,  nothing,  nothing, nothing,  nothing,  nothing,  nothing},    // B
                {nothing, left,     nothing,  right,    nothing,  nothing,  nothing,  nothing,  nothing,  nothing, nothing,  nothing,  nothing,  right}, // C
                {nothing, nothing,   left, nothing,     right,    nothing,  nothing,  nothing,  nothing,  nothing, nothing,  nothing,  right,    nothing},  // D
                {nothing, nothing,  nothing,  left,     nothing,  right,    nothing,  nothing,  nothing,  nothing, nothing,  right,    nothing,  left}, // F
                {nothing, nothing,  nothing,  nothing,  left,     nothing,  right,    nothing,  nothing,  nothing, right,  nothing,  left,  nothing}, // H
                {nothing, nothing,  nothing,  nothing,  nothing,  left,     nothing,  right,    nothing,  nothing, nothing,  left, nothing, nothing}, // J
                {nothing, nothing,  nothing,  nothing,  nothing,  nothing,  left,     nothing,  right,    nothing, left,    nothing,  nothing,     nothing},     // L
                {nothing, nothing,  nothing,  nothing,  nothing, nothing,   nothing,  left,    nothing,  right,   nothing, nothing,  nothing, nothing}, // M
                {nothing, nothing,  nothing,  nothing,  nothing, nothing,   nothing,  nothing,  left,   nothing, nothing, nothing, nothing, nothing},  // N
                {nothing, nothing,  nothing,  nothing,  nothing, left,      nothing,  right,   nothing, nothing, nothing,  left, nothing, nothing}, // K
                {nothing, nothing,  nothing,  nothing,  left,    nothing,   right,    nothing, nothing, nothing, right, nothing,  left, nothing}, // I
                {nothing, nothing,  nothing, left,   nothing, right,   nothing,  nothing, nothing, nothing, nothing, right, nothing,  left}, // G
                {nothing, nothing,  left, nothing,   right, nothing,   nothing,  nothing, nothing, nothing, nothing, nothing, right, nothing}   // E
            };

            for (int y = 0; y < resultMatrix.GetLength(0); ++y)
            {
                for (int x = 0; x < resultMatrix.GetLength(1); ++x)
                {
                    //For debugging: Console.WriteLine("yName: " + foot.HeaderWithEventNames[y] + " " + "xName: " + foot.HeaderWithEventNames[x] + " " + resultMatrix[y, x]);
                    if (resultMatrix[y, x] != resultFootprint[y, x])
                    {
                        Assert.Fail("y:"+y+" x:"+x+" yName: " + foot.HeaderWithEventNames[y] + " " + "xName: " + foot.HeaderWithEventNames[x] + " " + resultMatrix[y, x]);
                    }
                }
            }
        }

        /// <summary>
        /// Test for a merge of much footprints to only one footprint
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void FootprintMergeParallelismTest1()
        {
            //create Footprint list
            List<ComparingFootprint> footprintList = new List<ComparingFootprint>();

            ComparingFootprint foot1 = ComparingFootprintAlgorithm.CreateFootprint(EventLogExample.OneCaseEventLogWithFourOrderedEvents());
            footprintList.Add(foot1);

            ComparingFootprint foot2 = ComparingFootprintAlgorithm.CreateFootprint(EventLogExample.OneCaseEventLogSwitchTheMiddle());
            footprintList.Add(foot2);

            ComparingFootprint footResult = ComparingFootprintAlgorithm.MergeFootprints(footprintList);

            CellType[,] resultMatrix = footResult.ResultMatrix;

            List<String> headerNames = footResult.HeaderWithEventNames;

            int y = 0;
            foreach (String hNameY in headerNames)
            {
                int x = 0;
                foreach (String hNameX in headerNames)
                {
                    //For development:
                    //if (resultMatrix[y, x] != null)
                    //{
                    //    Debug.WriteLine("hNameY: " + hNameY + ", hNameX: " + hNameX + " [" + y + "," + x + "].CellType:" + resultMatrix[y, x]);
                    //}

                    if (hNameY.Equals("A") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Parallel))
                        {
                            //right oder left? -> Parallel is right
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Parallel))
                        {
                            //right oder left? -> Parallel is right
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else
                    {
                        Assert.Fail();
                    }
                    x++;
                }
                y++;
            }
        }

        /// <summary>
        /// Test for the right headernames (eventnames) in the result footprint after the merge process
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void FootprintMergeHeaderNameTest1()
        {
            //create Case1
            Case ca = new Case();
            ca.CreateEvent("A");
            ca.CreateEvent("B");
            ca.CreateEvent("C");
            ca.CreateEvent("D");

            //create Case2
            Case ca2 = new Case();
            ca2.CreateEvent("AA");
            ca2.CreateEvent("CC");
            ca2.CreateEvent("BB");
            ca2.CreateEvent("DD");

            //create Case3
            Case ca3 = new Case();
            ca2.CreateEvent("A");
            ca2.CreateEvent("CC");
            ca2.CreateEvent("B");
            ca2.CreateEvent("ZZZ");

            //create Footprint list
            List<ComparingFootprint> footprintList = new List<ComparingFootprint>();

            ComparingFootprint foot1 = ComparingFootprintAlgorithm.CreateFootprint(ca);
            footprintList.Add(foot1);

            ComparingFootprint foot2 = ComparingFootprintAlgorithm.CreateFootprint(ca2);
            footprintList.Add(foot2);

            ComparingFootprint foot3 = ComparingFootprintAlgorithm.CreateFootprint(ca3);
            footprintList.Add(foot3);

            ComparingFootprint footResult = ComparingFootprintAlgorithm.MergeFootprints(footprintList);

            List<String> headerNames = footResult.HeaderWithEventNames;

            int i = 0;
            foreach (String hName in headerNames)
            {
                if (hName.Equals("A"))
                {
                    i++;
                }
                else if (hName.Equals("B"))
                {
                    i++;
                }
                else if (hName.Equals("C"))
                {
                    i++;
                }
                else if (hName.Equals("D"))
                {
                    i++;
                }
                else if (hName.Equals("AA"))
                {
                    i++;
                }
                else if (hName.Equals("BB"))
                {
                    i++;
                }
                else if (hName.Equals("CC"))
                {
                    i++;
                }
                else if (hName.Equals("DD"))
                {
                    i++;
                }
                else if (hName.Equals("ZZZ"))
                {
                    i++;
                }
                else
                {
                    Assert.Fail();
                }
            }

            if (i != 9 || 9 != headerNames.Count || i != headerNames.Count)
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Test for the creation of the right footprint from a simple eventlog
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void CreateFootprintFromEventLogTest1()
        {
            //create Case1
            Case ca = new Case();
            ca.CreateEvent("A");
            ca.CreateEvent("B");
            ca.CreateEvent("C");
            ca.CreateEvent("D");

            //create Case2
            Case ca2 = new Case();
            ca2.CreateEvent("AA");
            ca2.CreateEvent("CC");
            ca2.CreateEvent("BB");
            ca2.CreateEvent("DD");

            //create Case3
            Case ca3 = new Case();
            ca2.CreateEvent("A");
            ca2.CreateEvent("CC");
            ca2.CreateEvent("B");
            ca2.CreateEvent("ZZZ");

            EventLog eventLog = new EventLog();
            eventLog.Cases.Add(ca);
            eventLog.Cases.Add(ca2);
            eventLog.Cases.Add(ca3);

            ComparingFootprint footResult = ComparingFootprintAlgorithm.CreateFootprint(eventLog);

            List<String> headerNames = footResult.HeaderWithEventNames;

            int i = 0;
            foreach (String hName in headerNames)
            {
                if (hName.Equals("A"))
                {
                    i++;
                }
                else if (hName.Equals("B"))
                {
                    i++;
                }
                else if (hName.Equals("C"))
                {
                    i++;
                }
                else if (hName.Equals("D"))
                {
                    i++;
                }
                else if (hName.Equals("AA"))
                {
                    i++;
                }
                else if (hName.Equals("BB"))
                {
                    i++;
                }
                else if (hName.Equals("CC"))
                {
                    i++;
                }
                else if (hName.Equals("DD"))
                {
                    i++;
                }
                else if (hName.Equals("ZZZ"))
                {
                    i++;
                }
                else
                {
                    Assert.Fail();
                }
            }

            if (i != 9 || 9 != headerNames.Count || i != headerNames.Count)
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Further test for the creation of the right footprint from an eventlog
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void CreateFootprintFromEventLogTest2()
        {
            ComparingFootprint footResult = ComparingFootprintAlgorithm.CreateFootprint(EventLogExample.TwoCaseEventLog());

            CellType[,] resultMatrix = footResult.ResultMatrix;

            List<String> headerNames = footResult.HeaderWithEventNames;

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
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("A") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Parallel))
                        {
                            //right oder left? -> parallel is right
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("B") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Parallel))
                        {
                            //right oder left? -> parallel is right
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("C") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Right))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("A"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("B"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("C"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Left))
                        {
                            Assert.Fail();
                        }
                    }
                    else if (hNameY.Equals("D") && hNameX.Equals("D"))
                    {
                        if (!resultMatrix[y, x].Equals(CellType.Nothing))
                        {
                            Assert.Fail();
                        }
                    }
                    else
                    {
                        Assert.Fail();
                    }
                    x++;
                }
                y++;
            }
        }

        /// <summary>
        /// Test for the calculation of the fitness which must throw an exception
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void CalculateFitnessComparingFootprintDivideByZeroTest1()
        {
            try
            {
                double resultFitness = ComparingFootprintAlgorithm.CalculateFitness(0, 0);
                Assert.Fail();
            }
            catch (DivideByZeroException)
            {
                //divide by zero exception
                //do nothing
                //this is the expected catching claus for the test
            }

            try
            {
                double resultFitness = ComparingFootprintAlgorithm.CalculateFitness(1, 0);
                Assert.Fail();
            }
            catch (DivideByZeroException)
            {
                //divide by zero exception
                //do nothing
            }
        }

        /// <summary>
        /// Some tests, for the calculation of the right fitness for given parameter
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void CalculateFitnessComparingFootprintTest1()
        {
            double resultFitness = ComparingFootprintAlgorithm.CalculateFitness(0, 1);
            Assert.AreEqual(resultFitness, 1);

            double resultFitness02 = ComparingFootprintAlgorithm.CalculateFitness(0, 2);
            Assert.AreEqual(resultFitness02, 1);

            double resultFitness2 = ComparingFootprintAlgorithm.CalculateFitness(1, 2);
            Assert.AreEqual(resultFitness2, 0.5);

            double resultFitness22 = ComparingFootprintAlgorithm.CalculateFitness(2, 2);
            Assert.AreEqual(resultFitness22, 0);

            double resultFitness13 = ComparingFootprintAlgorithm.CalculateFitness(1, 3);
            Assert.AreEqual(resultFitness13, 1.0 - (1.0 / 3.0));

            double resultFitness23 = ComparingFootprintAlgorithm.CalculateFitness(2, 3);
            Assert.AreEqual(resultFitness23, 1.0 - (2.0 / 3.0));

            double resultFitness33 = ComparingFootprintAlgorithm.CalculateFitness(3, 3);
            Assert.AreEqual(resultFitness33, 1.0 - (3.0 / 3.0));
        }

        /// <summary>
        /// Test for the calculation of the fitness with wrong parameters
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        [TestMethod]
        public void CalculateFitnessComparingFootprintTest2()
        {
            try
            {
                double resultFitnessMinus = ComparingFootprintAlgorithm.CalculateFitness(-1, 3);
                Assert.Fail();
            }
            catch (Exception) { }

            try
            {
                double resultFitnessMinus = ComparingFootprintAlgorithm.CalculateFitness(3, -3);
                Assert.Fail();
            }
            catch (Exception) { }

            try
            {
                double resultFitnessMinus = ComparingFootprintAlgorithm.CalculateFitness(-5, -5);
                Assert.Fail();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Test for the List Adding Footprint Headernames method (AddEventHeader(List<String> headerWithEventNames))
        /// </summary>
        /// <autor>Naby Moussa Sow</autor>
        [TestMethod]
        public void AddEventHeader_WithEventNames_Test()
        {
            // Arrange
            //create a Test Footprint header
            EventLog eventLog_test = EventLogExample.OneCaseEventLogWithFourOrderedEvents();

            ComparingFootprintResultMatrix foot = new ComparingFootprintResultMatrix();

            List<string> listWithStrings = new List<string> {"A", "B", "C", "D"};
            foot.AddEventHeader(listWithStrings);

            //Act 
            foreach (string eventTest in foot.HeaderWithEventNames){
                if (eventTest.Equals("A") || eventTest.Equals("B") || eventTest.Equals("C") || eventTest.Equals("D"))
                {

                }
                else 
                {
                    Assert.Fail(eventTest + " is not in list");
                }
            }

         
        }

       
    }
}
