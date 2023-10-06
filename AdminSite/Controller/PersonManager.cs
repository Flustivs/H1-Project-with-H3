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

        public PersonManager(IOptions<ConnectionString> connectionString)
        {
            _connectionString = connectionString.Value.DefaultConnection;
            //_allPersons =  GetAllPersons();
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
                return null;
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
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public void AddPerson(string email, string personName, string password)
        {
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
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("SQL Error: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error: " + ex.Message);
                    }
                }
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
