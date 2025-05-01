using System.Windows;
using System.Windows.Controls;
using AlumniaNotes.viewModels;

namespace AlumniaNotes.vistas
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                var passwordBox = sender as PasswordBox;
                if (passwordBox != null)
                {
                    var viewModel = DataContext as dynamic;
                    if (viewModel != null)
                    {
                        viewModel.Password = passwordBox.Password;
                    }
                }
            }
        }
    }
} 