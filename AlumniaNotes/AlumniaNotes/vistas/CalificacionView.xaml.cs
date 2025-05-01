using System.Windows;
using AlumniaNotes.viewmodels;
using AlumniaNotes.services;

namespace AlumniaNotes.vistas
{
    public partial class CalificacionView : Window
    {
        public CalificacionView()
        {
            InitializeComponent();
            DataContext = new CalificacionViewModel(
                new CalificacionRepository(),
                new AlumnoRepository(),
                new AsignaturaRepository()
            );
        }
    }
} 