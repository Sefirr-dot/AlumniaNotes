using System.Data.SqlClient;
using AlumniaNotes.models;

namespace AlumniaNotes.data
{
    public class AsistenciaRepository : BaseRepository<Asistencia>
    {
        public AsistenciaRepository(DatabaseContext context) : base(context, "Asistencias")
        {
        }

        protected override Asistencia MapReaderToEntity(SqlDataReader reader)
        {
            return new Asistencia
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                AlumnoId = reader.GetInt32(reader.GetOrdinal("AlumnoId")),
                AsignaturaId = reader.GetInt32(reader.GetOrdinal("AsignaturaId")),
                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                Estado = reader.GetString(reader.GetOrdinal("Estado")),
                Comentario = reader.IsDBNull(reader.GetOrdinal("Comentario")) ? null : reader.GetString(reader.GetOrdinal("Comentario"))
            };
        }
    }
} 