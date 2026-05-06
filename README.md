# PhoneBook - Project Documentation

## Content

1. [PhoneBook](#phonebook)
   1. [App.xaml](#app)
   2. [App.xaml.cs](#appxaml)
   3. [AssemblyInfo.cs](#assemblyinfo)
   4. [PhoneBook.csproj](#phonebook)
2. [PhoneBook\Commands](#commands)
   1. [RelayCommand.cs](#relaycommand)
3. [PhoneBook\Models](#models)
   1. [Contact.cs](#contact)
4. [PhoneBook\Services](#services)
   1. [DialogService.cs](#dialogservice)
   2. [IDialogService.cs](#idialogservice)
   3. [INavigationAware.cs](#inavigationaware)
   4. [INavigationService.cs](#inavigationservice)
   5. [NavigationService.cs](#navigationservice)
5. [PhoneBook\ViewModels](#viewmodels)
   1. [AboutViewModel.cs](#aboutviewmodel)
   2. [ContactEditViewModel.cs](#contacteditviewmodel)
   3. [ContactsListViewModel.cs](#contactslistviewmodel)
   4. [MainViewModel.cs](#mainviewmodel)
6. [PhoneBook\ViewModels\Base](#base)
   1. [ObservableObject.cs](#observableobject)
7. [PhoneBook\Views](#views)
   1. [ContactEditView.xaml](#contacteditview)
   2. [ContactEditView.xaml.cs](#contacteditviewxaml)
   3. [ContactsListView.xaml](#contactslistview)
   4. [ContactsListView.xaml.cs](#contactslistviewxaml)
   5. [MainWindow.xaml](#mainwindow)
   6. [MainWindow.xaml.cs](#mainwindowxaml)

## FILE 1: Project Root

## PhoneBook

<a id='phonebook'></a>

## FILE 1: App.xaml

<a id='app'></a>

```xml
<Application x:Class="PhoneBook.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:vm="clr-namespace:PhoneBook.ViewModels"
             xmlns:v="clr-namespace:PhoneBook.Views">
    <Application.Resources>
         <DataTemplate DataType="{x:Type vm:ContactsListViewModel}">
             <v:ContactsListView/>
         </DataTemplate>
        <DataTemplate DataType="{x:Type vm:AboutViewModel}">
            
        </DataTemplate>
        <DataTemplate DataType="{x:Type vm:ContactEditViewModel}">
            <v:ContactEditView/>
        </DataTemplate>
    </Application.Resources>
</Application>

```

---

## FILE 2: App.xaml.cs

<a id='appxaml'></a>

```csharp
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
            
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, NavigationService>();
            
            services.AddTransient<ContactsListViewModel>();
            services.AddTransient<AboutViewModel>();
            services.AddTransient<ContactsListView>();
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

```

---

## FILE 3: AssemblyInfo.cs

<a id='assemblyinfo'></a>

```csharp
using System.Windows;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None,            //where theme specific resource dictionaries are located
                                                //(used if a resource is not found in the page,
                                                // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly   //where the generic resource dictionary is located
                                                //(used if a resource is not found in the page,
                                                // app, or any theme specific resource dictionaries)
)]

```

---

## FILE 4: PhoneBook.csproj

<a id='phonebook'></a>

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net10.0-windows</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <UseWPF>true</UseWPF>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="10.0.6" />
  </ItemGroup>

</Project>

```

---

## PhoneBook\Commands

<a id='commands'></a>

## FILE 5: RelayCommand.cs

<a id='relaycommand'></a>

```csharp
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PhoneBook.Commands
{
    // no parameter
    /// <summary>
    /// Класс RelayCommand, реализующий интерфейс <see cref="ICommand"/>, который позволяет создавать команды для привязки к элементам управления в WPF,
    /// не требующие параметров.
    /// </summary>
    /// <param name="execute">Делегат, представляющий метод, который будет выполнен при вызове команды.</param>
    /// <param name="canExecute">Делегат, представляющий метод, который определяет, может ли команда выполняться.</param>
    public class RelayCommand(Action execute, Func<bool>? canExecute = null) : ICommand
    {
        private readonly Action _execute = execute;
        private readonly Func<bool>? _canExecute = canExecute;
        public bool CanExecute(object? parameter = null)
            => _canExecute?.Invoke() ?? true;
        public void Execute(object? parameter)
        {
            if (CanExecute(parameter)) _execute.Invoke();
        }

        public event EventHandler? CanExecuteChanged //Автообновление CanExecute
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

    }
}

// with parameter
/// <summary>
/// Класс RelayCommand<T>, реализующий интерфейс <see cref="ICommand"/>, который позволяет создавать команды для привязки к элементам управления в WPF,
/// требующие параметры.
/// </summary>
/// <typeparam name="T">Тип параметра команды.</typeparam>
/// <param name="execute">Делегат, представляющий метод, который будет выполнен при вызове команды.</param>
/// <param name="canExecute">Делегат, представляющий метод, который определяет, может ли команда выполняться.</param>
public class RelayCommand<T>(Action<object?> execute, Predicate<object?>? canExecute = null) : ICommand
{
    private readonly Action<object?> _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    private readonly Predicate<object?>? _canExecute = canExecute;
    public bool CanExecute(object? parameter)
        => _canExecute?.Invoke(parameter) ?? true;
    public void Execute(object? parameter)
    {
        if (CanExecute(parameter)) _execute.Invoke(parameter);
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}
```

---

## PhoneBook\Models

<a id='models'></a>

## FILE 6: Contact.cs

<a id='contact'></a>

```csharp
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace PhoneBook.Models
{
    /// <summary>
    /// Модель контакта для телефонной книги, содержащая имя и номер телефона с методом валидации данных.
    /// </summary>
    public class Contact
    {
        private string _name = string.Empty;
        private string _phone = string.Empty;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Phone
        {
            get => _phone;
            set => _phone = value;
        }

        /// <summary>
        /// Определяет, содержит ли объект допустимые значения для имени и номера телефона в формате российского номера.
        /// </summary>
        /// <remarks>Метод проверяет только базовую корректность формата. Дополнительная валидация номера телефона (например, существование номера) не выполняется.</remarks>
        /// <returns>Значение <see langword="true"/>, если имя не является пустым или состоящим только из пробелов и номер
        /// телефона соответствует формату "+7" с десятью цифрами; в противном случае — <see langword="false"/>.</returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && Regex.IsMatch(Phone, @"^\+7\d{10}$");
        }
    }
}

```

---

## PhoneBook\Services

<a id='services'></a>

## FILE 7: DialogService.cs

<a id='dialogservice'></a>

```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.Services
{
    /// <summary>
    /// Реализация интерфейса IDialogService для отображения диалогов в приложении. Использует стандартные MessageBox для показа сообщений и запросов подтверждения от пользователя.
    /// </summary>
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

```

---

## FILE 8: IDialogService.cs

<a id='idialogservice'></a>

```csharp
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

```

---

## FILE 9: INavigationAware.cs

<a id='inavigationaware'></a>

```csharp
namespace PhoneBook.Services;

public interface INavigationAware
{
    void OnNavigatedTo(object? parameter);
}
```

---

## FILE 10: INavigationService.cs

<a id='inavigationservice'></a>

```csharp
namespace PhoneBook.Services;

public interface INavigationService
{
    object? CurrentViewModel { get; }
    
    void NavigateTo<TViewModel>(object? parameter = null) where TViewModel : class;
}
```

---

## FILE 11: NavigationService.cs

<a id='navigationservice'></a>

```csharp
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
```

---

## PhoneBook\ViewModels

<a id='viewmodels'></a>

## FILE 12: AboutViewModel.cs

<a id='aboutviewmodel'></a>

```csharp
namespace PhoneBook.ViewModels;

public class AboutViewModel
{
    public string AppName =>  "Телефонная книга MVVM";
    public string AppVersion =>  "Версия 2.0 (With Navigation)";
}
```

---

## FILE 13: ContactEditViewModel.cs

<a id='contacteditviewmodel'></a>

```csharp
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
```

---

## FILE 14: ContactsListViewModel.cs

<a id='contactslistviewmodel'></a>

```csharp
using PhoneBook.Commands;
using PhoneBook.Models;
using PhoneBook.Services;
using PhoneBook.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PhoneBook.ViewModels
{
    /// <summary>
    /// Класс MainViewModel, который наследуется от <see cref="ObservableObject"/> и представляет собой модель представления для главного окна приложения телефонной книги, 
    /// содержащий свойства для имени, номера телефона, выбранного контакта и коллекцию контактов, 
    /// а также команды для добавления и удаления контактов с соответствующими методами проверки возможности выполнения этих действий.
    /// </summary>
    /// /// <remarks>Валидация контакта происходит с помощью метода <see cref="Contact.IsValid"/> из класса <see cref="Contact"/></remarks>
    public class ContactsListViewModel : ObservableObject
    {
        // Внедрение зависимости для отображения диалогов
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        
        private string _name = string.Empty;
        private string _phone = string.Empty;
        private Contact _selectedContact;

        public ObservableCollection<Contact> Contacts { get; set; }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public string Phone
        {
            get => _phone;
            set => Set(ref _phone, value);
        }

        public Contact SelectedContact
        {
            get => _selectedContact;
            set => Set(ref _selectedContact, value);
        }

        public ICommand AddContactCommand { get; }

        public ICommand RemoveContactCommand { get; }

        public ContactsListViewModel(IDialogService dialogService, INavigationService  navigationService)
        {
            // Инициализация коллекции контактов и команд для добавления и удаления контактов
            // Внедрение зависимости для отображения диалогов
            _dialogService = dialogService;
            _navigationService = navigationService;
            Contacts = [];
            AddContactCommand = new RelayCommand(AddContact, CanAddContact);
            RemoveContactCommand = new RelayCommand(RemoveContact, CanRemoveContact);
        }

        /// <summary>
        /// Команда для добавления контакта, которая создает новый объект <see cref="Contact"/> с текущими значениями имени и телефона,
        /// проверяет его на валидность с помощью метода <see cref="Contact.IsValid"/> и добавляет его в коллекцию контактов, если данные корректны.
        /// </summary>
        private void AddContact()
        {
            var contact = new Contact { Name = Name, Phone = Phone };

            if (Contacts.Any(c => c.Phone == contact.Phone))
            {
                // Если контакт с таким номером телефона уже существует, отображаем предупреждение и не добавляем новый контакт
                _dialogService.ShowWarning("Контакт с таким номером телефона уже существует!", "Предупреждение");
                return;
            }

            if (contact.IsValid())
            {
                Contacts.Add(contact);
                Name = string.Empty;
                Phone = string.Empty;
                // Отображаем информационное сообщение о том, что контакт был успешно добавлен
                _dialogService.ShowInfo($"Контакт {contact.Name} добавлен.", "Контакт добавлен");
            }
            else
            {
                // Если данные некорректные, отображаем сообщение об ошибке
                _dialogService.ShowError("Имя не должно быть пустым, а номер должен соответствовать формату +7XXXXXXXXXX.", "Некорректные данные");
            }
        }

        private bool CanAddContact()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Phone);
        }

        /// <summary>
        /// Команда для удаления контакта, которая удаляет выбранный контакт из коллекции контактов, если он не равен <see langword="null"/>, 
        /// и сбрасывает <see cref="SelectedContact"/> на <see langword="null"/>.
        /// </summary>
        private void RemoveContact()
        {
            if (SelectedContact != null)
            {
                // Запрашиваем подтверждение у пользователя перед удалением контакта
                if (_dialogService.ShowConfirmation($"Вы уверены, что хотите удалить контакт {SelectedContact.Name}?", "Подтверждение удаления"))
                {
                    // Если пользователь подтвердил удаление, отображаем информационное сообщение о том, что контакт был удален, и удаляем его из коллекции контактов
                    _dialogService.ShowInfo($"Контакт {SelectedContact.Name} удален.", "Контакт удален");
                    Contacts.Remove(SelectedContact);
                }
            }
        }

        private bool CanRemoveContact()
        {
            return SelectedContact != null;
        }
    }
}

```

---

## FILE 15: MainViewModel.cs

<a id='mainviewmodel'></a>

```csharp
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
```

---

## PhoneBook\ViewModels\Base

<a id='base'></a>

## FILE 16: ObservableObject.cs

<a id='observableobject'></a>

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PhoneBook.ViewModels.Base
{
    /// <summary>
    /// Базовый класс для объектов, поддерживающих уведомление об изменении свойств, реализующий интерфейс <see cref="INotifyPropertyChanged"/> и предоставляющий методы для обновления свойств и уведомления об их изменении.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Метод для вызова события <see cref="PropertyChanged"/> при изменении свойства, который принимает имя измененного свойства и уведомляет подписчиков об этом изменении.
        /// </summary>
        /// <param name="propertyName">Имя измененного свойства. По умолчанию используется имя вызывающего свойства.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Метод для обновления значения свойства и вызова события <see cref="PropertyChanged"/> при изменении, который принимает ссылку на поле,
        /// новое значение и имя свойства, и обновляет значение поля, если оно отличается от текущего, и уведомляет об этом изменение.
        /// </summary>
        /// <typeparam name="T">Тип свойства.</typeparam>
        /// <param name="field">Ссылка на поле, которое хранит значение свойства.</param>
        /// <param name="value">Новое значение свойства.</param>
        /// <param name="propertyName">Имя измененного свойства. По умолчанию используется имя вызывающего свойства.</param>
        /// <returns>Возвращает <see langword="true"/>, если значение свойства было изменено, иначе <see langword="false"/>.</returns>
        protected bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}

```

---

## PhoneBook\Views

<a id='views'></a>

## FILE 17: ContactEditView.xaml

<a id='contacteditview'></a>

```xml
<UserControl x:Class="PhoneBook.Views.ContactEditView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:local="clr-namespace:PhoneBook.Views"
             mc:Ignorable="d"
             d:DesignHeight="300" d:DesignWidth="300">
    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        
        <TextBlock Grid.Row="0"
                   Text="Редактирование контакта"
                   FontSize="20"
                   FontWeight="Bold"
                   HorizontalAlignment="Center"/>
        
        <TextBox Grid.Row="1"
                 Text="{Binding EditName, UpdateSourceTrigger=PropertyChanged}"
                 Margin="5 10"/>
        <TextBox Grid.Row="2"
                 Text="{Binding EditPhone, UpdateSourceTrigger=PropertyChanged}"
                 Margin="5 10"/>
        
        <StackPanel Grid.Row="3"
                    Orientation="Horizontal"
                    HorizontalAlignment="Center">
            <Button Content="Сохранить"
                    Command="{Binding SaveCommand}"
                    Margin="10 0"/>
            <Button Content="Отменить"
                    Margin="10 0"
                    Command="{Binding CancelCommand}"/>
        </StackPanel>
    </Grid>
</UserControl>

```

---

## FILE 18: ContactEditView.xaml.cs

<a id='contacteditviewxaml'></a>

```csharp
using System.Windows.Controls;

namespace PhoneBook.Views;

public partial class ContactEditView : UserControl
{
    public ContactEditView()
    {
        InitializeComponent();
    }
}
```

---

## FILE 19: ContactsListView.xaml

<a id='contactslistview'></a>

```xml
<!--Объявление MainViewModel как xmlns:vm в Window-->
<UserControl x:Class="PhoneBook.Views.ContactsListView"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:PhoneBook.Views"
        xmlns:vm="clr-namespace:PhoneBook.ViewModels"
        mc:Ignorable="d">

    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>

        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="*"/>
            <ColumnDefinition Width="auto"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>

        <Label Grid.Row="0" Grid.Column="1"
               Content="Телефонная книга" 
               FontSize="20" 
               HorizontalAlignment="Center" 
               Margin="0 10"/>

        <Label Grid.Row="1" Grid.Column="0" 
               Content="Имя:" 
               VerticalAlignment="Center" 
               HorizontalAlignment="Right"/>

        <!--Привязка свойства Name из MainViewModel к TextBox-->
        <TextBox Grid.Row="1" Grid.Column="1"
                 Margin="0 5"
                 ToolTip="Имя"
                 HorizontalAlignment="Center"
                 Width="200"
                 Text="{Binding Name, UpdateSourceTrigger=PropertyChanged}"/>

        <Label Grid.Row="2" Grid.Column="0" 
               Content="Номер телефона:" 
               VerticalAlignment="Center" 
               HorizontalAlignment="Right"/>

        <!--Привязка свойства Phone из MainViewModel к TextBox-->
        <TextBox Grid.Row="2" Grid.Column="1"
                 Margin="0 5" 
                 ToolTip="Номер телефона"
                 HorizontalAlignment="Center"
                 Width="200"
                 Text="{Binding Phone, UpdateSourceTrigger=PropertyChanged}"/>

        <StackPanel Grid.Row="3" Grid.Column="1"
                    Orientation="Horizontal" 
                    HorizontalAlignment="Center"
                    Margin="0 5">
            <!--Кнопки для добавления и удаления контактов-->
            <!--К кнопкам привязаны команды из MainViewModel-->
            <Button Content="Добавить" 
                    Margin="0 0 5 0"
                    Command="{Binding AddContactCommand}"/>
            <Button Content="Удалить"
                    Command="{Binding RemoveContactCommand}"/>
        </StackPanel>

        <!--Привязка коллекции Contacts из MainViewModel к ListBox-->
        <ListBox Grid.Row="5" Grid.ColumnSpan="3"
                 ItemsSource="{Binding Contacts}"
                 SelectedItem="{Binding SelectedContact}"
                 Margin="5">
            <ListBox.ItemTemplate>
                <DataTemplate>
                    <StackPanel Orientation="Horizontal">
                        <TextBlock Text="{Binding Name}" 
                                   Margin="0 0 10 0"
                                   FontWeight="Bold"/>
                        <TextBlock Text="{Binding Phone}"/>
                    </StackPanel>
                </DataTemplate>
            </ListBox.ItemTemplate>
        </ListBox>

        <Button Grid.Row="6" Grid.Column="2"
                Content="Редактировать"
                Margin="5"/>
    </Grid>
</UserControl>

```

---

## FILE 20: ContactsListView.xaml.cs

<a id='contactslistviewxaml'></a>

```csharp
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhoneBook.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class ContactsListView : UserControl
    {
        
        public ContactsListView()
        {
            
            InitializeComponent();
        }
    }
}

```

---

## FILE 21: MainWindow.xaml

<a id='mainwindow'></a>

```xml
<Window x:Class="PhoneBook.Views.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:local="clr-namespace:PhoneBook.Views"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">
    <DockPanel>
        <StackPanel DockPanel.Dock="Top"
                    Orientation="Horizontal"
                    Background="LightGray">
            <Button Content="Контакты"
                    Command="{Binding ShowContactsCommand}"
                    Margin="5" Padding="10 2"/>
            <Button Content="О программе"
                    Command="{Binding ShowAboutCommand}"
                    Margin="5" Padding="10 2"/>
        </StackPanel>
        <ContentControl Content="{Binding NavigationService.CurrentViewModel}"/>
    </DockPanel>
</Window>

```

---

## FILE 22: MainWindow.xaml.cs

<a id='mainwindowxaml'></a>

```csharp
using System.Windows;

namespace PhoneBook.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
}
```

---

