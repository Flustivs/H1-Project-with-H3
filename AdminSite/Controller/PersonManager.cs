using AdminSite.DAL;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace AdminSite.Controller
{
    public class PersonManager
    {
        private readonly string _connectionString;
        internal List<Person> _allPersons;

        /// <summary>
        /// In the constructor of PersonManager, the IOptions<ConnectionString> interface is used as a parameter.
        /// ASP.NET Core's dependency injection system automatically provides an instance of ConnectionString
        /// to the constructor when creating an instance of PersonManager. 
        /// This process is known as dependency injection.
        /// </summary>
        /// <param name="connectionString"></param>
        public PersonManager(IOptions<ConnectionString> connectionString)
        {
            _connectionString = connectionString.Value.DefaultConnection;
            /* connectionString is an instance of IOptions<ConnectionString> injected into the constructor. 
            *By accessing connectionString.Value.DefaultConnection, the actual connection string value from the configuration file 
            *(such as appsettings.json in an ASP.NET Core application) is retrieved and stored in the private _connectionString field. 
            *This connection string can then be used to establish a connection to the database.
            */
        }


        public Person RetrievePerson(string inputEmail)
        {
            List<Person> persons = GetAllPersons();
            try
            {
                //Search for the person in the cached list
                return persons.FirstOrDefault(p => p.Email == inputEmail);
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Error: email no found" + ex.Message);
                return new Person();
            }
        }

        public List<Person> GetAllPersons()
        {
            string selectAllRecords = "SELECT * FROM tblPerson";
            List<Person> allPersons = new List<Person>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(selectAllRecords, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Person person = new Person
                                {
                                    PersonID = int.Parse(reader["personID"].ToString()),
                                    Email = reader["email"].ToString(),
                                    PersonName = reader["personName"].ToString(),
                                    //Salt = reader["salt"].ToString(),
                                    //PersonPassword = reader["personPassword"].ToString()
                                };
                                // Convert the Base64-encoded string from the database to a byte array
                                string base64Salt = reader["salt"].ToString();
                                byte[] saltBytes = Convert.FromBase64String(base64Salt);
                                person.Salt = Convert.ToBase64String(saltBytes);
                                // Convert it back to Base64 if necessary
                                string base64Password = reader["personPassword"].ToString();
                                byte[] personPasswordBytes = Convert.FromBase64String(base64Password);
                                person.PersonPassword = Convert.ToBase64String(personPasswordBytes);


                                allPersons.Add(person);
                            }
                        }
                    }
                }
                return allPersons;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                return new List<Person>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return new List<Person>();
            }
        }

        /// <summary>
        /// Insert a new person into DB after generate Salt, hashing salt + password
        /// Check first email is a new one.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="personName"></param>
        /// <param name="password"></param>
        public string AddPerson(string email, string personName, string password)
        {
            // First check if email is existed in db
            List<Person> persons = GetAllPersons();

            if (persons.FirstOrDefault(p => p.Email == email) != null)
            {
                return "email already exists in databese.";
            }

            byte[] saltBytes = GenerateSalt();
            byte[] hashedPasswordBytes = HashPassword(Encoding.UTF8.GetBytes(password), saltBytes);

            string hashedPassword = Convert.ToBase64String(hashedPasswordBytes);
            string salt = Convert.ToBase64String(saltBytes);


            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Insert_tblPerson", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters to the stored procedure
                    cmd.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = email;
                    cmd.Parameters.Add("@personName", SqlDbType.VarChar, 50).Value = personName;
                    cmd.Parameters.Add("@personPassword", SqlDbType.VarChar, 50).Value = hashedPassword;
                    cmd.Parameters.Add("@salt", SqlDbType.VarChar, 50).Value = salt;
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return $"{personName} added to database successfully!";
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

        /// <summary>
        /// To enable partial updates (updating one or two columns) in database, 
        /// modify the EditPerson method to accept nullable parameters for the columns being updated. 
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <param name="newEmail"></param>
        /// <param name="newName"></param>
        /// <param name="newPassword"></param>
        public void EditPerson(string inputEmail, string newEmail = null, string newName = null, string newPassword = null)
        {
            List<Person> persons = GetAllPersons();
            string newSalt = "";
            string newHashedPassword = "";

            if (persons.FirstOrDefault(p => p.Email == inputEmail) != null)
            {
                Person personToEdit = RetrievePerson(inputEmail);

                // Check which columns need to be updated and construct the update query accordingly
                string editPersonCommand = "UPDATE tblPerson SET";
                bool needComma = false;

                if(!string.IsNullOrEmpty(newEmail))
                {
                    editPersonCommand += "email = @newEmail";
                    needComma = true;
                }

                if(!string.IsNullOrEmpty (newName))
                {
                    if(needComma)
                    {
                        editPersonCommand += ",";
                    }
                    editPersonCommand += "personName = @newName";
                    needComma = true;
                }

                if (!string.IsNullOrEmpty(newPassword))
                {
                    if (needComma)
                    {
                        editPersonCommand += ",";
                    }
                    editPersonCommand += "personPassword = @newHashedPassword, salt = @newSalt";
                    needComma = true;
                }

                editPersonCommand += "WHERE personID = @PersonID";

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(editPersonCommand, conn))
                    {
                        cmd.Parameters.AddWithValue("@personID", personToEdit.PersonID);

                        if (!string.IsNullOrEmpty(newEmail))
                        {
                            cmd.Parameters.AddWithValue("@newEmail", newEmail);
                        }

                        if (!string.IsNullOrEmpty(newName))
                        {
                            cmd.Parameters.AddWithValue("@newName", newName);
                        }

                        if (!string.IsNullOrEmpty(newPassword))
                        {
                            // Generate new salt, then get newHashedPassword
                            byte[] saltBytes = GenerateSalt();
                            byte[] hashedPasswordBytes = HashPassword(Encoding.UTF8.GetBytes(newPassword), saltBytes);

                            newHashedPassword = Convert.ToBase64String(hashedPasswordBytes);
                            newSalt = Convert.ToBase64String(saltBytes);

                            cmd.Parameters.AddWithValue("@newHashedPassword", newHashedPassword);
                            cmd.Parameters.AddWithValue("@newSalt", newSalt);
                        }

                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            Debug.WriteLine("Person updated successfully!");
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
                Debug.WriteLine("Email does not exist in databese.");
            }
        }

        /// <summary>
        /// To delete rows from multiple tables in a database transaction, use a transaction in SQL Server.
        /// A transaction ensures that a series of SQL operations are treated as a single unit of work. 
        /// If any operation within the transaction fails, 
        /// the entire transaction can be rolled back, ensuring data consistency.
        /// </summary>
        /// <param name="inputEmail"></param>
        public void DeletePerson(string inputEmail)
        {
            List<Person> persons = GetAllPersons();

            if (persons.FirstOrDefault(p => p.Email == inputEmail) != null)
            {
                // Find the person to delete
                Person personToDelete = RetrievePerson(inputEmail);

                string deletePersonRoleCommand = "DELETE FROM tblPersonRole WHERE personID = @PersonID";
                string deletePersonFacilityCommand = "DELETE FROM tblPersonFacility WHERE personID = @PersonID";
                string deletePersonCommand = "DELETE FROM tblPerson WHERE personID = @PersonID";

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        // Delete PersonRole associated with the PersonID to be deleted
                        using (SqlCommand cmd = new SqlCommand(deletePersonRoleCommand, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@PersonID", personToDelete.PersonID);
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

                        // also delete PersonFacility associated to the person to be deleted
                        using (SqlCommand cmd = new SqlCommand(deletePersonFacilityCommand, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@PersonID", personToDelete.PersonID);
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

                        // Delete Person 
                        using (SqlCommand cmd = new SqlCommand(deletePersonCommand, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@PersonID", personToDelete.PersonID);
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
                    }
                }
            }
            else
            {
                Debug.WriteLine("Email does not exist in databese.");
            }
        }

        public byte[] HashPassword(byte[] password, byte[] salt)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, 600000, HashAlgorithmName.SHA256))
            {
                return rfc2898.GetBytes(32);
            };
        }

        public byte[] GenerateSalt()
        {
            var randomNumberGenerator = RandomNumberGenerator.Create();
            byte[] randomBytes = new byte[32];
            randomNumberGenerator.GetBytes(randomBytes);

            return randomBytes;
        }
    }
}
