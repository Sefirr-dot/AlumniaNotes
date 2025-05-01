using System;
using System.Collections.ObjectModel;
using System.Linq;
using AlumniaNotes.data;
using AlumniaNotes.models;
using AlumniaNotes.services;

namespace AlumniaNotes.viewModels
{
    public class AlumnoViewModel : ViewModelBase
    {
        private ObservableCollection<Alumno> _alumnos;
        private Alumno _selectedAlumno;
        private string _searchText;

        public ObservableCollection<Alumno> Alumnos
        {
            get => _alumnos;
            set => SetProperty(ref _alumnos, value);
        }

        public Alumno SelectedAlumno
        {
            get => _selectedAlumno;
            set => SetProperty(ref _selectedAlumno, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    SearchAlumnos();
                }
            }
        }

        public RelayCommand AddAlumnoCommand { get; }
        public RelayCommand EditAlumnoCommand { get; }
        public RelayCommand DeleteAlumnoCommand { get; }
        public RelayCommand SaveAlumnoCommand { get; }

        public AlumnoViewModel(AlumnoService alumnoService)
        {
            Alumnos = new ObservableCollection<Alumno>();
            AddAlumnoCommand = new RelayCommand(AddAlumno);
            EditAlumnoCommand = new RelayCommand(EditAlumno, CanEditAlumno);
            DeleteAlumnoCommand = new RelayCommand(DeleteAlumno, CanDeleteAlumno);
            SaveAlumnoCommand = new RelayCommand(SaveAlumno, CanSaveAlumno);

            LoadAlumnos();
        }

        private void LoadAlumnos()
        {
            try
            {
                var alumnos = ServiceLocator.AlumnoRepository.GetAll();
                Alumnos.Clear();
                foreach (var alumno in alumnos)
                {
                    Alumnos.Add(alumno);
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al cargar alumnos: {ex.Message}");
            }
        }

        private void SearchAlumnos()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    LoadAlumnos();
                }
                else
                {
                    var alumnos = ServiceLocator.AlumnoRepository.GetByNombre(SearchText);
                    Alumnos.Clear();
                    foreach (var alumno in alumnos)
                    {
                        Alumnos.Add(alumno);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al buscar alumnos: {ex.Message}");
            }
        }

        private void AddAlumno(object parameter)
        {
            SelectedAlumno = new Alumno();
        }

        private bool CanEditAlumno(object parameter)
        {
            return SelectedAlumno != null;
        }

        private void EditAlumno(object parameter)
        {
            // No es necesario hacer nada aquí, ya que SelectedAlumno ya está establecido
        }

        private bool CanDeleteAlumno(object parameter)
        {
            return SelectedAlumno != null;
        }

        private void DeleteAlumno(object parameter)
        {
            try
            {
                if (SelectedAlumno != null)
                {
                    if (ServiceLocator.MessageService.ShowConfirmation(
                        $"¿Está seguro de que desea eliminar al alumno {SelectedAlumno.Nombre} {SelectedAlumno.Apellidos}?"))
                    {
                        ServiceLocator.AlumnoRepository.Delete(SelectedAlumno.Id);
                        Alumnos.Remove(SelectedAlumno);
                        SelectedAlumno = null;
                        ServiceLocator.MessageService.ShowSuccess("Alumno eliminado correctamente");
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al eliminar alumno: {ex.Message}");
            }
        }

        private bool CanSaveAlumno(object parameter)
        {
            return SelectedAlumno != null &&
                   !string.IsNullOrWhiteSpace(SelectedAlumno.Nombre) &&
                   !string.IsNullOrWhiteSpace(SelectedAlumno.Apellidos) &&
                   !string.IsNullOrWhiteSpace(SelectedAlumno.DNI) &&
                   !string.IsNullOrWhiteSpace(SelectedAlumno.Email);
        }

        private void SaveAlumno(object parameter)
        {
            try
            {
                if (SelectedAlumno.Id == 0)
                {
                    ServiceLocator.AlumnoRepository.Add(SelectedAlumno);
                    Alumnos.Add(SelectedAlumno);
                    ServiceLocator.MessageService.ShowSuccess("Alumno agregado correctamente");
                }
                else
                {
                    ServiceLocator.AlumnoRepository.Update(SelectedAlumno);
                    var index = Alumnos.IndexOf(SelectedAlumno);
                    if (index != -1)
                    {
                        Alumnos[index] = SelectedAlumno;
                    }
                    ServiceLocator.MessageService.ShowSuccess("Alumno actualizado correctamente");
                }
                SelectedAlumno = null;
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al guardar alumno: {ex.Message}");
            }
        }
    }
} 
} 