using System.Windows.Controls;
using AlumniaNotes.viewModels;

namespace AlumniaNotes.vistas
{
    public partial class AsistenciasView : UserControl
    {
        public AsistenciasView()
        {
            InitializeComponent();
            DataContext = new AsistenciaViewModel();
        }
    }
} 