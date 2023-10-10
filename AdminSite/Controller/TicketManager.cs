using AdminSite.DAL;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace AdminSite.Controller
{
	public class TicketManager
	{
		private readonly string _connectionString;
		private List<string> _lastPurchase;
		string _person;

		public TicketManager(IOptions<ConnectionString> connectionString)
		{
			_connectionString = connectionString.Value.DefaultConnection;
		}
		public List<string> LatestPurchases()
		{
			_lastPurchase = new List<string>();
			_lastPurchase.Add(GetPerson(1).ToString());
			_lastPurchase.Add(GetPerson(2).ToString());
			_lastPurchase.Add(GetPerson(3).ToString());
			_lastPurchase.Add(GetPerson(4).ToString());
			_lastPurchase.Add(GetPerson(5).ToString());
			return _lastPurchase;
		}
		private string GetPerson(byte personID)
		{
			SqlConnection conn = new SqlConnection(_connectionString);
			try
			{
				conn.Open();
				string personID1 = @"SELECT personName FROM tblPerson WHERE personID = 1";
				string personID2 = @"SELECT personName FROM tblPerson WHERE personID = 2";
				string personID3 = @"SELECT personName FROM tblPerson WHERE personID = 3";
				string personID4 = @"SELECT personName FROM tblPerson WHERE personID = 4";
				string personID5 = @"SELECT personName FROM tblPerson WHERE personID = 5";

				switch (personID)
				{
					case 1:
						_person = GetPerson(personID1, conn);
						break;
					case 2:
						_person = GetPerson(personID2, conn);
						break;
					case 3:
						_person = GetPerson(personID3, conn);
						break;
					case 4:
						_person = GetPerson(personID4, conn);
						break;
					case 5:
						_person = GetPerson(personID5, conn);
						break;
				}

			}
			catch (Exception e)
			{
				Console.WriteLine("Error: " + e);
			}
			finally
			{
				conn.Close();
			}
			return _person;
		}
		private string GetPerson(string box, SqlConnection conn)
		{
			SqlCommand command = new SqlCommand(box, conn);
			using (SqlDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					_person = reader["personName"].ToString();
				}
			}
			return _person;
		}
	}
}
