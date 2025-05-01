using System.Data.SqlClient;
using AlumniaNotes.models;

namespace AlumniaNotes.data
{
    public class AsignaturaRepository : BaseRepository<Asignatura>
    {
        public AsignaturaRepository(DatabaseContext context) : base(context, "Asignaturas")
        {
        }

        protected override Asignatura MapReaderToEntity(SqlDataReader reader)
        {
            return new Asignatura
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Codigo = reader.GetString(reader.GetOrdinal("Codigo")),
                Creditos = reader.GetInt32(reader.GetOrdinal("Creditos")),
                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion"))
            };
        }

        public IEnumerable<Asignatura> GetByNombre(string nombre)
        {
            var query = "SELECT * FROM Asignaturas WHERE Nombre LIKE @Nombre";
            var parameters = new[] { new SqlParameter("@Nombre", $"%{nombre}%") };
            var asignaturas = new List<Asignatura>();

            using (var reader = _context.ExecuteReader(query, parameters))
            {
                while (reader.Read())
                {
                    asignaturas.Add(MapReaderToEntity(reader));
                }
            }

            return asignaturas;
        }
    }
} 