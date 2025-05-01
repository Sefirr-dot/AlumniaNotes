using System.Data.SqlClient;
using System.Configuration;

namespace AlumniaNotes.data
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["AlumniaNotesDB"].ConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public void ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        public object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    return command.ExecuteScalar();
                }
            }
        }

        public SqlDataReader ExecuteReader(string query, params SqlParameter[] parameters)
        {
            var connection = GetConnection();
            connection.Open();
            var command = new SqlCommand(query, connection);
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            return command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }
    }
} 