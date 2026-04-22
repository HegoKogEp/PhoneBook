using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.Services
{
    /// <summary>
    /// Интерфейс для отображения диалогов в приложении. Позволяет показывать информационные, 
    /// ошибочные и предупреждающие сообщения, а также запрашивать подтверждение от пользователя.
    /// </summary>
    public interface IDialogService
    {
        void ShowInfo(string message, string title = "Информация");
        void ShowError(string message, string title = "Ошибка");
        void ShowWarning(string message, string title = "Предупреждение");
        bool ShowConfirmation(string message, string title = "Подтверждение");
    }
}
