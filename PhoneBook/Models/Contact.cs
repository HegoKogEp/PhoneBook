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
