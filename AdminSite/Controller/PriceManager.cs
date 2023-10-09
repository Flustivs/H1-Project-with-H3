using AdminSite.DAL;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AdminSite.Controller
{
    public class PriceManager
    {
        private string _ticketvalue;
        private readonly string _connectionString;
        
        public PriceManager()
        {

        }

        public PriceManager(IOptions<ConnectionString> connectionString)
        {
            _connectionString = connectionString.Value.DefaultConnection;
        }
        public string TicketsConnect(byte i)
        {
            SqlConnection conn = new SqlConnection(_connectionString); // Makes a new connection to Database

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