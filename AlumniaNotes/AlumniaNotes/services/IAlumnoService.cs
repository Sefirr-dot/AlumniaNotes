using System.Collections.Generic;
using System.Threading.Tasks;
using AlumniaNotes.models;

namespace AlumniaNotes.services
{
    public interface IAlumnoService
    {
        Task<IEnumerable<Alumno>> GetAllAlumnosAsync();
        Task<Alumno> GetAlumnoByIdAsync(int id);
        Task<Alumno> CreateAlumnoAsync(Alumno alumno);
        Task<Alumno> UpdateAlumnoAsync(Alumno alumno);
        Task DeleteAlumnoAsync(int id);
    }
} 