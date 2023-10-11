using AdminSite.DAL;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;


namespace AdminSite.Controller
{
    public class FacilityManager : LogInManager
    {
        private readonly string _connectionString;
        public FacilityManager(IOptions<ConnectionString> connectionString) : base(connectionString)
        {
            _connectionString = connectionString.Value.DefaultConnection;
        }

        private List<string> GetMaintenanceStaff(int facilityID)
        {
            List<Person> maintenanceStaff = new List<Person>();

            string selectPersonViaFacilityID = "SELECT p.* FROM tblPerson p INNER JOIN tblPersonFacility pf ON p.personID = pf.personID WHERE pf.facilityID = @FacilityID";
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(selectPersonViaFacilityID, conn))
                    {
                        cmd.Parameters.AddWithValue("@FacilityID", facilityID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Person person = new Person
                                {
                                    PersonID = int.Parse(reader["personID"].ToString()),
                                    Email = reader["email"].ToString(),
                                    PersonName = reader["personName"].ToString()
                                };
                                maintenanceStaff.Add(person);
                            }
                        }
                    }
                }
                List<string> maintenanceStaffNames = maintenanceStaff.Select(p => p.PersonName).ToList();

                return maintenanceStaffNames;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                return new List<string>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return new List<string>();
            }

        }

        /// <summary>
        /// Get list of objects Facility.
        /// Then get a list of MaintenanceStaff for each facility.
        /// </summary>
        /// <returns></returns>
        public List<Facility> GetAllFacilities()
        {
            string selectAllFacilities = "SELECT * FROM tblFacility";
            List<Facility> allFacilities = new List<Facility>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(selectAllFacilities, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Facility facility = new Facility()
                                {
                                    FacilityID = int.Parse(reader["facilityID"].ToString()),
                                    FacilityName = reader["facilityName"].ToString(),
                                    FacilityPicURL = reader["facilityPicURL"].ToString(),
                                    NextMaintenanceDate = (DateTime)reader["nextMaintenanceDate"]
                                };
                                facility.MaintenanceStaff = GetMaintenanceStaff(facility.FacilityID);
                                allFacilities.Add(facility);
                            }
                        }
                    }
                }
                return allFacilities;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                return new List<Facility>(); 
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return new List<Facility>();
            }
        }

         public void EditFacility(int inputID, string newName = null, string newPicURL = null, DateTime? newDate = null)
         {
		List<Facility> facilities = GetAllFacilities();

         // use the selected FacilityID to retrieve the specific facility
		if (facilities.FirstOrDefault(f => f.FacilityID == inputID) != null)
		{
			// Check which columns need to be updated and construct the update query accordingly
			string editFacilityCommand = "UPDATE tblFacility SET ";
			bool needComma = false;

			if (!string.IsNullOrEmpty(newName))
			{
				editFacilityCommand += " facilityName = @newName ";
				needComma = true;
			}

			if (!string.IsNullOrEmpty(newPicURL))
			{
				if (needComma)
				{
					editFacilityCommand += ",";
				}
				editFacilityCommand += " facilityPicURL = @newPicURL ";
				needComma = true;
			}

			if (newDate.HasValue)
			{
				if (needComma)
				{
					editFacilityCommand += ",";
				}
				editFacilityCommand += " nextMaintenanceDate = @newDate ";
				needComma = true;
			}

         editFacilityCommand += "WHERE facilityID = @inputID";

			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				using (SqlCommand cmd = new SqlCommand(editFacilityCommand, conn))
				{
					cmd.Parameters.AddWithValue("@inputID", inputID);

					if (!string.IsNullOrEmpty(newName))
					{
						cmd.Parameters.AddWithValue("@newName", newName);
					}

					if (!string.IsNullOrEmpty(newPicURL))
					{
						cmd.Parameters.AddWithValue("@newPicURL", newPicURL);
					}

					if (newDate.HasValue)
					{
						cmd.Parameters.AddWithValue("@newDate ", newDate);
					}

					try
					{
						conn.Open();
						cmd.ExecuteNonQuery();
						Debug.WriteLine("Facility updated successfully!");
					}
					catch (SqlException ex)
					{
						Debug.WriteLine("SQL Error: " + ex.Message);
					}
					catch (Exception ex)
					{
						Debug.WriteLine("Error: " + ex.Message);
					}
				}
			}
		}
		else
		{
			Debug.WriteLine("Facility does not exist in databese.");
		}
	}
 /// <summary>
 /// Delete facility with a specific facilityID from tblPersonFacility and tblFacility
 /// </summary>
 /// <param name="facilityID"></param>
 public void DeleteFacility(int inputID)
 {
		List<Facility> facilities = GetAllFacilities();

		if (facilities.FirstOrDefault(f => f.FacilityID == inputID) != null)
		{
			
			string deletePersonFacilityCommand = "DELETE FROM tblPersonFacility WHERE facilityID = @FacilityID";
			string deleteFacilityCommand = "DELETE FROM tblFacility WHERE facilityID = @FacilityID";

			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				conn.Open();

				using (SqlTransaction transaction = conn.BeginTransaction())
				{
					// Delete PersonFacility associated with the FacilityID to be deleted
					using (SqlCommand cmd = new SqlCommand(deletePersonFacilityCommand, conn, transaction))
					{
						cmd.Parameters.AddWithValue("@FacilityID",inputID);
						try
						{
							cmd.ExecuteNonQuery();
						}
						catch (SqlException ex)
						{
							Debug.WriteLine("SQL Error: " + ex.Message);
						}
						catch (Exception ex)
						{
							Debug.WriteLine("Error: " + ex.Message);
						}
					}

					// Delete Facility
					using (SqlCommand cmd = new SqlCommand(deleteFacilityCommand, conn, transaction))
					{
						cmd.Parameters.AddWithValue("@FacilityID", inputID);
						try
						{
							cmd.ExecuteNonQuery();
						}
						catch (SqlException ex)
						{
							Debug.WriteLine("SQL Error: " + ex.Message);
						}
						catch (Exception ex)
						{
							Debug.WriteLine("Error: " + ex.Message);
						}
					}

					// Commit the transaction if all operations succeed
					transaction.Commit();

				}
			}
		}
		else
		{
			Debug.WriteLine("Email does not exist in databese.");
		}
	}

  public string AddFacility(string facilityName, string facilityPicURL, DateTime nextMaintenanceDate)
{
    List<Facility> facilities = GetAllFacilities();

    // Check if the facility name already exists in db
    if(facilities.FirstOrDefault(f => f.FacilityName == facilityName) != null)
    {
        return "Facility already exists in databese.";
    }

    // Add new Facility
	using (SqlConnection conn = new SqlConnection(_connectionString))
	{
		using (SqlCommand cmd = new SqlCommand("Insert_tblFacility", conn))
		{
			cmd.CommandType = CommandType.StoredProcedure;

			// Add parameters to the stored procedure
			cmd.Parameters.Add("@facilityName", SqlDbType.VarChar, 50).Value = facilityName;
			cmd.Parameters.Add("@facilityPicURL", SqlDbType.VarChar, 50).Value = facilityPicURL;
			cmd.Parameters.Add("@nextMaintenanceDate", SqlDbType.Date).Value = nextMaintenanceDate;

			try
			{
				conn.Open();
				cmd.ExecuteNonQuery();
				return $"{facilityName} added to database successfully!";
			}
			catch (SqlException ex)
			{
				return "SQL Error: " + ex.Message;
			}
			catch (Exception ex)
			{
				return "Error: " + ex.Message;
			}
		}
	}
}

public void SetMaintenanceStaff()
{

}
        
    }
}
