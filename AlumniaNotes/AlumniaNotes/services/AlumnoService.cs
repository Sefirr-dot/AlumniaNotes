using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlumniaNotes.models;

namespace AlumniaNotes.services
{
    public class AlumnoService : IAlumnoService
    {
        private readonly List<Alumno> _alumnos = new List<Alumno>();
        private int _nextId = 1;

        public async Task<IEnumerable<Alumno>> GetAllAlumnosAsync()
        {
            await Task.Delay(100); // Simulate network delay
            return _alumnos;
        }

        public async Task<Alumno> GetAlumnoByIdAsync(int id)
        {
            await Task.Delay(100); // Simulate network delay
            return _alumnos.Find(a => a.Id == id);
        }

        public async Task<Alumno> CreateAlumnoAsync(Alumno alumno)
        {
            await Task.Delay(100); // Simulate network delay
            alumno.Id = _nextId++;
            _alumnos.Add(alumno);
            return alumno;
        }

        public async Task<Alumno> UpdateAlumnoAsync(Alumno alumno)
        {
            await Task.Delay(100); // Simulate network delay
            var index = _alumnos.FindIndex(a => a.Id == alumno.Id);
            if (index != -1)
            {
                _alumnos[index] = alumno;
                return alumno;
            }
            throw new ArgumentException("Alumno no encontrado");
        }

        public async Task DeleteAlumnoAsync(int id)
        {
            await Task.Delay(100); // Simulate network delay
            var index = _alumnos.FindIndex(a => a.Id == id);
            if (index != -1)
            {
                _alumnos.RemoveAt(index);
            }
            else
            {
                throw new ArgumentException("Alumno no encontrado");
            }
        }
    }
} 