using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Database;
using pgmpm.Database.Model;
using pgmpm.MainV2.Utilities;
using pgmpm.MiningAlgorithm;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MainV2Tests.FunctionsTests
{
    /// <summary>
    ///Dies ist eine Testklasse für "InterfaceHelpersTest" und soll
    ///alle InterfaceHelpersTest Komponententests enthalten.
    ///</summary>
    ///<author>Bernhard Bruns</author>
    [TestClass()]
    public class InterfaceHelpersTest
    {

        public static List<String> PageList = new List<string>() { "/Pages/P1connection.xaml", "/Pages/P2metadata.xaml", "/Pages/P3dimensionselection.xaml", "/Pages/P4eventselection.xaml", "/Pages/P5configuration.xaml", "/Pages/P6mining.xaml", "/Pages/P8results.xaml" };

        private TestContext _testContextInstance;

        /// <summary>
        ///Ruft den Testkontext auf, der Informationen
        ///über und Funktionalität für den aktuellen Testlauf bietet, oder legt diesen fest.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }

        /// <summary>
        ///Checks if the navigation to a lower page is allowed
        ///</summary>
        [TestMethod()]
        public void CheckIfNavigationIsAllowedTest()
        {
            bool expected = true;
            bool actual;

            DBWorker.MetaData = new MetaDataRepository();
            InterfaceHelpers.CurrentPage = PageList[6];

            foreach (String navigateToPage in PageList)
            {
                actual = InterfaceHelpers.CheckIfNavigationIsAllowed(navigateToPage);
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///Checks if the navigation to the next page is allowed
        ///</summary>
        [TestMethod()]
        public void CheckIfNavigationIsAllowedTest2()
        {
            // [0] P1connection.xaml", 
            // [1] P2metadata.xaml", 
            // [2] P3dimensionselection.xaml", 
            // [3] P4eventselection.xaml", 
            // [4] P5configuration.xaml", 
            // [5] P6mining.xaml", 
            // [6] P8results.xaml"

            bool expected = true;
            bool actual = true;
            DBWorker.MetaData = new MetaDataRepository();

            if (actual)
            {
                InterfaceHelpers.CurrentPage = PageList[0];
                actual = InterfaceHelpers.CheckIfNavigationIsAllowed(PageList[1]);
            }

            if (actual)
            {
                InterfaceHelpers.CurrentPage = PageList[1];
                actual = InterfaceHelpers.CheckIfNavigationIsAllowed(PageList[2]);
            }

            if (actual)
            {
                InterfaceHelpers.CurrentPage = PageList[2];
                actual = InterfaceHelpers.CheckIfNavigationIsAllowed(PageList[3]);
            }

            if (actual)
            {
                InterfaceHelpers.CurrentPage = PageList[3];
                actual = InterfaceHelpers.CheckIfNavigationIsAllowed(PageList[4]);
            }

            if (actual)
            {
                MinerSettings.IsAlgorithmSet = true;
                InterfaceHelpers.CurrentPage = PageList[4];
                actual = InterfaceHelpers.CheckIfNavigationIsAllowed(PageList[5]);
            }


            if (actual)
            {
                InterfaceHelpers.MiningIsCompleted = true;
                InterfaceHelpers.CurrentPage = PageList[5];
                actual = InterfaceHelpers.CheckIfNavigationIsAllowed(PageList[6]);
            }

            Assert.AreEqual(expected, actual);
        }



        /// <summary>
        ///Checks if the navigation is allowed, if the parameter value is a string which is not contained in the pagelist
        ///</summary>
        [TestMethod()]
        public void CheckIfNavigationIsAllowedTest3()
        {
            // [0] P1connection.xaml", 
            // [1] P2metadata.xaml", 
            // [2] P3dimensionselection.xaml", 
            // [3] P4eventselection.xaml", 
            // [4] P5configuration.xaml", 
            // [5] P6mining.xaml", 
            // [6] P8results.xaml"

            bool expected = true;
            bool actual;

            actual = InterfaceHelpers.CheckIfNavigationIsAllowed("StringThatIsNotInPageList");
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Checks if the navigation is allowed from P1Connection to P6Results if RestoreData==true
        ///</summary>
        [TestMethod()]
        public void CheckIfNavigationIsAllowedTest4()
        {
            // [0] P1connection.xaml", 
            // [1] P2metadata.xaml", 
            // [2] P3dimensionselection.xaml", 
            // [3] P4eventselection.xaml", 
            // [4] P5configuration.xaml", 
            // [5] P6mining.xaml", 
            // [6] P8results.xaml"

            bool expected = true;
            bool actual;

            InterfaceHelpers.CurrentPage = PageList[0];
            InterfaceHelpers.RestoreData = true;

            actual = InterfaceHelpers.CheckIfNavigationIsAllowed(PageList[6]);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Ein Test für "CheckIfNavigationIsAllowed"
        ///</summary>
        [TestMethod()]
        public void CheckIfNavigationIsAllowedTest5()
        {
            // [0] P1connection.xaml", 
            // [1] P2metadata.xaml", 
            // [2] P3dimensionselection.xaml", 
            // [3] P4eventselection.xaml", 
            // [4] P5configuration.xaml", 
            // [5] P6mining.xaml", 
            // [6] P8results.xaml"

            bool expected = true;
            bool actual = true;

            InterfaceHelpers.MiningIsCompleted = true;

            if (actual)
            {
                InterfaceHelpers.CurrentPage = PageList[5];
                actual = InterfaceHelpers.CheckIfNavigationIsAllowed(PageList[6]);
            }


            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///Ein Test für "CheckIfNavigationIsAllowed"
        ///</summary>
        [TestMethod()]
        public void CheckIfNavigationIsAllowedTest6()
        {
            // [0] P1connection.xaml", 
            // [1] P2metadata.xaml", 
            // [2] P3dimensionselection.xaml", 
            // [3] P4eventselection.xaml", 
            // [4] P5configuration.xaml", 
            // [5] P6mining.xaml", 
            // [6] P8results.xaml"

            bool expected = false;
            bool actual;

            InterfaceHelpers.CurrentPage = PageList[0];
            InterfaceHelpers.RestoreData = false;

            for (int i = 2; i <= 6; i++)
            {
                actual = InterfaceHelpers.CheckIfNavigationIsAllowed(PageList[i]);
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///Ein Test für "CreateButton"
        ///</summary>
        [TestMethod()]
        public void CreateButtonTest()
        {
            string name = "TestName";
            string text = "TestText";
            string style = string.Empty;

            Button expected = new Button
            {
                Name = name,
                Content = text,
                Margin = new Thickness(0, 0, 0, 0),
                Style = new Style()
            };

            Button actual;
            actual = InterfaceHelpers.CreateButton(name, text, style);
            Assert.IsTrue(actual.Name == expected.Name && actual.Content == expected.Content && actual.Margin == expected.Margin);

        }

        /// <summary>
        ///Ein Test für "CreateComboBox"
        ///</summary>
        [TestMethod()]
        public void CreateComboBoxTest()
        {
            string name = "TestName";
            string style = string.Empty;

            ComboBox expected = new ComboBox { Name = name, Margin = new Thickness(0, 0, 0, 0), Style = new Style() };
            ComboBox actual;
            actual = InterfaceHelpers.CreateComboBox(name, style);
            Assert.IsTrue(actual.Name == expected.Name && actual.Margin == expected.Margin);
        }


        /// <summary>
        ///Ein Test für "CreateComboBox"
        ///</summary>
        [TestMethod()]
        public void CreateComboBoxTest2()
        {
            string name = "TestName";
            string style = string.Empty;

            ComboBox expected = new ComboBox
            {
                Name = name,
                Margin = new Thickness(0, 0, 0, 0),
                Style = new Style(),
                Width = 1.0
            };

            ComboBox actual;
            actual = InterfaceHelpers.CreateComboBox(name, null, 1, 0, 0, 0, 0, 1, 1);
            Assert.IsTrue(actual.Name == expected.Name && actual.Margin == expected.Margin && actual.Width == expected.Width);
        }

        /// <summary>
        ///Ein Test für "CreateLabel"
        ///</summary>
        [TestMethod()]
        public void CreateLabelTest()
        {
            string text = "TestText";
            string style = string.Empty;

            Label expected = new Label { Content = text, Margin = new Thickness(0, 0, 0, 0), Style = new Style() };

            Label actual;
            actual = InterfaceHelpers.CreateLabel(text, style);
            Assert.IsTrue(actual.Content == expected.Content && actual.Margin == expected.Margin);
        }

        /// <summary>
        ///Ein Test für "CreateSeparator"
        ///</summary>
        [TestMethod()]
        public void CreateSeparatorTest()
        {

            string name = "TestName";
            string style = string.Empty;

            Separator expected = new Separator
            {
                Name = name,
                Margin = new Thickness(10, 10, 10, 10),
                Style = new Style()
            };

            Separator actual;
            actual = InterfaceHelpers.CreateSeparator(name, style);
            Assert.IsTrue(actual.Name == expected.Name && actual.Margin == expected.Margin);

        }

        /// <summary>
        ///Ein Test für "CreateTextBlock"
        ///</summary>
        [TestMethod()]
        public void CreateTextBlockTest()
        {
            string text = "TestText";
            string style = string.Empty;

            TextBlock expected = new TextBlock { Text = text, Margin = new Thickness(0, 0, 0, 0), Style = new Style() };

            TextBlock actual;
            actual = InterfaceHelpers.CreateTextBlock(text, style);
            Assert.IsTrue(actual.Text == expected.Text && actual.Margin == expected.Margin);

        }

        /// <summary>
        ///Ein Test für "CreateTextBox"
        ///</summary>
        [TestMethod()]
        public void CreateTextBoxTest()
        {

            string name = "TestName";
            string text = "TestText";
            string style = string.Empty;

            TextBox expected = new TextBox
            {
                Name = name,
                Text = text,
                Margin = new Thickness(0, 0, 0, 0),
                Style = new Style()
            };

            TextBox actual;
            actual = InterfaceHelpers.CreateTextBox(name, text, style);
            Assert.IsTrue(actual.Name == expected.Name && actual.Text == expected.Text && actual.Margin == expected.Margin);

        }

        /// <summary>
        ///Ein Test für "CurrentPage"
        ///</summary>
        [TestMethod()]
        public void CurrentPageTest()
        {
            string expected = PageList[0];
            string actual;
            InterfaceHelpers.CurrentPage = expected;
            actual = InterfaceHelpers.CurrentPage;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Ein Test für "MiningIsCompleted"
        ///</summary>
        [TestMethod()]
        public void MiningIsCompletedTest()
        {
            bool expected = true;
            bool actual;
            InterfaceHelpers.MiningIsCompleted = expected;
            actual = InterfaceHelpers.MiningIsCompleted;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Ein Test für "RestoreData"
        ///</summary>
        [TestMethod()]
        public void RestoreDataTest()
        {
            bool expected = true;
            bool actual;
            InterfaceHelpers.RestoreData = expected;
            actual = InterfaceHelpers.RestoreData;
            Assert.AreEqual(expected, actual);
        }
    }
}
