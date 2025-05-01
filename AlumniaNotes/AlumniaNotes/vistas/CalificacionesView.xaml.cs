using System.Windows.Controls;
using AlumniaNotes.viewModels;

namespace AlumniaNotes.vistas
{
    public partial class CalificacionesView : UserControl
    {
        public CalificacionesView()
        {
            InitializeComponent();
            DataContext = new CalificacionViewModel();
        }
    }
} 