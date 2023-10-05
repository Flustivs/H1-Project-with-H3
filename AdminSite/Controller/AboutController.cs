using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AdminSite.Controller
{
    public class AboutController
    {
        static string columnValue;
        public string connect(byte i)
        {
            //string datasource = @"LAPTOP-94N0K9HA\MSSQLSERVER01"; // Test on local pc
            string connString = @"Data Source=LAPTOP-94N0K9HA\MSSQLSERVER01;Initial Catalog=SuperFunFunParkDB;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection conn = new SqlConnection(connString); // Makes a new connection to Database

            try
            {
                conn.Open(); // if theres a connection it opens it

                string english1 = "SELECT aboutUsTranslationText FROM tblAboutUsTranslation WHERE aboutUsTranslationID = 1";
                string english2 = "SELECT aboutUsTranslationText FROM tblAboutUsTranslation WHERE aboutUsTranslationID = 2";
                string english3 = "SELECT aboutUsTranslationText FROM tblAboutUsTranslation WHERE aboutUsTranslationID = 3";

                string danish1 = "SELECT aboutUsTranslationText FROM tblAboutUsTranslation WHERE aboutUsTranslationID = 4";
                string danish2 = "SELECT aboutUsTranslationText FROM tblAboutUsTranslation WHERE aboutUsTranslationID = 5";
                string danish3 = "SELECT aboutUsTranslationText FROM tblAboutUsTranslation WHERE aboutUsTranslationID = 6";
                switch (i)
                {
                    case 1:
                        GetAboutUs(danish1, conn);
                        break;
                    case 2:
                        GetAboutUs(danish2, conn);
                        break;
                    case 3:
                        GetAboutUs(danish3, conn);
                        break;
                    case 4:
                        GetAboutUs(english1, conn);
                        break;
                    case 5:
                        GetAboutUs(english2, conn);
                        break;
                    case 6:
                        GetAboutUs(english3, conn);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                conn.Close();
            }
            return columnValue;
        }
        private string GetAboutUs(string box, SqlConnection conn)
        {
            SqlCommand command = new SqlCommand(box, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    columnValue = reader["aboutUsTranslationText"].ToString();
                }
            }
            return columnValue;
        }
    }
}