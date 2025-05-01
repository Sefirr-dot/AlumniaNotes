using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AlumniaNotes.models
{
    public class Alumno : INotifyPropertyChanged
    {
        private int _id;
        private string _nombre;
        private string _apellidos;
        private string _dni;
        private DateTime _fechaNacimiento;
        private string _email;
        private string _telefono;
        private string _direccion;
        public List<Asignatura> Asignaturas { get; set; }
        public List<Calificacion> Calificaciones { get; set; }
        public List<Asistencia> Asistencias { get; set; }

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }

        public string Apellidos
        {
            get => _apellidos;
            set
            {
                _apellidos = value;
                OnPropertyChanged(nameof(Apellidos));
            }
        }

        public string DNI
        {
            get => _dni;
            set
            {
                _dni = value;
                OnPropertyChanged(nameof(DNI));
            }
        }

        public DateTime FechaNacimiento
        {
            get => _fechaNacimiento;
            set
            {
                _fechaNacimiento = value;
                OnPropertyChanged(nameof(FechaNacimiento));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Telefono
        {
            get => _telefono;
            set
            {
                _telefono = value;
                OnPropertyChanged(nameof(Telefono));
            }
        }

        public string Direccion
        {
            get => _direccion;
            set
            {
                _direccion = value;
                OnPropertyChanged(nameof(Direccion));
            }
        }

        public Alumno()
        {
            Asignaturas = new List<Asignatura>();
            Calificaciones = new List<Calificacion>();
            Asistencias = new List<Asistencia>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 