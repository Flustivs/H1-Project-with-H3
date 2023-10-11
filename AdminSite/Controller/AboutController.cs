using AdminSite.DAL;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace AdminSite.Controller
{
    /// <summary>
    /// This class is to read the about us text from the DB and accordingly to what about us text we want,
    /// The first 3 is the about us in English and the last 3 is the about us text but in Danish
    /// </summary>
    public class AboutController
    {
        static string _columnValue;
        private readonly string _connectionString;

        // To get the default connection string that is holded in the appsettings.json file
        public AboutController(IOptions<ConnectionString> connectionString)
        {
            _connectionString = connectionString.Value.DefaultConnection;
        }

        // In this method we open the connection in a try-cath where we also switch on what we hvae gotten in our byte i
        // the byte i is to switch on what we return rather we return the english text for box 2 or the danish text in box 3
        public string Connect(byte i)
        {

            SqlConnection conn = new SqlConnection(_connectionString); // Makes a new connection to Database

            try
            {
                conn.Open(); // if theres a connection it opens it
                // Variables for the about section on the website
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
            return _columnValue;
        }

        // This method is to read in the DB and take out the aboutUsTranslationText
        private string GetAboutUs(string box, SqlConnection conn)
        {
            SqlCommand command = new SqlCommand(box, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    _columnValue = reader["aboutUsTranslationText"].ToString();
                }
            }
            return _columnValue;
        }
    }
}