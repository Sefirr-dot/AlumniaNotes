csharp
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Modelos;

namespace Datos
{
    public class AsistenciaDAL
    {
        private string connectionString;

        public AsistenciaDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Asistencia> GetAll()
        {
            List<Asistencia> asistencias = new List<Asistencia>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, EstudianteId, AsignaturaId, Fecha, Presente FROM Asistencias";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Asistencia asistencia = new Asistencia
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        EstudianteId = Convert.ToInt32(reader["EstudianteId"]),
                        AsignaturaId = Convert.ToInt32(reader["AsignaturaId"]),
                        Fecha = Convert.ToDateTime(reader["Fecha"]),
                        Presente = Convert.ToBoolean(reader["Presente"])
                    };
                    asistencias.Add(asistencia);
                }
            }
            return asistencias;
        }

        public Asistencia GetById(int id)
        {
            Asistencia asistencia = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, EstudianteId, AsignaturaId, Fecha, Presente FROM Asistencias WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    asistencia = new Asistencia
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        EstudianteId = Convert.ToInt32(reader["EstudianteId"]),
                        AsignaturaId = Convert.ToInt32(reader["AsignaturaId"]),
                        Fecha = Convert.ToDateTime(reader["Fecha"]),
                        Presente = Convert.ToBoolean(reader["Presente"])
                    };
                }
            }
            return asistencia;
        }

        public void Insert(Asistencia asistencia)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Asistencias (EstudianteId, AsignaturaId, Fecha, Presente) VALUES (@EstudianteId, @AsignaturaId, @Fecha, @Presente)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EstudianteId", asistencia.EstudianteId);
                command.Parameters.AddWithValue("@AsignaturaId", asistencia.AsignaturaId);
                command.Parameters.AddWithValue("@Fecha", asistencia.Fecha);
                command.Parameters.AddWithValue("@Presente", asistencia.Presente);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(Asistencia asistencia)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Asistencias SET EstudianteId = @EstudianteId, AsignaturaId = @AsignaturaId, Fecha = @Fecha, Presente = @Presente WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", asistencia.Id);
                command.Parameters.AddWithValue("@EstudianteId", asistencia.EstudianteId);
                command.Parameters.AddWithValue("@AsignaturaId", asistencia.AsignaturaId);
                command.Parameters.AddWithValue("@Fecha", asistencia.Fecha);
                command.Parameters.AddWithValue("@Presente", asistencia.Presente);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Asistencias WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}