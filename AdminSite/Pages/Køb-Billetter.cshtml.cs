using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace AdminSite.Pages
{
    public class Køb_BilletterModel : PageModel
    {
        public void OnGet()
        {
            //string datasource = @"LAPTOP-94N0K9HA\MSSQLSERVER01"; // Test on local pc
            string connString = @"Data Source=LAPTOP-94N0K9HA\MSSQLSERVER01;Initial Catalog=SuperFunFunParkDB;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection conn = new SqlConnection(connString); // Makes a new connection to Database

            try
            {
                conn.Open(); // if theres a connection it opens it

            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
        }
    }
}
