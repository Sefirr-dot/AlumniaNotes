using System.Windows.Controls;
using AlumniaNotes.viewModels;

namespace AlumniaNotes.vistas
{
    public partial class AsignaturasView : UserControl
    {
        public AsignaturasView()
        {
            InitializeComponent();
            DataContext = new AsignaturaViewModel();
        }
    }
} 