using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Diff.DiffAlgorithm;
using pgmpm.MatrixSelection.Fields;

namespace pgmpm.Diff.Tests
{
    [TestClass()]
    public class DiffFactoryTests
    {
        /// <author>Thomas Meents</author>
        [TestMethod()]
        public void CreateDiffTest()
        {
            Field diffField = new Field();
            var diff = DiffFactory.CreateDiff<SnapshotDiff>(diffField);
            Assert.IsInstanceOfType(diff, typeof(SnapshotDiff));
        }
    }
}