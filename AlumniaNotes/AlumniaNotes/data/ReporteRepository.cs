using System.Data.SqlClient;
using AlumniaNotes.models;

namespace AlumniaNotes.data
{
    public class ReporteRepository
    {
        private readonly DatabaseContext _context;

        public ReporteRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<ReporteCalificaciones> GetReporteCalificaciones(int? alumnoId, int? asignaturaId, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var query = @"
                SELECT 
                    a.Nombre AS AlumnoNombre,
                    a.Apellidos AS AlumnoApellidos,
                    asig.Nombre AS AsignaturaNombre,
                    c.Nota,
                    c.Fecha,
                    c.Tipo
                FROM Calificaciones c
                INNER JOIN Alumnos a ON c.AlumnoId = a.Id
                INNER JOIN Asignaturas asig ON c.AsignaturaId = asig.Id
                WHERE 1=1";

            var parameters = new List<SqlParameter>();

            if (alumnoId.HasValue)
            {
                query += " AND c.AlumnoId = @AlumnoId";
                parameters.Add(new SqlParameter("@AlumnoId", alumnoId.Value));
            }

            if (asignaturaId.HasValue)
            {
                query += " AND c.AsignaturaId = @AsignaturaId";
                parameters.Add(new SqlParameter("@AsignaturaId", asignaturaId.Value));
            }

            if (fechaInicio.HasValue)
            {
                query += " AND c.Fecha >= @FechaInicio";
                parameters.Add(new SqlParameter("@FechaInicio", fechaInicio.Value));
            }

            if (fechaFin.HasValue)
            {
                query += " AND c.Fecha <= @FechaFin";
                parameters.Add(new SqlParameter("@FechaFin", fechaFin.Value));
            }

            var reporte = new List<ReporteCalificaciones>();

            using (var reader = _context.ExecuteReader(query, parameters.ToArray()))
            {
                while (reader.Read())
                {
                    reporte.Add(new ReporteCalificaciones
                    {
                        AlumnoNombre = reader.GetString(reader.GetOrdinal("AlumnoNombre")),
                        AlumnoApellidos = reader.GetString(reader.GetOrdinal("AlumnoApellidos")),
                        AsignaturaNombre = reader.GetString(reader.GetOrdinal("AsignaturaNombre")),
                        Nota = reader.GetDecimal(reader.GetOrdinal("Nota")),
                        Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                        Tipo = reader.GetString(reader.GetOrdinal("Tipo"))
                    });
                }
            }

            return reporte;
        }

        public IEnumerable<ReporteAsistencias> GetReporteAsistencias(int? alumnoId, int? asignaturaId, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var query = @"
                SELECT 
                    a.Nombre AS AlumnoNombre,
                    a.Apellidos AS AlumnoApellidos,
                    asig.Nombre AS AsignaturaNombre,
                    ast.Fecha,
                    ast.Estado,
                    ast.Comentario
                FROM Asistencias ast
                INNER JOIN Alumnos a ON ast.AlumnoId = a.Id
                INNER JOIN Asignaturas asig ON ast.AsignaturaId = asig.Id
                WHERE 1=1";

            var parameters = new List<SqlParameter>();

            if (alumnoId.HasValue)
            {
                query += " AND ast.AlumnoId = @AlumnoId";
                parameters.Add(new SqlParameter("@AlumnoId", alumnoId.Value));
            }

            if (asignaturaId.HasValue)
            {
                query += " AND ast.AsignaturaId = @AsignaturaId";
                parameters.Add(new SqlParameter("@AsignaturaId", asignaturaId.Value));
            }

            if (fechaInicio.HasValue)
            {
                query += " AND ast.Fecha >= @FechaInicio";
                parameters.Add(new SqlParameter("@FechaInicio", fechaInicio.Value));
            }

            if (fechaFin.HasValue)
            {
                query += " AND ast.Fecha <= @FechaFin";
                parameters.Add(new SqlParameter("@FechaFin", fechaFin.Value));
            }

            var reporte = new List<ReporteAsistencias>();

            using (var reader = _context.ExecuteReader(query, parameters.ToArray()))
            {
                while (reader.Read())
                {
                    reporte.Add(new ReporteAsistencias
                    {
                        AlumnoNombre = reader.GetString(reader.GetOrdinal("AlumnoNombre")),
                        AlumnoApellidos = reader.GetString(reader.GetOrdinal("AlumnoApellidos")),
                        AsignaturaNombre = reader.GetString(reader.GetOrdinal("AsignaturaNombre")),
                        Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                        Estado = reader.GetString(reader.GetOrdinal("Estado")),
                        Comentario = reader.IsDBNull(reader.GetOrdinal("Comentario")) ? null : reader.GetString(reader.GetOrdinal("Comentario"))
                    });
                }
            }

            return reporte;
        }

        public IEnumerable<ReporteRendimiento> GetReporteRendimiento(int? alumnoId, int? asignaturaId)
        {
            var query = @"
                SELECT 
                    a.Nombre AS AlumnoNombre,
                    a.Apellidos AS AlumnoApellidos,
                    asig.Nombre AS AsignaturaNombre,
                    AVG(c.Nota) AS PromedioNotas,
                    CAST(COUNT(CASE WHEN ast.Estado = 'Presente' THEN 1 END) AS FLOAT) / 
                    NULLIF(COUNT(*), 0) * 100 AS PorcentajeAsistencia
                FROM Alumnos a
                CROSS JOIN Asignaturas asig
                LEFT JOIN Calificaciones c ON c.AlumnoId = a.Id AND c.AsignaturaId = asig.Id
                LEFT JOIN Asistencias ast ON ast.AlumnoId = a.Id AND ast.AsignaturaId = asig.Id
                WHERE 1=1";

            var parameters = new List<SqlParameter>();

            if (alumnoId.HasValue)
            {
                query += " AND a.Id = @AlumnoId";
                parameters.Add(new SqlParameter("@AlumnoId", alumnoId.Value));
            }

            if (asignaturaId.HasValue)
            {
                query += " AND asig.Id = @AsignaturaId";
                parameters.Add(new SqlParameter("@AsignaturaId", asignaturaId.Value));
            }

            query += " GROUP BY a.Nombre, a.Apellidos, asig.Nombre";

            var reporte = new List<ReporteRendimiento>();

            using (var reader = _context.ExecuteReader(query, parameters.ToArray()))
            {
                while (reader.Read())
                {
                    reporte.Add(new ReporteRendimiento
                    {
                        AlumnoNombre = reader.GetString(reader.GetOrdinal("AlumnoNombre")),
                        AlumnoApellidos = reader.GetString(reader.GetOrdinal("AlumnoApellidos")),
                        AsignaturaNombre = reader.GetString(reader.GetOrdinal("AsignaturaNombre")),
                        PromedioNotas = reader.IsDBNull(reader.GetOrdinal("PromedioNotas")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PromedioNotas")),
                        PorcentajeAsistencia = reader.IsDBNull(reader.GetOrdinal("PorcentajeAsistencia")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PorcentajeAsistencia"))
                    });
                }
            }

            return reporte;
        }
    }
} 