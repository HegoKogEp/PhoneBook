using PhoneBook.Commands;
using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace PhoneBook.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _name = string.Empty;
        private string _phone = string.Empty;
        private Contact _selectedContact;

        public ObservableCollection<Contact> Contacts { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Phone
        {
            get => _phone;
            set 
            { 
                _phone = value;
                OnPropertyChanged();
            }
        }

        public Contact SelectedContact
        {
            get => _selectedContact;
            set 
            {
                _selectedContact = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddContactCommand { get; }

        public ICommand RemoveContactCommand { get; }

        public MainViewModel()
        {
            Contacts = new();
            AddContactCommand = new RelayCommand(AddContact, CanAddContact);
            RemoveContactCommand = new RelayCommand(RemoveContact, CanRemoveContact);
        }

        private void AddContact()
        {
            try
            {
                var contact = new Contact { Name = Name, Phone = Phone };
                Contacts.Add(contact);
                Name = string.Empty;
                Phone = string.Empty;

            }
            catch (ArgumentException ex)
            {
                // Handle validation errors (e.g., show a message to the user)
                System.Windows.MessageBox.Show(ex.Message); // temporary error handling, consider a more user-friendly approach
            }
        }

        private bool CanAddContact()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Phone);
        }

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

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
