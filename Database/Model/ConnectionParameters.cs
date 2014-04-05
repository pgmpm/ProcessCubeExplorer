using System;

namespace pgmpm.Database.Model
{
    /// <summary>
    /// An abstract data Type that stores the necessary information to connect to a Database. This is set in the wpfDatabaseConnection-Window.
    /// Note: for Oracle-Services use Database-field!
    /// </summary>
    /// <author>Bernhard Bruns, Jannik Arndt, Bernd Nottbeck</author>
    [Serializable()]
    public class ConnectionParameters
    {
        public String Name { get; set; }
        public String Type;
        public String Host;
        public String Database;
        public String User;
        public String Port;
        public bool IsLastUsedDatabase;
        private String Salt;
        private string _password;

        /// <summary>
        /// Setter encrypts a password and saves it in private _password.
        /// Getter returns "" if no password is set. Decrypts Password and returns it to the user.
        /// </summary>
        public String Password
        {
            get
            {
                if (String.IsNullOrEmpty(_password))
                {
                    return "";
                }
                else
                {
                    return CipherUtility.Decrypt(_password, Salt);
                }
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    _password = "";
                }
                else
                {
                    _password = CipherUtility.Encrypt(value, Salt);
                }
            }
        }

        /// <summary>
        /// Creates a connection parameter object an uses System.Guid.NewGuid()  to generate a salt value.
        /// </summary>
        /// <param name="type">Connection type</param>
        /// <param name="name">Connection name</param>
        /// <param name="host">Host IP</param>
        /// <param name="database">Database</param>
        /// <param name="user">User</param>
        /// <param name="password">Password</param>
        /// <param name="port">Port</param>
        public ConnectionParameters(String type = "", String name = "", String host = "", String database = "", String user = "", String password = "", String port = "")
        {
            Salt = CipherUtility.CreateSalt();
            Type = type;
            Name = name;
            Host = host;
            Database = database;
            User = user;
            Password = password;
            Port = port;
        }

        /// <summary>
        /// Checks the completeness of the parameters
        /// </summary>
        /// <returns>true, if all required connection parameters have not an empty string </returns>
        public bool IsComplete()
        {
            if (Name == "" || Host == "" || Port == "" || User == "" || Type == "")
                return false;
            return true;
        }
    }
}