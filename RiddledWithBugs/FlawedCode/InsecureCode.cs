﻿using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Data.SqlClient;
using System.Net;


namespace MicrosoftSecurityCodeAnalysisTesting.FlawedCode
{
    internal class VulnerableClass
    {
        #region Constructor
        private readonly IConfiguration configuration;

        public VulnerableClass(IConfiguration config)
        {
            configuration = config;

            HardcodedPassword_1();
            HardcodedPassword_2();
            HardcodedPasswordInConfig();
            ShouldDispose();
            SqlInjection(1); //"magic number" - what is 1?
        }
        #endregion

        public string password = "TWFuIGlzIGRpc3Rpbmd1aXNoZWQsIG5vdCBvbmx5IGJ5IGhpcyByZWFzb24sIGJ1dCBieSB0aGlzCBvbmx5IGJ==";//vuln #1 (when used with method HardcodedPassword_2 )
        
        private static void HardcodedPassword_1()
        {
            using (var client = new WebClient())
            {
                client.Credentials = new System.Net.NetworkCredential("UserName", "TWFuIGlzIGRpc3Rpbmd1aXNoZWQsIG5vdCBvbmx5IGJ5IGhpcyByZWFzb24sIGJ1dCBieSB0aGlzCBvbmx5IGJ==", "Domain"); //vuln #2 (hardcoded password)
            }
        }
         
        private void HardcodedPassword_2()
        {
            using (var client = new WebClient())
            {
                client.Credentials = new System.Net.NetworkCredential("UserName", password, "Domain"); //vuln #3 (using hardcoded password in variable)
            }
        }
        

        private void HardcodedPasswordInConfig()
        {
            using (var client = new SqlConnection())
            {
                client.ConnectionString = configuration.GetConnectionString("myDb1"); //vuln #4 (see config - contains hardcoded secrets)
            }
        }

        internal static void ShouldDispose()
        {
            var con = new System.Timers.Timer//memory leak - not disposed
            {
                AutoReset = false
            };

            con.Stop();
        }

        internal static void SqlInjection(int fromClient)
        {
            SqlDataReader reader = null;

            using (var command = new SqlCommand
            {
                CommandText = "SELECT * FROM Table WHERE SomeColumn = '" + fromClient + "'", //vuln #5 (SQLi using concatenated string) AND ToString can be overridden (CA2100 SDL rules)
                CommandType = System.Data.CommandType.Text
            })
            {
                 reader = command.ExecuteReader(); //memory leak #2?
                
                #region Hiding bit that is not interesting
                while (reader.Read())
                {
                    //do something
                }
                #endregion
            }
        }

        public static void StorageCredentialsHardCoded()
        {
            var creds = new StorageCredentials("CredScan", "TWFuIGlzIGRpc3Rpbmd1aXNoZWQsIG5vdCBvbmx5IGJ5IGhpcyByZWFzb24sIGJ1dCBieSB0aGlzCBvbmx5IGJ==", "MyCreds");

        }
    }
}
