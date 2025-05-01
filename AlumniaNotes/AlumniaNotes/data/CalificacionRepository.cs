using System.Data.SqlClient;
using AlumniaNotes.models;

namespace AlumniaNotes.data
{
    public class CalificacionRepository : BaseRepository<Calificacion>
    {
        public CalificacionRepository(DatabaseContext context) : base(context, "Calificaciones")
        {
        }

        protected override Calificacion MapReaderToEntity(SqlDataReader reader)
        {
            return new Calificacion
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                AlumnoId = reader.GetInt32(reader.GetOrdinal("AlumnoId")),
                AsignaturaId = reader.GetInt32(reader.GetOrdinal("AsignaturaId")),
                Nota = reader.GetDecimal(reader.GetOrdinal("Nota")),
                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                Tipo = reader.GetString(reader.GetOrdinal("Tipo")),
                Comentario = reader.IsDBNull(reader.GetOrdinal("Comentario")) ? null : reader.GetString(reader.GetOrdinal("Comentario"))
            };
        }

        public IEnumerable<Calificacion> GetByAlumno(int alumnoId)
        {
            var query = "SELECT * FROM Calificaciones WHERE AlumnoId = @AlumnoId";
            var parameters = new[] { new SqlParameter("@AlumnoId", alumnoId) };
            var calificaciones = new List<Calificacion>();

            using (var reader = _context.ExecuteReader(query, parameters))
            {
                while (reader.Read())
                {
                    calificaciones.Add(MapReaderToEntity(reader));
                }
            }

            return calificaciones;
        }

        public IEnumerable<Calificacion> GetByAsignatura(int asignaturaId)
        {
            var query = "SELECT * FROM Calificaciones WHERE AsignaturaId = @AsignaturaId";
            var parameters = new[] { new SqlParameter("@AsignaturaId", asignaturaId) };
            var calificaciones = new List<Calificacion>();

            using (var reader = _context.ExecuteReader(query, parameters))
            {
                while (reader.Read())
                {
                    calificaciones.Add(MapReaderToEntity(reader));
                }
            }

            return calificaciones;
        }

        public decimal GetPromedioByAlumno(int alumnoId)
        {
            var query = "SELECT AVG(Nota) FROM Calificaciones WHERE AlumnoId = @AlumnoId";
            var parameters = new[] { new SqlParameter("@AlumnoId", alumnoId) };
            
            var result = _context.ExecuteScalar(query, parameters);
            return result == DBNull.Value ? 0 : Convert.ToDecimal(result);
        }
    }
} 