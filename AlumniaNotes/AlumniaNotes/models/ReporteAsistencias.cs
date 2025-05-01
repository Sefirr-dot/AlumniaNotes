namespace AlumniaNotes.models
{
    public class ReporteAsistencias
    {
        public string AlumnoNombre { get; set; }
        public string AlumnoApellidos { get; set; }
        public string AsignaturaNombre { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public string Comentario { get; set; }
    }
} 