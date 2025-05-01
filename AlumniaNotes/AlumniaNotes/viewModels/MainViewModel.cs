using System.Windows.Input;
using AlumniaNotes.models;
using System.Windows.Controls;
using AlumniaNotes.data;
using AlumniaNotes.vistas;

namespace AlumniaNotes.viewModels
{
    public class MainViewModel : ViewModelBase
    {
        private UserControl _currentView;
        private Usuario _usuarioActual;

        public UserControl CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public Usuario UsuarioActual
        {
            get => _usuarioActual;
            set => SetProperty(ref _usuarioActual, value);
        }

        public RelayCommand NavigateCommand { get; }
        public RelayCommand LogoutCommand { get; }

        public MainViewModel(Usuario usuario)
        {
            UsuarioActual = usuario;
            NavigateCommand = new RelayCommand(Navigate);
            LogoutCommand = new RelayCommand(Logout);
            
            // Cargar la vista inicial
            Navigate("Alumnos");
        }

        private void Navigate(object parameter)
        {
            if (parameter is string viewName)
            {
                switch (viewName)
                {
                    case "Alumnos":
                        CurrentView = new AlumnosView();
                        break;
                    case "Asignaturas":
                        CurrentView = new AsignaturasView();
                        break;
                    case "Calificaciones":
                        CurrentView = new CalificacionesView();
                        break;
                    case "Asistencias":
                        CurrentView = new AsistenciasView();
                        break;
                    case "Reportes":
                        CurrentView = new ReportesView();
                        break;
                }
            }
        }

        private void Logout(object parameter)
        {
            var loginWindow = new LoginView();
            loginWindow.Show();
            
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Close();
            }
        }
    }
} 