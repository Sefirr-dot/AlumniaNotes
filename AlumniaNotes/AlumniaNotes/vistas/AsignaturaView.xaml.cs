using System.Windows;
using AlumniaNotes.viewmodels;
using AlumniaNotes.services;

namespace AlumniaNotes.vistas
{
    public partial class AsignaturaView : Window
    {
        public AsignaturaView()
        {
            InitializeComponent();
            DataContext = new AsignaturaViewModel(new AsignaturaRepository(), new ProfesorRepository());
        }
    }
} 