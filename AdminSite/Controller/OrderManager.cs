using AdminSite.DAL;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace AdminSite.Controller
{
    public class OrderManager
    {
        private string _columnValue;
        private readonly string _connectionString;

        public OrderManager(IOptions<ConnectionString> connectionString)
        {
            _connectionString = connectionString.Value.DefaultConnection;
        }
        public string OrderConnect()
        {
            SqlConnection conn = new SqlConnection(_connectionString); // Makes a new connection to Database
            string orderID = "SELECT orderID FROM tblOrder";

            try
            {
                conn.Open();
                GetOrder(orderID, conn);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return _columnValue;
        }
        private string GetOrder(string box, SqlConnection conn)
        {
            SqlCommand command = new SqlCommand(box, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    _columnValue = reader["orderID"].ToString();
                }
            }
            return _columnValue;
        }
    }
}
