using System.Windows;
using AlumniaNotes.viewmodels;
using AlumniaNotes.services;

namespace AlumniaNotes.vistas
{
    public partial class AsistenciaView : Window
    {
        public AsistenciaView()
        {
            InitializeComponent();
            DataContext = new AsistenciaViewModel(new AsistenciaService(), new AlumnoService(), new AsignaturaService());
        }
    }
} 