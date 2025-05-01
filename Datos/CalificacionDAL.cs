csharp
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Modelos;

namespace Datos
{
    public class CalificacionDAL
    {
        private string connectionString;

        public CalificacionDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Calificacion> GetAll()
        {
            List<Calificacion> calificaciones = new List<Calificacion>(); 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, EstudianteId, AsignaturaId, Nota, Fecha FROM Calificaciones";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Calificacion calificacion = new Calificacion
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        EstudianteId = Convert.ToInt32(reader["EstudianteId"]),
                        AsignaturaId = Convert.ToInt32(reader["AsignaturaId"]),
                        Nota = Convert.ToDecimal(reader["Nota"]),
                        Fecha = Convert.ToDateTime(reader["Fecha"])
                    };

                    calificaciones.Add(calificacion);
                }

            }

            return calificaciones;
        }

       public Calificacion GetById(int id)
        {
            Calificacion calificacion = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, EstudianteId, AsignaturaId, Nota, Fecha FROM Calificaciones WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    calificacion = new Calificacion
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        EstudianteId = Convert.ToInt32(reader["EstudianteId"]),
                        AsignaturaId = Convert.ToInt32(reader["AsignaturaId"]),
                        Nota = Convert.ToDecimal(reader["Nota"]),
                        Fecha = Convert.ToDateTime(reader["Fecha"])
                    };
                }
            }
            return calificacion;
        }

        public void Insert(Calificacion calificacion)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Calificaciones (EstudianteId, AsignaturaId, Nota, Fecha) VALUES (@EstudianteId, @AsignaturaId, @Nota, @Fecha)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EstudianteId", calificacion.EstudianteId);
                command.Parameters.AddWithValue("@AsignaturaId", calificacion.AsignaturaId);
                command.Parameters.AddWithValue("@Nota", calificacion.Nota);
                command.Parameters.AddWithValue("@Fecha", calificacion.Fecha);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(Calificacion calificacion)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Calificaciones SET EstudianteId = @EstudianteId, AsignaturaId = @AsignaturaId, Nota = @Nota, Fecha = @Fecha WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", calificacion.Id);
                command.Parameters.AddWithValue("@EstudianteId", calificacion.EstudianteId);
                command.Parameters.AddWithValue("@AsignaturaId", calificacion.AsignaturaId);
                command.Parameters.AddWithValue("@Nota", calificacion.Nota);
                command.Parameters.AddWithValue("@Fecha", calificacion.Fecha);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Calificaciones WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}