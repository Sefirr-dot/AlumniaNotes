using System.Windows;

namespace AlumniaNotes.services
{
    public interface IMessageBox
    {
        MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon);
    }
} 