namespace AlumniaNotes.models
{
    public class ReporteCalificaciones
    {
        public string AlumnoNombre { get; set; }
        public string AlumnoApellidos { get; set; }
        public string AsignaturaNombre { get; set; }
        public decimal Nota { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
    }
} 