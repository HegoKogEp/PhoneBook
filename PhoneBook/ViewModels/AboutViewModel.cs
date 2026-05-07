namespace PhoneBook.ViewModels;

/// <summary>
/// ViewModel для UserControl AboutView. Содержит в себе два поля с информацией о программе, которые привязаны к TextBlock 
/// </summary>
public class AboutViewModel
{
    public string AppName =>  "Телефонная книга MVVM";
    public string AppVersion =>  "Версия 2.0 (With Navigation)";
}