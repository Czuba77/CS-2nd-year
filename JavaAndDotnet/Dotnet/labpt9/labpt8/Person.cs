using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace labpt8
{
    public enum ZodiacSign
    {
        Aries,
        Taurus,
        Gemini,
        Cancer,
        Leo,
        Virgo,
        Libra,
        Scorpio,
        Sagittarius,
        Capricorn,
        Aquarius,
        Pisces
    }

    public class Person : INotifyPropertyChanged
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [XmlIgnore]
        public int ID { get; set; }
        public ZodiacSign Zs { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? p = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        public int GetZSToInt()
        {
            return (int)Zs;
        }


        public Person(int number, string name, string surname, ZodiacSign zs, int id)
        {
            Number = number;
            Name = name;
            Surname = surname;
            Zs = zs;
            ID =id;
        }

        public override string ToString()
        {
            return $"{Name} {Surname}, {Number}, {Zs}";
        }
        public Person()
        {
        }
    }
}
