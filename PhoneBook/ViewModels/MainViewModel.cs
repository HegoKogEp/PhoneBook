using PhoneBook.Base;
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
