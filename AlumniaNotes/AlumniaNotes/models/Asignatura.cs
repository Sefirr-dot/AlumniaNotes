using System;
using System.Collections.Generic;

namespace AlumniaNotes.models
{
    public class Asignatura
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int HorasSemanales { get; set; }
        public List<Alumno> Alumnos { get; set; }
        public List<Calificacion> Calificaciones { get; set; }

        public Asignatura()
        {
            Alumnos = new List<Alumno>();
            Calificaciones = new List<Calificacion>();
        }
    }
} 