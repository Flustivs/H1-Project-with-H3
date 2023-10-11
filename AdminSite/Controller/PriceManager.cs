using AdminSite.DAL;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace AdminSite.Controller
{
	/// <summary>
	/// This class is to get the price of the different types of tickets,
	/// For example the daypass ticket cost 175 and i get that from the database so if the admins change the price in the admin panel,
	/// these prices updates and is the same value as the new prices.
	/// </summary>
	public class PriceManager
	{
		private string _ticketvalue;
		private readonly string _connectionString;

		public PriceManager(IOptions<ConnectionString> connectionString)
		{
			_connectionString = connectionString.Value.DefaultConnection;
		}
		public string TicketsConnect(byte i)
		{
			SqlConnection conn = new SqlConnection(_connectionString); // Makes a new connection to Database

			try
			{
				conn.Open();
				string DayPass = "SELECT price FROM tblTicketType WHERE ticketTypeID = 1";
				string MonthPass = "SELECT price FROM tblTicketType WHERE ticketTypeID = 2";
				string YearPass = "SELECT price FROM tblTicketType WHERE ticketTypeID = 3";
				switch (i)
				{
					case 1:
						GetTicketPrice(DayPass, conn);
						break;
					case 2:
						GetTicketPrice(MonthPass, conn);
						break;
					case 3:
						GetTicketPrice(YearPass, conn);
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
		// This method reads the price from the tbl tblTicketType and gets the price accordingly to what sql code in a string comes through to the string box
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
		// Updates the prices if they have been changed but only if the prices that have changed but isnt changed to 0 or null
		public void UpdatePrice(int dayPass, int monthPass, int yearPass)
		{
			SqlConnection conn = new SqlConnection(_connectionString);

			string DPass = $"UPDATE tblTicketType SET price = {dayPass} WHERE ticketTypeID = 1";
			string MPass = $"UPDATE tblTicketType SET price = {monthPass} WHERE ticketTypeID = 2";
			string YPass = $"UPDATE tblTicketType SET price = {yearPass} WHERE ticketTypeID = 3";

			try
			{
				conn.Open();
				if (dayPass != 0 && dayPass != null)
				{
					SqlCommand command = new SqlCommand(DPass, conn);
					command.ExecuteNonQuery();
				}
				if (monthPass != 0 && monthPass != null)
				{
					SqlCommand command = new SqlCommand(MPass, conn);
					command.ExecuteNonQuery();
				}
				if (yearPass != 0 && yearPass != null)
				{
					SqlCommand command = new SqlCommand(YPass, conn);
					command.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
                Console.WriteLine("Error: " + ex);
            }
			finally
			{
				conn.Close();
			}

		}
	}
}