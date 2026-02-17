using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Lab8Plus
{
    /// <summary>
    /// ObservableCollection rozszerzona o SortBy(...) i FindBy(...).
    /// </summary>
    public class SearchableSortableCollection<T> : ObservableCollection<T>
    {
        /// Sortuje kolekcję po wskazanej właściwości, jeżeli jej typ
        /// implementuje IComparable.  W przeciwnym razie rzuca InvalidOperationException.
        public void SortBy(string propertyName,
                           ListSortDirection dir = ListSortDirection.Ascending)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException(nameof(propertyName));

            PropertyInfo prop = typeof(T).GetProperty(propertyName,
                                      BindingFlags.Instance | BindingFlags.Public)
                ?? throw new ArgumentException($"Brak właściwości {propertyName}");

            // czy typ właściwości implementuje IComparable?
            if (!typeof(IComparable).IsAssignableFrom(prop.PropertyType))
                throw new InvalidOperationException(
                    $"Typ {prop.PropertyType.Name} nie implementuje IComparable");

            // przygotuj kopię, posortuj, przesuń elementy na swoje nowe pozycje
            List<T> sorted = (dir == ListSortDirection.Ascending)
                                ? this.OrderBy(x => (IComparable)prop.GetValue(x)).ToList()
                                : this.OrderByDescending(x => (IComparable)prop.GetValue(x)).ToList();

            for (int i = 0; i < sorted.Count; i++)
            {
                int oldIndex = IndexOf(sorted[i]);
                if (oldIndex != i) Move(oldIndex, i);
            }
        }

        /// Zwraca WSZYSTKIE elementy, w których wartość propertyName
        /// (string lub int) równa się searchValue.
        public IEnumerable<T> FindBy(string propertyName, object searchValue)
        {
            if (searchValue is not string && searchValue is not int)
                throw new ArgumentException("FindBy wspiera tylko string lub Int32");

            PropertyInfo prop = typeof(T).GetProperty(propertyName,
                                      BindingFlags.Instance | BindingFlags.Public)
                ?? throw new ArgumentException($"Brak właściwości {propertyName}");

            return this.Where(item =>
            {
                object value = prop.GetValue(item);
                return Equals(value, searchValue);
            });
        }
    }
}