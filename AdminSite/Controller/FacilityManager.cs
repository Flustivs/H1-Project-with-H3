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
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
    }
}
