using System;
using System.Collections.Generic;
using System.Linq;
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

    public class Person
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [XmlIgnore]
        public int ID { get; set; }
        public ZodiacSign Zs { get; set; }

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
