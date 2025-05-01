using System.Windows;

namespace AlumniaNotes.services
{
    public class WpfMessageBox : IMessageBox
    {
        public MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            return MessageBox.Show(messageBoxText, caption, button, icon);
        }
    }
} 