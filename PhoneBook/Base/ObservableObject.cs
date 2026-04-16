using System;
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
