csharp
--- a/Datos/EstudianteDAL.cs
+++ b/Datos/EstudianteDAL.cs
@@
using Modelos;
using System;
using System.Collections.Generic;
 using System.Data.SqlClient;

 namespace Datos
 {
     public class EstudianteDAL
@@
                 SqlDataReader reader = command.ExecuteReader();
                 if (reader.Read())
                 {
                     return new Estudiante
                     {
@@
                 }
             }
             return null;
@@
         {
             using (SqlConnection connection = new SqlConnection(connectionString))
             {
                 string query = "INSERT INTO Estudiantes (Nombre, Apellido, FechaNacimiento, Email, Telefono) VALUES (@Nombre, @Apellido, @FechaNacimiento, @Email, @Telefono); SELECT SCOPE_IDENTITY();";
@@
             }
         }
     }
 }

        public void Insert(Estudiante estudiante)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Estudiantes (Nombre, Apellido, FechaNacimiento, Email, Telefono) VALUES (@Nombre, @Apellido, @FechaNacimiento, @Email, @Telefono); SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nombre", estudiante.Nombre);
                command.Parameters.AddWithValue("@Apellido", estudiante.Apellido);
                command.Parameters.AddWithValue("@FechaNacimiento", estudiante.FechaNacimiento);
                command.Parameters.AddWithValue("@Email", estudiante.Email);
                command.Parameters.AddWithValue("@Telefono", estudiante.Telefono ?? (object)DBNull.Value);
                connection.Open();
                estudiante.Id = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void Update(Estudiante estudiante)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Estudiantes SET Nombre = @Nombre, Apellido = @Apellido, FechaNacimiento = @FechaNacimiento, Email = @Email, Telefono = @Telefono WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", estudiante.Id);
                command.Parameters.AddWithValue("@Nombre", estudiante.Nombre);
                command.Parameters.AddWithValue("@Apellido", estudiante.Apellido);
                command.Parameters.AddWithValue("@FechaNacimiento", estudiante.FechaNacimiento);
                command.Parameters.AddWithValue("@Email", estudiante.Email);
                command.Parameters.AddWithValue("@Telefono", estudiante.Telefono ?? (object)DBNull.Value);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Estudiantes WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}