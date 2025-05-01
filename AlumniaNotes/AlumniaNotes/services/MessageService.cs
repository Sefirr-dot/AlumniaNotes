using System;
using System.Windows;

namespace AlumniaNotes.services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageBox _messageBox;

        public MessageService(IMessageBox messageBox)
        {
            _messageBox = messageBox ?? throw new ArgumentNullException(nameof(messageBox));
        }

        public void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("El mensaje no puede ser nulo o vacío.", nameof(message));

            _messageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowSuccess(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("El mensaje no puede ser nulo o vacío.", nameof(message));

            _messageBox.Show(message, "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowWarning(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("El mensaje no puede ser nulo o vacío.", nameof(message));

            _messageBox.Show(message, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public bool ShowConfirmation(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("El mensaje no puede ser nulo o vacío.", nameof(message));

            return _messageBox.Show(message, "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
} 