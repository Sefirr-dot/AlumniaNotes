using System;

namespace AlumniaNotes.models
{
    public class Calificacion
    {
        public int Id { get; set; }
        public int AlumnoId { get; set; }
        public int AsignaturaId { get; set; }
        public decimal Nota { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoEvaluacion { get; set; }
        public string Comentarios { get; set; }
    }
} 