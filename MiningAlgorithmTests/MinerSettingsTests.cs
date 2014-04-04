using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.MiningAlgorithm;

namespace pgmpm.MiningAlgorithmTests
{
    /// <summary>
    /// Unit-Tests for MinerSettings
    /// </summary>
    [TestClass]
    public class MinerSettingsTests
    {
        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void MinerSettingsGetValueAsObject()
        {
            var myObject = new Object();
            MinerSettings.AddOrUpdateKey("Test", myObject);

            var actual = MinerSettings.Get("Test");
            Assert.AreEqual(myObject, actual);

            actual = MinerSettings.Get("Does not exist");
            Assert.IsNull(actual);
        }

        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void MinerSettingsGetValueAsDouble()
        {
            MinerSettings.AddOrUpdateKey("Test", 42.0);

            var actual = MinerSettings.GetAsDouble("Test");
            Assert.AreEqual(42.0, actual);

            actual = MinerSettings.GetAsDouble("Does not exist");
            Assert.AreEqual(-1, actual);
        }

        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void MinerSettingsGetValueAsInt()
        {
            MinerSettings.AddOrUpdateKey("Test", 42);

            var actual = MinerSettings.GetAsInt("Test");
            Assert.AreEqual(42, actual);

            actual = MinerSettings.GetAsInt("Does not exist");
            Assert.AreEqual(-1, actual);
        }

        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void MinerSettingsGetValueAsString()
        {
            MinerSettings.AddOrUpdateKey("Test1", "TestString");
            MinerSettings.AddOrUpdateKey("Test2", 42);

            var actual = MinerSettings.GetAsString("Test1");
            Assert.AreEqual("TestString", actual);

            actual = MinerSettings.GetAsString("Test2");
            Assert.AreEqual("42", actual);

            actual = MinerSettings.GetAsString("Does not exist");
            Assert.IsNull(actual);
        }
    }
}
