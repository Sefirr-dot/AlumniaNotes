using System;

namespace AlumniaNotes.models
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string NombreUsuario { get; set; }
        public required string Contrasena { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? Rol { get; set; }
        public DateTime UltimoAcceso { get; set; }
        public bool Activo { get; set; }
    }
} 