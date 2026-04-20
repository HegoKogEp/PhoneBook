using Microsoft.Extensions.DependencyInjection;
using PhoneBook.Services;
using PhoneBook.ViewModels;
using PhoneBook.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PhoneBook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddTransient<MainViewModel>();

            services.AddSingleton<MainWindow>(provider =>
            {
                var window = new MainWindow();
                window.DataContext = provider.GetRequiredService<MainViewModel>();
                return window;
            });

            var serviceProvider = services.BuildServiceProvider();
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
