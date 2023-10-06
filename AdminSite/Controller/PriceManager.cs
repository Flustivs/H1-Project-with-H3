using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AdminSite.Controller
{
    public class PriceManager
    {
        private string _ticketvalue;
        public string TicketsConnect(byte i)
        {
            //string datasource = @"LAPTOP-94N0K9HA\MSSQLSERVER01"; // Test on local pc
            string connString = @"Data Source=(localdb)\MSSqlLocalDb;Initial Catalog=SuperFunFunParkDB;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection conn = new SqlConnection(connString); // Makes a new connection to Database

            try
            {
                conn.Open(); // if theres a connection it opens it
                string Daypass = "SELECT price FROM tblTicketType WHERE ticketTypeID = 1";
                string MonthlyPass = "SELECT price FROM tblTicketType WHERE ticketTypeID = 2";
                string YearlyPass = "SELECT price FROM tblTicketType WHERE ticketTypeID = 3";
                switch (i)
                {
                    case 1:
                        GetTicketPrice(Daypass, conn);
                        break;
                    case 2:
                        GetTicketPrice(MonthlyPass, conn);
                        break;
                    case 3:
                        GetTicketPrice(YearlyPass, conn);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return _ticketvalue;
        }
        private string GetTicketPrice(string box, SqlConnection conn)
        {
            SqlCommand command = new SqlCommand(box, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    _ticketvalue = reader["price"].ToString();
                }
            }
            return _ticketvalue;
        }
    }
}