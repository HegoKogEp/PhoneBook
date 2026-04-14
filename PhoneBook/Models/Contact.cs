using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PhoneBook.Models
{
    public class Contact
    {
        private string _name = string.Empty;
        private string _phone = string.Empty;

        public string Name
        {
            get => _name;
            set
            {
                //if (string.IsNullOrWhiteSpace(value))
                //{
                //    throw new ArgumentException("Name cannot be empty.");
                //}
                _name = value;
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                //if (!Regex.IsMatch(value, @"^\+7\d{10}$")) ;
                //{
                //    throw new ArgumentException("Phone number must be in the format +7XXXXXXXXXX.");
                //}
                _phone = value;
            }
        }
    }
}
