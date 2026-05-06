using System.Windows.Input;
using PhoneBook.Commands;
using PhoneBook.Models;
using PhoneBook.Services;
using PhoneBook.ViewModels.Base;

namespace PhoneBook.ViewModels;

public class ContactEditViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService _navigationService;
    private Contact _contact = null;

    public string EditName
    {
        get => _contact.Name;
        set
        {
            _contact.Name = value;
            OnPropertyChanged();
        }
    }
    public string  EditPhone
    {
        get => _contact.Phone;
        set
        {
            _contact.Phone = value;
            OnPropertyChanged();
        }
    }
    
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public ContactEditViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        SaveCommand = new RelayCommand( 
            (() => _navigationService.NavigateTo<ContactsListViewModel>()));
        CancelCommand = new RelayCommand(
            () => _navigationService.NavigateTo<ContactsListViewModel>());
    }

    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is Contact contact) _contact = contact;
    }
}