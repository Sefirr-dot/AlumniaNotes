using System.Data.SqlClient;
using AlumniaNotes.models;

namespace AlumniaNotes.data
{
    public class UsuarioRepository : BaseRepository<Usuario>
    {
        public UsuarioRepository(DatabaseContext context) : base(context, "Usuarios")
        {
        }

        protected override Usuario MapReaderToEntity(SqlDataReader reader)
        {
            return new Usuario
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                NombreUsuario = reader.GetString(reader.GetOrdinal("NombreUsuario")),
                Contrasena = reader.GetString(reader.GetOrdinal("Contrasena")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Apellidos = reader.GetString(reader.GetOrdinal("Apellidos")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Rol = reader.GetString(reader.GetOrdinal("Rol"))
            };
        }

        public Usuario GetByNombreUsuario(string nombreUsuario)
        {
            var query = "SELECT * FROM Usuarios WHERE NombreUsuario = @NombreUsuario";
            var parameters = new[] { new SqlParameter("@NombreUsuario", nombreUsuario) };
            
            using (var reader = _context.ExecuteReader(query, parameters))
            {
                if (reader.Read())
                {
                    return MapReaderToEntity(reader);
                }
            }

            return null;
        }

        public bool ValidateCredentials(string nombreUsuario, string contrasena)
        {
            var query = "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @NombreUsuario AND Contrasena = @Contrasena";
            var parameters = new[]
            {
                new SqlParameter("@NombreUsuario", nombreUsuario),
                new SqlParameter("@Contrasena", contrasena)
            };

            var count = Convert.ToInt32(_context.ExecuteScalar(query, parameters));
            return count > 0;
        }

        public void ChangePassword(int usuarioId, string nuevaContrasena)
        {
            var query = "UPDATE Usuarios SET Contrasena = @Contrasena WHERE Id = @Id";
            var parameters = new[]
            {
                new SqlParameter("@Contrasena", nuevaContrasena),
                new SqlParameter("@Id", usuarioId)
            };

            _context.ExecuteNonQuery(query, parameters);
        }
    }
} 