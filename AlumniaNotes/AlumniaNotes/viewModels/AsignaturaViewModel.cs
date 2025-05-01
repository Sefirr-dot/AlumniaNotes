using System;
using System.Collections.ObjectModel;
using System.Linq;
using AlumniaNotes.data;
using AlumniaNotes.models;
using AlumniaNotes.services;

namespace AlumniaNotes.viewModels
{
    public class AsignaturaViewModel : ViewModelBase
    {
        private ObservableCollection<Asignatura> _asignaturas;
        private Asignatura _selectedAsignatura;
        private string _searchText;

        public ObservableCollection<Asignatura> Asignaturas
        {
            get => _asignaturas;
            set => SetProperty(ref _asignaturas, value);
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
                    SearchAsignaturas();
                }
            }
        }

        public RelayCommand AddAsignaturaCommand { get; }
        public RelayCommand EditAsignaturaCommand { get; }
        public RelayCommand DeleteAsignaturaCommand { get; }
        public RelayCommand SaveAsignaturaCommand { get; }

        public AsignaturaViewModel(AsignaturaService asignaturaService, ProfesorService profesorService)
        {
            Asignaturas = new ObservableCollection<Asignatura>();
            AddAsignaturaCommand = new RelayCommand(AddAsignatura);
            EditAsignaturaCommand = new RelayCommand(EditAsignatura, CanEditAsignatura);
            DeleteAsignaturaCommand = new RelayCommand(DeleteAsignatura, CanDeleteAsignatura);
            SaveAsignaturaCommand = new RelayCommand(SaveAsignatura, CanSaveAsignatura);

            LoadAsignaturas();
        }

        private void LoadAsignaturas()
        {
            try
            {
                var asignaturas = ServiceLocator.AsignaturaRepository.GetAll();
                Asignaturas.Clear();
                foreach (var asignatura in asignaturas)
                {
                    Asignaturas.Add(asignatura);
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al cargar asignaturas: {ex.Message}");
            }
        }

        private void SearchAsignaturas()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    LoadAsignaturas();
                }
                else
                {
                    var asignaturas = ServiceLocator.AsignaturaRepository.GetByNombre(SearchText);
                    Asignaturas.Clear();
                    foreach (var asignatura in asignaturas)
                    {
                        Asignaturas.Add(asignatura);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al buscar asignaturas: {ex.Message}");
            }
        }

        private void AddAsignatura(object parameter)
        {
            SelectedAsignatura = new Asignatura();
        }

        private bool CanEditAsignatura(object parameter)
        {
            return SelectedAsignatura != null;
        }

        private void EditAsignatura(object parameter)
        {
            // No es necesario hacer nada aquí, ya que SelectedAsignatura ya está establecido
        }

        private bool CanDeleteAsignatura(object parameter)
        {
            return SelectedAsignatura != null;
        }

        private void DeleteAsignatura(object parameter)
        {
            try
            {
                if (SelectedAsignatura != null)
                {
                    if (ServiceLocator.MessageService.ShowConfirmation(
                        $"¿Está seguro de que desea eliminar la asignatura {SelectedAsignatura.Nombre}?"))
                    {
                        ServiceLocator.AsignaturaRepository.Delete(SelectedAsignatura.Id);
                        Asignaturas.Remove(SelectedAsignatura);
                        SelectedAsignatura = null;
                        ServiceLocator.MessageService.ShowSuccess("Asignatura eliminada correctamente");
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al eliminar asignatura: {ex.Message}");
            }
        }

        private bool CanSaveAsignatura(object parameter)
        {
            return SelectedAsignatura != null &&
                   !string.IsNullOrWhiteSpace(SelectedAsignatura.Nombre) &&
                   !string.IsNullOrWhiteSpace(SelectedAsignatura.Descripcion);
        }

        private void SaveAsignatura(object parameter)
        {
            try
            {
                if (SelectedAsignatura.Id == 0)
                {
                    ServiceLocator.AsignaturaRepository.Add(SelectedAsignatura);
                    Asignaturas.Add(SelectedAsignatura);
                    ServiceLocator.MessageService.ShowSuccess("Asignatura agregada correctamente");
                }
                else
                {
                    ServiceLocator.AsignaturaRepository.Update(SelectedAsignatura);
                    var index = Asignaturas.IndexOf(SelectedAsignatura);
                    if (index != -1)
                    {
                        Asignaturas[index] = SelectedAsignatura;
                    }
                    ServiceLocator.MessageService.ShowSuccess("Asignatura actualizada correctamente");
                }
                SelectedAsignatura = null;
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al guardar asignatura: {ex.Message}");
            }
        }
    }
} 
} 