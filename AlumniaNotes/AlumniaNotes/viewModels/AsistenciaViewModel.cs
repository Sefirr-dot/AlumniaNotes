using System;
using System.Collections.ObjectModel;
using System.Linq;
using AlumniaNotes.data;
using AlumniaNotes.models;
using AlumniaNotes.services;

namespace AlumniaNotes.viewModels
{
    public class AsistenciaViewModel : ViewModelBase
    {
        private ObservableCollection<Asistencia> _asistencias;
        private Asistencia _selectedAsistencia;
        private ObservableCollection<Alumno> _alumnos;
        private ObservableCollection<Asignatura> _asignaturas;
        private Alumno _selectedAlumno;
        private Asignatura _selectedAsignatura;
        private string _searchText;
        private DateTime _fechaSeleccionada;

        public ObservableCollection<Asistencia> Asistencias
        {
            get => _asistencias;
            set => SetProperty(ref _asistencias, value);
        }

        public Asistencia SelectedAsistencia
        {
            get => _selectedAsistencia;
            set => SetProperty(ref _selectedAsistencia, value);
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
                    SearchAsistencias();
                }
            }
        }

        public DateTime FechaSeleccionada
        {
            get => _fechaSeleccionada;
            set
            {
                if (SetProperty(ref _fechaSeleccionada, value))
                {
                    LoadAsistencias();
                }
            }
        }

        public RelayCommand AddAsistenciaCommand { get; }
        public RelayCommand EditAsistenciaCommand { get; }
        public RelayCommand DeleteAsistenciaCommand { get; }
        public RelayCommand SaveAsistenciaCommand { get; }

        public AsistenciaViewModel()
        {
            Asistencias = new ObservableCollection<Asistencia>();
            Alumnos = new ObservableCollection<Alumno>();
            Asignaturas = new ObservableCollection<Asignatura>();
            FechaSeleccionada = DateTime.Today;
            
            AddAsistenciaCommand = new RelayCommand(AddAsistencia);
            EditAsistenciaCommand = new RelayCommand(EditAsistencia, CanEditAsistencia);
            DeleteAsistenciaCommand = new RelayCommand(DeleteAsistencia, CanDeleteAsistencia);
            SaveAsistenciaCommand = new RelayCommand(SaveAsistencia, CanSaveAsistencia);

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Cargar asistencias
                LoadAsistencias();

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

        private void LoadAsistencias()
        {
            try
            {
                var asistencias = ServiceLocator.AsistenciaRepository.GetByFecha(FechaSeleccionada);
                Asistencias.Clear();
                foreach (var asistencia in asistencias)
                {
                    Asistencias.Add(asistencia);
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al cargar asistencias: {ex.Message}");
            }
        }

        private void SearchAsistencias()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    LoadAsistencias();
                }
                else
                {
                    var asistencias = ServiceLocator.AsistenciaRepository.GetByAlumno(SearchText);
                    Asistencias.Clear();
                    foreach (var asistencia in asistencias)
                    {
                        Asistencias.Add(asistencia);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al buscar asistencias: {ex.Message}");
            }
        }

        private void AddAsistencia(object parameter)
        {
            SelectedAsistencia = new Asistencia
            {
                Fecha = FechaSeleccionada
            };
        }

        private bool CanEditAsistencia(object parameter)
        {
            return SelectedAsistencia != null;
        }

        private void EditAsistencia(object parameter)
        {
            // No es necesario hacer nada aquí, ya que SelectedAsistencia ya está establecido
        }

        private bool CanDeleteAsistencia(object parameter)
        {
            return SelectedAsistencia != null;
        }

        private void DeleteAsistencia(object parameter)
        {
            try
            {
                if (SelectedAsistencia != null)
                {
                    if (ServiceLocator.MessageService.ShowConfirmation(
                        $"¿Está seguro de que desea eliminar la asistencia de {SelectedAsistencia.Alumno.Nombre} del {SelectedAsistencia.Fecha.ToShortDateString()}?"))
                    {
                        ServiceLocator.AsistenciaRepository.Delete(SelectedAsistencia.Id);
                        Asistencias.Remove(SelectedAsistencia);
                        SelectedAsistencia = null;
                        ServiceLocator.MessageService.ShowSuccess("Asistencia eliminada correctamente");
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al eliminar asistencia: {ex.Message}");
            }
        }

        private bool CanSaveAsistencia(object parameter)
        {
            return SelectedAsistencia != null &&
                   SelectedAlumno != null &&
                   SelectedAsignatura != null;
        }

        private void SaveAsistencia(object parameter)
        {
            try
            {
                SelectedAsistencia.AlumnoId = SelectedAlumno.Id;
                SelectedAsistencia.AsignaturaId = SelectedAsignatura.Id;
                SelectedAsistencia.Fecha = FechaSeleccionada;

                if (SelectedAsistencia.Id == 0)
                {
                    ServiceLocator.AsistenciaRepository.Add(SelectedAsistencia);
                    Asistencias.Add(SelectedAsistencia);
                    ServiceLocator.MessageService.ShowSuccess("Asistencia registrada correctamente");
                }
                else
                {
                    ServiceLocator.AsistenciaRepository.Update(SelectedAsistencia);
                    var index = Asistencias.IndexOf(SelectedAsistencia);
                    if (index != -1)
                    {
                        Asistencias[index] = SelectedAsistencia;
                    }
                    ServiceLocator.MessageService.ShowSuccess("Asistencia actualizada correctamente");
                }
                SelectedAsistencia = null;
                SelectedAlumno = null;
                SelectedAsignatura = null;
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al guardar asistencia: {ex.Message}");
            }
        }
    }
} 