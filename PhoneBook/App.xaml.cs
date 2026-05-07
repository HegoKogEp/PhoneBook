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
        /// <summary>
        /// Метод <see cref="OnStartup"/> , который вызывается при запуске приложения. Здесь мы настраиваем контейнер зависимостей и отображаем главное окно.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();
            
            // Регистрируем сервисы диалогов и навигации как Singleton
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, NavigationService>();
            
            // ContactsListViewModel решил зарегистрировать как Singleton, чтобы не терялся список контактов при пересоздании ViewModel
            // В качестве альтернативы можно было завести отдельный Singleton репозиторий или подключить БД, и тогда ContactsListViewModel был бы Transient
            services.AddSingleton<ContactsListViewModel>(); 
            
            services.AddTransient<AboutViewModel>();
            services.AddTransient<ContactsListView>();
            services.AddTransient<ContactEditViewModel>();
            // Регистрируем MainWindow и его MainViewModel как Singleton, так как это одно основное окно, в котором будет менять только ContentControl
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>(sp =>
            {
                var window = new MainWindow();
                window.DataContext = sp.GetRequiredService<MainViewModel>();
                return window;
            });
            
            var sp = services.BuildServiceProvider();
            sp.GetRequiredService<MainWindow>().Show();
        }
    }
}
