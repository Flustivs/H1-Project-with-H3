using AdminSite.DAL;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text;
using System.Data.SqlClient;

namespace AdminSite.Controller
{
    public class LogInManager : PersonManager
    {
        private readonly string _connectionString;

        public LogInManager(IOptions<ConnectionString> connectionString) : base(connectionString)
        {
            _connectionString = connectionString.Value.DefaultConnection;
        }

        /// <summary>
        /// Use input email to RetrievePerson
        /// if exist, find corresponding salt and hash inputPassword 
        /// if hashInputPassword also exist in same row
        /// login success, use PersonID to retrieve RoleId
        /// then direct to different pages
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <param name="inputPassword"></param>
        /// <returns></returns>
        public string HashAndRetrieveInputPassword(string inputEmail, string inputPassword)
        {
            // Check if email exists in DB
            Person personToLogin = RetrievePerson(inputEmail);

            // if email exists:
            if (personToLogin != null)
            {
                byte[] personSalt = Convert.FromBase64String(personToLogin.Salt);
                byte[] inputPasswordBytes = HashPassword(Encoding.UTF8.GetBytes(inputPassword), personSalt);
                //string hashedInputPassword = Encoding.UTF8.GetString(inputPasswordBytes);

                // check if there is a person with both inputEmail and hashedInputPassword
                // DO NOT directly compare hashedInputPassword and saved hashedPassword in db

                string base64Password = Convert.ToBase64String(inputPasswordBytes);

                //bool personAndPasswordExists = _allPersons.Any(p => p.Email == inputEmail && p.PersonPassword == base64Password);


                //if (hashedInputPassword == personToLogin.PersonPassword.ToString())
                if (base64Password == personToLogin.PersonPassword)

                //if (personAndPasswordExists) 
                {
                    // Person and password fits, find the person role
                    return personToLogin.PersonID.ToString();
                }
                else
                    return "Wrong password.";

            }
            else
                return "Email does not exist. Please try again.";
        }

        public int GetRole(int personID)
        {
            int roleID = 0;
            string selectRoleViaPersonID = "SELECT r.roleID FROM tblRole r INNER JOIN tblPersonRole pr ON r.roleID = pr.roleID WHERE pr.personID = @PersonID";
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(selectRoleViaPersonID, conn))
                    {
                        cmd.Parameters.AddWithValue("@PersonID", personID);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                roleID = int.Parse(reader["roleID"].ToString());
                            }
                        }
                    }
                }
                return roleID;
            }
            catch (SqlException ex)
            {
                Debug.WriteLine("SQL Error: " + ex.Message);
                return 0;
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Error: Unable to define a role" + ex.Message);
                return 0;
            }
        }
    }
}
