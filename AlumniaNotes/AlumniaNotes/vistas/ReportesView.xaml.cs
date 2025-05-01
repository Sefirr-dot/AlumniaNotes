using System.Windows.Controls;
using AlumniaNotes.viewModels;

namespace AlumniaNotes.vistas
{
    public partial class ReportesView : UserControl
    {
        public ReportesView()
        {
            InitializeComponent();
            DataContext = new ReporteViewModel();
        }
    }
} 