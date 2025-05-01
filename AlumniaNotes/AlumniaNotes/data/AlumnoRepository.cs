using System.Data.SqlClient;
using AlumniaNotes.models;

namespace AlumniaNotes.data
{
    public class AlumnoRepository : BaseRepository<Alumno>
    {
        public AlumnoRepository(DatabaseContext context) : base(context, "Alumnos")
        {
        }

        protected override Alumno MapReaderToEntity(SqlDataReader reader)
        {
            return new Alumno
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Apellidos = reader.GetString(reader.GetOrdinal("Apellidos")),
                DNI = reader.GetString(reader.GetOrdinal("DNI")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                Direccion = reader.IsDBNull(reader.GetOrdinal("Direccion")) ? null : reader.GetString(reader.GetOrdinal("Direccion"))
            };
        }

        public IEnumerable<Alumno> GetByNombre(string nombre)
        {
            var query = "SELECT * FROM Alumnos WHERE Nombre LIKE @Nombre OR Apellidos LIKE @Nombre";
            var parameters = new[] { new SqlParameter("@Nombre", $"%{nombre}%") };
            var alumnos = new List<Alumno>();

            using (var reader = _context.ExecuteReader(query, parameters))
            {
                while (reader.Read())
                {
                    alumnos.Add(MapReaderToEntity(reader));
                }
            }

            return alumnos;
        }
    }
} 