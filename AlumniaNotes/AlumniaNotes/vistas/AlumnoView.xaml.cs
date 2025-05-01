using System.Windows;
using AlumniaNotes.viewModels;
using AlumniaNotes.services;

namespace AlumniaNotes.vistas
{
    public partial class AlumnoView : Window
    {
        public AlumnoView()
        {
            InitializeComponent();
            DataContext = new AlumnoViewModel(new AlumnoService());
        }
    }
} 