using System.Windows.Input;
using PhoneBook.Commands;
using PhoneBook.Services;

namespace PhoneBook.ViewModels;

public class MainViewModel
{
    private readonly INavigationService _navigationService;
    public INavigationService NavigationService { get =>  _navigationService; }
    
    public ICommand ShowContactsCommand { get; }
    public ICommand ShowAboutCommand { get; }

    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        ShowContactsCommand = new RelayCommand(
            (() => _navigationService.NavigateTo<ContactsListViewModel>()));
        ShowAboutCommand = new RelayCommand(
            () => _navigationService.NavigateTo<AboutViewModel>());
    }
    
}