using System.Windows.Controls;
using AlumniaNotes.viewModels;

namespace AlumniaNotes.vistas
{
    public partial class AlumnosView : UserControl
    {
        public AlumnosView()
        {
            InitializeComponent();
            DataContext = new AlumnoViewModel();
        }
    }
} 