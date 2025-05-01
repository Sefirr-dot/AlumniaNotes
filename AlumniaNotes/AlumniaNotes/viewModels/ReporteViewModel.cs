using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Win32;
using AlumniaNotes.data;
using AlumniaNotes.models;
using AlumniaNotes.services;

namespace AlumniaNotes.viewModels
{
    public class ReporteViewModel : ViewModelBase
    {
        private ObservableCollection<Reporte> _reportes;
        private Reporte _selectedReporte;
        private ObservableCollection<Alumno> _alumnos;
        private ObservableCollection<Asignatura> _asignaturas;
        private Alumno _selectedAlumno;
        private Asignatura _selectedAsignatura;
        private string _searchText;
        private DateTime _fechaInicio;
        private DateTime _fechaFin;
        private TipoReporte _tipoReporte;
        private FormatoExportacion _formatoExportacion;

        public ObservableCollection<Reporte> Reportes
        {
            get => _reportes;
            set => SetProperty(ref _reportes, value);
        }

        public Reporte SelectedReporte
        {
            get => _selectedReporte;
            set => SetProperty(ref _selectedReporte, value);
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
                    SearchReportes();
                }
            }
        }

        public DateTime FechaInicio
        {
            get => _fechaInicio;
            set
            {
                if (SetProperty(ref _fechaInicio, value))
                {
                    GenerarReporte();
                }
            }
        }

        public DateTime FechaFin
        {
            get => _fechaFin;
            set
            {
                if (SetProperty(ref _fechaFin, value))
                {
                    GenerarReporte();
                }
            }
        }

        public TipoReporte TipoReporte
        {
            get => _tipoReporte;
            set
            {
                if (SetProperty(ref _tipoReporte, value))
                {
                    GenerarReporte();
                }
            }
        }

        public FormatoExportacion FormatoExportacion
        {
            get => _formatoExportacion;
            set => SetProperty(ref _formatoExportacion, value);
        }

        public RelayCommand GenerarReporteCommand { get; }
        public RelayCommand ExportarReporteCommand { get; }
        public RelayCommand DeleteReporteCommand { get; }

        public ReporteViewModel()
        {
            Reportes = new ObservableCollection<Reporte>();
            Alumnos = new ObservableCollection<Alumno>();
            Asignaturas = new ObservableCollection<Asignatura>();
            FechaInicio = DateTime.Today.AddMonths(-1);
            FechaFin = DateTime.Today;
            TipoReporte = TipoReporte.Calificaciones;
            FormatoExportacion = FormatoExportacion.PDF;
            
            GenerarReporteCommand = new RelayCommand(GenerarReporte);
            ExportarReporteCommand = new RelayCommand(ExportarReporte, CanExportarReporte);
            DeleteReporteCommand = new RelayCommand(DeleteReporte, CanDeleteReporte);

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Cargar reportes
                var reportes = ServiceLocator.ReporteRepository.GetAll();
                Reportes.Clear();
                foreach (var reporte in reportes)
                {
                    Reportes.Add(reporte);
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

        private void SearchReportes()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    LoadData();
                }
                else
                {
                    var reportes = ServiceLocator.ReporteRepository.GetByAlumno(SearchText);
                    Reportes.Clear();
                    foreach (var reporte in reportes)
                    {
                        Reportes.Add(reporte);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al buscar reportes: {ex.Message}");
            }
        }

        private void GenerarReporte(object parameter = null)
        {
            try
            {
                if (FechaInicio > FechaFin)
                {
                    ServiceLocator.MessageService.ShowError("La fecha de inicio no puede ser mayor que la fecha de fin");
                    return;
                }

                var reporte = new Reporte
                {
                    Tipo = TipoReporte,
                    FechaInicio = FechaInicio,
                    FechaFin = FechaFin,
                    AlumnoId = SelectedAlumno?.Id ?? 0,
                    AsignaturaId = SelectedAsignatura?.Id ?? 0
                };

                ServiceLocator.ReporteRepository.GenerarReporte(reporte);
                Reportes.Add(reporte);
                ServiceLocator.MessageService.ShowSuccess("Reporte generado correctamente");
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al generar reporte: {ex.Message}");
            }
        }

        private bool CanExportarReporte(object parameter)
        {
            return SelectedReporte != null;
        }

        private void ExportarReporte(object parameter)
        {
            try
            {
                if (SelectedReporte != null)
                {
                    var saveFileDialog = new SaveFileDialog
                    {
                        Filter = FormatoExportacion switch
                        {
                            FormatoExportacion.PDF => "Archivos PDF|*.pdf",
                            FormatoExportacion.Excel => "Archivos Excel|*.xlsx",
                            FormatoExportacion.CSV => "Archivos CSV|*.csv",
                            _ => "Todos los archivos|*.*"
                        },
                        Title = "Exportar reporte",
                        FileName = $"Reporte_{SelectedReporte.Tipo}_{DateTime.Now:yyyyMMddHHmmss}"
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        switch (FormatoExportacion)
                        {
                            case FormatoExportacion.PDF:
                                ServiceLocator.ExportService.ExportToPdf(SelectedReporte, saveFileDialog.FileName);
                                break;
                            case FormatoExportacion.Excel:
                                ServiceLocator.ExportService.ExportToExcel(SelectedReporte, saveFileDialog.FileName);
                                break;
                            case FormatoExportacion.CSV:
                                ServiceLocator.ExportService.ExportToCsv(SelectedReporte, saveFileDialog.FileName);
                                break;
                        }

                        ServiceLocator.MessageService.ShowSuccess("Reporte exportado correctamente");
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al exportar reporte: {ex.Message}");
            }
        }

        private bool CanDeleteReporte(object parameter)
        {
            return SelectedReporte != null;
        }

        private void DeleteReporte(object parameter)
        {
            try
            {
                if (SelectedReporte != null)
                {
                    if (ServiceLocator.MessageService.ShowConfirmation(
                        $"¿Está seguro de que desea eliminar el reporte de {SelectedReporte.Alumno?.Nombre} del {SelectedReporte.FechaInicio.ToShortDateString()} al {SelectedReporte.FechaFin.ToShortDateString()}?"))
                    {
                        ServiceLocator.ReporteRepository.Delete(SelectedReporte.Id);
                        Reportes.Remove(SelectedReporte);
                        SelectedReporte = null;
                        ServiceLocator.MessageService.ShowSuccess("Reporte eliminado correctamente");
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.MessageService.ShowError($"Error al eliminar reporte: {ex.Message}");
            }
        }
    }

    public enum FormatoExportacion
    {
        PDF,
        Excel,
        CSV
    }
} 
} 