using Microsoft.Extensions.DependencyInjection;
using PhoneBook.ViewModels.Base;

namespace PhoneBook.Services;

public class NavigationService : ObservableObject, INavigationService
{
    private readonly IServiceProvider _serviceProvider;
    private object? _currentViewModel;
    public object? CurrentViewModel
    {
        get => _currentViewModel;
        private set
        {
            _currentViewModel = value;
            OnPropertyChanged();
        }
    }
    
    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void NavigateTo<TViewModel>(object? parameter = null) where TViewModel : class
    {
        var vm = _serviceProvider.GetRequiredService<TViewModel>();

        if (vm is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedTo(parameter);
        }
        
        CurrentViewModel = vm;
    }
}