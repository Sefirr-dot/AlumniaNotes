using System;
using System.Windows;
using AlumniaNotes.data;
using AlumniaNotes.models;
using AlumniaNotes.vistas;
using System.Windows.Input;
using System.ComponentModel;

namespace AlumniaNotes.viewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _nombreUsuario;
        private string _contrasena;
        private string _errorMessage;
        private bool _isLoading;

        public string NombreUsuario
        {
            get => _nombreUsuario;
            set => SetProperty(ref _nombreUsuario, value);
        }

        public string Contrasena
        {
            get => _contrasena;
            set => SetProperty(ref _contrasena, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public RelayCommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login, CanLogin);
        }

        private bool CanLogin(object parameter)
        {
            return !string.IsNullOrWhiteSpace(NombreUsuario) && 
                   !string.IsNullOrWhiteSpace(Contrasena) && 
                   !IsLoading;
        }

        private void Login(object parameter)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                if (ServiceLocator.UsuarioRepository.ValidateCredentials(NombreUsuario, Contrasena))
                {
                    var usuario = ServiceLocator.UsuarioRepository.GetByNombreUsuario(NombreUsuario);
                    var mainWindow = new MainView();
                    var mainViewModel = new MainViewModel(usuario);
                    mainWindow.DataContext = mainViewModel;
                    mainWindow.Show();

                    Application.Current.MainWindow?.Close();
                }
                else
                {
                    ErrorMessage = "Nombre de usuario o contraseña incorrectos";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error al iniciar sesión: " + ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
} 