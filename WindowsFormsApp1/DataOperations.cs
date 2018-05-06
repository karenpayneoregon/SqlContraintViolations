using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class DataOperations : BaseSqlServerConnections
    {
        /// <summary>
        /// Column(s) violating constraint
        /// </summary>
        public string ConstraintColumns = null;
        /// <summary>
        /// Column values violating a constraint
        /// </summary>
        public string ConstraintValue = null;
        public string DuplicateCountryValue = null;

        public DataOperations()
        {
            DefaultCatalog = "ForumExample";
        }
        /// <summary>
        /// Read all records from Persons1 table
        /// </summary>
        /// <returns></returns>
        public DataTable Read()
        {
            DataTable dt = new DataTable();

            var selectStatement = "SELECT id,FirstName,LastName FROM dbo.Persons1 ORDER BY LastName";

            using (var cn = new SqlConnection() { ConnectionString = ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn, CommandText = selectStatement })
                {

                    cn.Open();
                    dt.Load(cmd.ExecuteReader());

                }
            }

            return dt;

        }
        /// <summary>
        /// Read all countries
        /// </summary>
        /// <returns></returns>
        public DataTable ReadCountries()
        {
            DataTable dt = new DataTable();

            var selectStatement = "SELECT id,Name FROM dbo.Country";

            using (var cn = new SqlConnection() { ConnectionString = ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn, CommandText = selectStatement })
                {

                    cn.Open();
                    dt.Load(cmd.ExecuteReader());

                }
            }

            return dt;
        }

        /// <summary>
        /// Update a person without causing an exception againsts a constraint but
        /// will increment the primary key without adding a new record
        /// </summary>
        /// <param name="pFirstName">First name to update</param>
        /// <param name="pLastName">Last name to update</param>        
        /// <param name="pIdentifier">Identifying key for person</param>
        /// <returns></returns>
        public bool PersonUpdate(string pFirstName, string pLastName, int pIdentifier)
        {

            using (var cn = new SqlConnection() { ConnectionString = ConnectionString })
            {

                var statement = "UPDATE dbo.Persons1 SET FirstName = @FirstName,LastName = @LastName  WHERE id = @Id";

                using (var cmd = new SqlCommand() { Connection = cn, CommandText = statement })
                {

                    cmd.Parameters.AddWithValue("@FirstName", pFirstName);
                    cmd.Parameters.AddWithValue("@LastName", pLastName);
                    cmd.Parameters.AddWithValue("@id", pIdentifier);

                    try
                    {

                        cn.Open();
                        cmd.ExecuteNonQuery();

                        return true;

                    }
                    catch (SqlException ex)
                    {

                        string message = null;
                        int pos = 0;

                        //
                        // Proposed values for update causing the exception
                        //
                        ConstraintValue = Regex.Match(ex.Message, "\\(([^)]*)\\)").Groups[1].Value;

                        pos = ex.Message.IndexOf(".", StringComparison.Ordinal);
                        message = ex.Message.Substring(0, pos);


                        if (ex.Number == 2601)
                        {
                            ConstraintColumns = GetIndexKeys(cmd, ex.Message,"Persons1");
                        }

                        mHasException = true;
                        mLastException = ex;

                        return false;

                    }
                    catch (Exception ex)
                    {

                        mHasException = true;
                        mLastException = ex;

                        return false;
                    }
                }
            }
        }
        /// <summary>
        /// Update person the right way by first determing if FirstName and LastName
        /// will not produce a duplicate record or increment the next primary key sequence.
        /// </summary>
        /// <param name="pFirstName">First name to update</param>
        /// <param name="pLastName">Last name to update</param>        
        /// <param name="pIdentifier">Identifying key for person</param>
        /// <returns>Success</returns>
        public bool PersonUpdate1(string pFirstName, string pLastName, int pIdentifier)
        {

            using (var cn = new SqlConnection() { ConnectionString = ConnectionString })
            {
                // see note 1 in information.txt
                var statement = "SELECT 1 FROM dbo.Persons1 AS p WHERE p.FirstName = @FirstName AND p.LastName = @LastName ";

                using (var cmd = new SqlCommand() { Connection = cn, CommandText = statement })
                {

                    cmd.Parameters.AddWithValue("@FirstName", pFirstName);
                    cmd.Parameters.AddWithValue("@LastName", pLastName);


                    try
                    {

                        cn.Open();

                        if (cmd.ExecuteScalar() == null) 
                        {
                            cmd.Parameters.AddWithValue("@id", pIdentifier);
                            cmd.ExecuteNonQuery();
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                        

                    }
                    catch (SqlException ex)
                    {

                        //
                        // Proposed values for update causing the exception
                        //
                        ConstraintValue = Regex.Match(ex.Message, "\\(([^)]*)\\)").Groups[1].Value;

                        /*
                            * See note 2 in Information.txt
                            */
                        if (ex.Number == 2601)
                        {
                            ConstraintColumns = GetIndexKeys(cmd, ex.Message,"Persons1");
                        }

                        mHasException = true;
                        mLastException = ex;

                        return false;

                    }
                    catch (Exception ex)
                    {

                        mHasException = true;
                        mLastException = ex;

                        return false;
                    }
                }
            }
        }
        public string ConstraintColumnName { get; set; }
        /// <summary>
        /// This method shows how to assert against adding a duplicate country. 
        /// There are several parts of this method that are overkill e.g. using
        /// regx to get the value to be inserted, the table name and the index
        /// which was violated for educational purposes if you were to get into
        /// this topic in a generic manner
        /// </summary>
        /// <param name="pCountryName"></param>
        /// <param name="pIdentifier"></param>
        /// <param name="pError"></param>
        /// <returns></returns>
        public bool InsertCountry(string pCountryName, ref int pIdentifier, ref string pError)
        {
            using (var cn = new SqlConnection() {ConnectionString = ConnectionString})
            {
                using (var cmd = new SqlCommand() {Connection = cn})
                {
                    var insertStatement = "INSERT INTO dbo.Country (Name)  VALUES (@Name);" +
                                          "SELECT CAST(scope_identity() AS int);";

                    try
                    {
                        cmd.CommandText = insertStatement;
                        cmd.Parameters.AddWithValue("@Name", pCountryName);

                        cn.Open();

                        pIdentifier = Convert.ToInt32(cmd.ExecuteScalar());
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        string message = null;
                        string tableName = "";
                        string indexName = "";

                        /*
                         * We already know the value but if you want to get
                         * into some regx this shows how to parse the value.
                         */
                        DuplicateCountryValue = Regex.Match(ex.Message, "\\(([^)]*)\\)").Groups[1].Value;

                        /*
                         * Get the table name 'country' which we have in the INSERT INTO
                         */
                        var match = Regex.Match(ex.Message, @"'([^']*)");
                        if (match.Success)
                        {
                            tableName = match.Groups[1].Value;
                        }


                        if (ex.Number == 2601)
                        {

                            pError = $"Can not add '{DuplicateCountryValue}' into '{tableName}' since it already exists.";
                            // if you needed the index involved with the error
                            indexName = GetIndexKeys(cmd, ex.Message, "Country");
                        }

                        mHasException = true;
                        mLastException = ex;

                        return false;
                    }
                }
            }
        }
        /// <summary>
        /// Insert new record the right way by first determing if the country name
        /// is present in the database.
        /// </summary>
        /// <param name="pCountryName">Country name to insert</param>
        /// <param name="pIdentifier">New primary key</param>
        /// <param name="pError">Error message on failure</param>
        /// <returns>Success</returns>
        public bool InsertCountry1(string pCountryName, ref int pIdentifier, ref string pError)
        {
            using (var cn = new SqlConnection() { ConnectionString = ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn })
                {
                    var selectStatement = "SELECT 1 FROM dbo.Country WHERE Name = @Name";

                    var insertStatement = "INSERT INTO dbo.Country (Name)  VALUES (@Name);" +
                                            "SELECT CAST(scope_identity() AS int);";

                    try
                    {
                        cmd.CommandText = selectStatement;
                        cmd.Parameters.AddWithValue("@Name", pCountryName);
                        cn.Open();

                        if (cmd.ExecuteScalar() != null)
                        {
                            pError = $"Country '{pCountryName}' already in table";
                            mHasException = false;
                            return false;
                        }

                        cmd.CommandText = insertStatement;


                        pIdentifier = Convert.ToInt32(cmd.ExecuteScalar());
                        return true;
                    }
                    catch (Exception ex)
                    {
                        mHasException = true;
                        mLastException = ex;

                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Get indexes for table Person1 along with their
        /// index keys
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public string GetIndexKeys(SqlCommand cmd, string message, string tableName)
        {

            var indexName = "Unknown";

            cmd.CommandText = $"EXEC sp_helpindex N'dbo.{tableName}'";
            try
            {

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (message.Contains(reader.GetString(0)))
                        {
                            indexName = reader.GetString(2);
                        }
                    }
                }

            }
            catch (Exception)
            {
                //
                // Should not land here but must be careful as 
                // this method is called from a catch block.
                // Now if you do land here the table name does
                // not exists or you spelled the name wrong.
                //
            }

            return indexName;

        }
    }
}
