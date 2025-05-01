using System;
using System.Collections.ObjectModel;
using System.Linq;
using AlumniaNotes.data;
using AlumniaNotes.models;
using AlumniaNotes.services;

namespace AlumniaNotes.viewModels
{
    public class CalificacionViewModel : ViewModelBase
    {
        private ObservableCollection<Calificacion> _calificaciones;
        private Calificacion _selectedCalificacion;
        private ObservableCollection<Alumno> _alumnos;
        private ObservableCollection<Asignatura> _asignaturas;
        private Alumno _selectedAlumno;
        private Asignatura _selectedAsignatura;
        private string _searchText;

        public ObservableCollection<Calificacion> Calificaciones
        {
            get => _calificaciones;
            set => SetProperty(ref _calificaciones, value);
        }

        public Calificacion SelectedCalificacion
        {
            get => _selectedCalificacion;
            set => SetProperty(ref _selectedCalificacion, value);
        }

        public ObservableCollection<Alumno> Alumnos
        {
            get => _alumnos;
            set => SetProperty(ref _alumnos, value);
        }

        public ObservableCollection<Asignatura> Asignaturas
        {
            get => _asignaturas;
            set => SetProperty(ref _asignaturas, value);
        }

        public Alumno SelectedAlumno
        {
            get => _selectedAlumno;
            set => SetProperty(ref _selectedAlumno, value);
        }

        public Asignatura SelectedAsignatura
        {
            get => _selectedAsignatura;
            set => SetProperty(ref _selectedAsignatura, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    SearchCalificaciones();
                }
            }
        }

        public RelayCommand AddCalificacionCommand { get; }
        public RelayCommand EditCalificacionCommand { get; }
        public RelayCommand DeleteCalificacionCommand { get; }
        public RelayCommand SaveCalificacionCommand { get; }

        public CalificacionViewModel()
        {
            Calificaciones = new ObservableCollection<Calificacion>();
            Alumnos = new ObservableCollection<Alumno>();
            Asignaturas = new ObservableCollection<Asignatura>();
            
            AddCalificacionCommand = new RelayCommand(AddCalificacion);
            EditCalificacionCommand = new RelayCommand(EditCalificacion, CanEditCalificacion);
            DeleteCalificacionCommand = new RelayCommand(DeleteCalificacion, CanDeleteCalificacion);
            SaveCalificacionCommand = new RelayCommand(SaveCalificacion, CanSaveCalificacion);

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Cargar calificaciones
                var calificaciones = ServiceLocator.CalificacionRepository.GetAll();
                Calificaciones.Clear();
                foreach (var calificacion in calificaciones)
                {
                    Calificaciones.Add(calificacion);
                }

                // Cargar alumnos
                var alumnos = ServiceLocator.AlumnoRepository.GetAll();
                Alumnos.Clear();
                foreach (var alumno in alumnos)
                {
                    Alumnos.Add(alumno);
                }

                // Cargar asignaturas
                var asignaturas = ServiceLocator.AsignaturaRepository.GetAll();
                Asignaturas.Clear();
                foreach (var asignatura in asignaturas)
                {
                    Asignaturas.Add(asignatura);
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al cargar datos: {ex.Message}");
            }
        }

        private void SearchCalificaciones()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    LoadData();
                }
                else
                {
                    var calificaciones = ServiceLocator.CalificacionRepository.GetByAlumno(SearchText);
                    Calificaciones.Clear();
                    foreach (var calificacion in calificaciones)
                    {
                        Calificaciones.Add(calificacion);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al buscar calificaciones: {ex.Message}");
            }
        }

        private void AddCalificacion(object parameter)
        {
            SelectedCalificacion = new Calificacion();
        }

        private bool CanEditCalificacion(object parameter)
        {
            return SelectedCalificacion != null;
        }

        private void EditCalificacion(object parameter)
        {
            // No es necesario hacer nada aquí, ya que SelectedCalificacion ya está establecido
        }

        private bool CanDeleteCalificacion(object parameter)
        {
            return SelectedCalificacion != null;
        }

        private void DeleteCalificacion(object parameter)
        {
            try
            {
                if (SelectedCalificacion != null)
                {
                    if (ServiceLocator.MessageService.ShowConfirmation(
                        $"¿Está seguro de que desea eliminar la calificación de {SelectedCalificacion.Alumno.Nombre} en {SelectedCalificacion.Asignatura.Nombre}?"))
                    {
                        ServiceLocator.CalificacionRepository.Delete(SelectedCalificacion.Id);
                        Calificaciones.Remove(SelectedCalificacion);
                        SelectedCalificacion = null;
                        ServiceLocator.MessageService.ShowSuccess("Calificación eliminada correctamente");
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al eliminar calificación: {ex.Message}");
            }
        }

        private bool CanSaveCalificacion(object parameter)
        {
            return SelectedCalificacion != null &&
                   SelectedAlumno != null &&
                   SelectedAsignatura != null &&
                   SelectedCalificacion.Nota >= 0 &&
                   SelectedCalificacion.Nota <= 10;
        }

        private void SaveCalificacion(object parameter)
        {
            try
            {
                SelectedCalificacion.AlumnoId = SelectedAlumno.Id;
                SelectedCalificacion.AsignaturaId = SelectedAsignatura.Id;

                if (SelectedCalificacion.Id == 0)
                {
                    ServiceLocator.CalificacionRepository.Add(SelectedCalificacion);
                    Calificaciones.Add(SelectedCalificacion);
                    ServiceLocator.MessageService.ShowSuccess("Calificación agregada correctamente");
                }
                else
                {
                    ServiceLocator.CalificacionRepository.Update(SelectedCalificacion);
                    var index = Calificaciones.IndexOf(SelectedCalificacion);
                    if (index != -1)
                    {
                        Calificaciones[index] = SelectedCalificacion;
                    }
                    ServiceLocator.MessageService.ShowSuccess("Calificación actualizada correctamente");
                }
                SelectedCalificacion = null;
                SelectedAlumno = null;
                SelectedAsignatura = null;
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al guardar calificación: {ex.Message}");
            }
        }
    }
} 