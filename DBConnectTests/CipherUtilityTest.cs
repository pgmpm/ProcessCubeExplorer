using pgmpm.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography;

namespace pgmpm.DBConnectTests
{
    /// <summary>
    ///This is a test class for CipherUtilityTest and is intended
    ///to contain all CipherUtilityTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CipherUtilityTest
    {
        private TestContext _testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
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
        ///A test for CipherUtility Constructor
        ///</summary>
        /// <author>Bernd Nottbeck</author>
        [TestMethod()]
        public void CipherUtilityConstructorTest()
        {
            CipherUtility target = new CipherUtility();
            Assert.IsNotNull(target);
        }

        ///<summary>
        ///A test for CreateSalt
        ///</summary>
        ///<author>Bernd Nottbeck</author>
        [TestMethod()]
        public void CreateSaltTest()
        {
            string actual;
            actual = CipherUtility.CreateSalt();
            Assert.IsTrue(!String.IsNullOrEmpty(actual));
        }

        /// <summary>
        /// Decryption tTest with an empty password.
        /// </summary>
        /// <author>Bernd Nottbeck</author>
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void DecryptExceptionTest()
        {
            string text = "Teststring";
            string password = string.Empty;
            string salt = "Teststring";
            string actual;
            actual = CipherUtility.Decrypt<AesManaged>(text, password, salt);
            Assert.Fail("An Exception should have been thrown");
        }

        /// <summary>
        /// Encryption Test  with an empty salt. Should throw an Exception.
        /// </summary>
        /// <author>Bernd Nottbeck</author>
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void EncryptionExceptionTest1()
        {
            string value = "Teststing";
            CipherUtility.Encrypt(value, "");
        }

        /// <summary>
        /// Encryption Test  with an empty password. Should throw an Exception.
        /// </summary>
        /// <author>Bernd Nottbeck</author>
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void EncryptionExceptionTest2()
        {
            string salt = "Teststing";
            CipherUtility.Encrypt("", salt);
        }

        /// <summary>
        /// Encryption Test  with an empty AesKey. Should throw an Exception.
        /// </summary>
        /// <author>Bernd Nottbeck</author>
        [TestMethod()]
        public void EncryptionEmptyAesKeyTest()
        {
            String tempAeskey = CipherUtility.TemporaryAESKeyDelete();
            Boolean testSuccessful = false;

              Assert.IsFalse(String.IsNullOrEmpty(CipherUtility.Encrypt("Teststring", "Teststting")));
            

                testSuccessful = CipherUtility.TemporaryAESKeyRestore(tempAeskey);

            Assert.IsTrue(testSuccessful);
        }

        /// <summary>
        /// Decryption Test  with an empty AesKey. Should throw an Exception.
        /// </summary>
        /// <author>Bernd Nottbeck</author>
        [TestMethod()]
        public void DecryptionEmptyAesKeyTest()
        {
            String tempAeskey = CipherUtility.TemporaryAESKeyDelete();
            Boolean testSuccessful = false;
            try
            {
                CipherUtility.Decrypt("Teststring", "Teststting");
            }
            catch 
            {
                testSuccessful = CipherUtility.TemporaryAESKeyRestore(tempAeskey);
            }
            Assert.IsTrue(testSuccessful);
        }

    }
}
