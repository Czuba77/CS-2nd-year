using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labpt8
{
    /// <summary>Obiekt wiążący się z Person, służy do sortowań LINQ/algorytmicznych.</summary>
    public class PersonKey : IComparable<PersonKey>
    {
        public Person Source { get; }

        public PersonKey(Person p) => Source = p;

        // domyślnie sortujemy po polu numerycznym Number
        public int CompareTo(PersonKey? other)
            => other == null ? 1 : Source.Number.CompareTo(other.Source.Number);

        // wygodne konwersje
        public static implicit operator Person(PersonKey k) => k.Source;
        public override string ToString() => Source.ToString();
    }
}