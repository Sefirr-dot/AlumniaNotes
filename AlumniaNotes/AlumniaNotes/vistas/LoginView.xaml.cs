using System.Windows;
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
    }
} 