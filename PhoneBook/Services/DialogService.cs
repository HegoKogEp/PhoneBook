using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.Services
{
    public class DialogService : IDialogService
    {
        public void ShowInfo(string message, string title = "Информация")
        {
            System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }
        public void ShowError(string message, string title = "Ошибка")
        {
            System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
        public void ShowWarning(string message, string title = "Предупреждение")
        {
            System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
        }
        public bool ShowConfirmation(string message, string title = "Подтверждение")
        {
            var result = System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
            return result == System.Windows.MessageBoxResult.Yes;
        }

    }
}
