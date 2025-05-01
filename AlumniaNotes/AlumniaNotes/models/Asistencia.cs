using System;

namespace AlumniaNotes.models
{
    public class Asistencia
    {
        public int Id { get; set; }
        public int AlumnoId { get; set; }
        public int AsignaturaId { get; set; }
        public DateTime Fecha { get; set; }
        public EstadoAsistencia Estado { get; set; }
        public string? Comentario { get; set; }
        public Alumno? Alumno { get; set; }
        public Asignatura? Asignatura { get; set; }
    }

    public enum EstadoAsistencia
    {
        Presente,
        Ausente,
        Justificado,
        Tardanza
    }
} 