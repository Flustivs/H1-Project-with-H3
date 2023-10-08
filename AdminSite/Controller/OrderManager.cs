using Microsoft.Data.SqlClient;

namespace AdminSite.Controller
{
    public class OrderManager
    {
        private string _columnValue;
        public string OrderConnect()
        {
            //string datasource = @"LAPTOP-94N0K9HA\MSSQLSERVER01"; // Test on local pc
            string connString = @"Data Source=(localdb)\MSSqlLocalDb;Initial Catalog=SuperFunFunParkDB;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection conn = new SqlConnection(connString); // Makes a new connection to Database
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
