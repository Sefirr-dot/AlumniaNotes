using System.Windows;
using AlumniaNotes.viewModels;

namespace AlumniaNotes.vistas
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
} 