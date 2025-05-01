csharp
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Modelos;

namespace Datos
{
    public class AsignaturaDAL
    {
        private readonly string _connectionString;

        public AsignaturaDAL(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Asignatura> GetAll()
        {
            List<Asignatura> asignaturas = new List<Asignatura>();
            string query = "SELECT Id, Nombre, Descripcion FROM Asignaturas";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Asignatura asignatura = new Asignatura
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Descripcion = reader.IsDBNull(2) ? null : reader.GetString(2)
                    };
                    asignaturas.Add(asignatura);
                }
                reader.Close();
            }
            return asignaturas;
        }

        public Asignatura GetById(int id)
        {
            Asignatura asignatura = null;
            string query = "SELECT Id, Nombre, Descripcion FROM Asignaturas WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    asignatura = new Asignatura
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Descripcion = reader.IsDBNull(2) ? null : reader.GetString(2)
                    };
                }
                reader.Close();
            }
            return asignatura;
        }

        public void Insert(Asignatura asignatura)
        {
            string query = "INSERT INTO Asignaturas (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nombre", asignatura.Nombre);
                command.Parameters.AddWithValue("@Descripcion", asignatura.Descripcion ?? (object)DBNull.Value);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(Asignatura asignatura)
        {
            string query = "UPDATE Asignaturas SET Nombre = @Nombre, Descripcion = @Descripcion WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", asignatura.Id);
                command.Parameters.AddWithValue("@Nombre", asignatura.Nombre);
                command.Parameters.AddWithValue("@Descripcion", asignatura.Descripcion ?? (object)DBNull.Value);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM Asignaturas WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}