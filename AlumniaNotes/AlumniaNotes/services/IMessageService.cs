using System;

namespace AlumniaNotes.services
{
    public interface IMessageService
    {
        void ShowError(string message);
        void ShowWarning(string message);
        void ShowInfo(string message);
        void ShowSuccess(string message);
        bool ShowConfirmation(string message, string title = "Confirmación");
    }
} 