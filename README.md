# PhoneBook - Документация по проекту

## Содержание
1. [Phonebook](#phonebook)
   1. [App.xaml](#appxaml)
   2. [App.xaml.cs](#appxamlcs)
   3. [AssemblyInfo.cs](#assemblyinfocs)
   4. [PhoneBook.csproj](#phonebookcsproj)
2. [Phonebook/Base](#phonebook-base)
   1. [ObservableObject.cs](#observableobjectcs)
3. [Phonebook/Commands](#phonebook-commands)
   1. [RelayCommand.cs](#relaycommandcs)
4. [Phonebook/Models](#phonebook-models)
   1. [Contact.cs](#contactcs)
5. [Phonebook/Viewmodels](#phonebook-viewmodels)
   1. [MainViewModel.cs](#mainviewmodelcs)
6. [Phonebook/Views](#phonebook-views)
   1. [MainWindow.xaml](#mainwindowxaml)
   2. [MainWindow.xaml.cs](#mainwindowxamlcs)

## FILE 1: App.xaml

<a id='appxaml'></a>

```xml
﻿<Application x:Class="PhoneBook.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:local="clr-namespace:PhoneBook"
             StartupUri="Views\MainWindow.xaml">
    <Application.Resources>
         
    </Application.Resources>
</Application>
```

---

## FILE 2: App.xaml.cs

<a id='appxamlcs'></a>

```csharp
﻿using System.Configuration;
using System.Data;
using System.Windows;

namespace PhoneBook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    }

}
```

---

## FILE 3: AssemblyInfo.cs

<a id='assemblyinfocs'></a>

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

## FILE 4: ObservableObject.cs

<a id='observableobjectcs'></a>

```csharp
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PhoneBook.Base
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

## FILE 5: RelayCommand.cs

<a id='relaycommandcs'></a>

```csharp
﻿using System;
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

## FILE 6: Contact.cs

<a id='contactcs'></a>

```csharp
﻿using System;
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

## FILE 7: PhoneBook.csproj

<a id='phonebookcsproj'></a>

```xml
﻿<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net10.0-windows</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <UseWPF>true</UseWPF>
  </PropertyGroup>

</Project>
```

---

## FILE 8: MainViewModel.cs

<a id='mainviewmodelcs'></a>

```csharp
﻿using PhoneBook.Base;
using PhoneBook.Commands;
using PhoneBook.Models;
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
    public class MainViewModel : ObservableObject
    {

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

        public MainViewModel()
        {
            // Инициализация коллекции контактов и команд для добавления и удаления контактов
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
            if (contact.IsValid())
            {
                Contacts.Add(contact);
                Name = string.Empty;
                Phone = string.Empty;
            }
            else
            {
                MessageBox.Show("Имя не должно быть пустым, а номер должен соответствовать формату +7XXXXXXXXXX.", "Некорректные данные");
            }
        }

        private bool CanAddContact()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Phone);
        }

        /// <summary>
        /// Команда для удаления контакта, которая удаляет выбранный контакт из коллекции контактов, если он не равен <see langword="null"/>, и сбрасывает <see cref="SelectedContact"/> на <see langword="null"/>.
        /// </summary>
        private void RemoveContact()
        {
            if (SelectedContact != null)
            {
                Contacts.Remove(SelectedContact);
                SelectedContact = null;
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

## FILE 9: MainWindow.xaml

<a id='mainwindowxaml'></a>

```xml
﻿<!--Объявление MainViewModel как xmlns:vm в Window-->
<Window x:Class="PhoneBook.Views.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:PhoneBook.Views"
        xmlns:vm="clr-namespace:PhoneBook.ViewModels"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">

    <!--Подключение MainViewModel-->
    <Window.DataContext>
        <vm:MainViewModel/>
    </Window.DataContext>

    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
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

    </Grid>
</Window>
```

---

## FILE 10: MainWindow.xaml.cs

<a id='mainwindowxamlcs'></a>

```csharp
﻿using System;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
```

---

