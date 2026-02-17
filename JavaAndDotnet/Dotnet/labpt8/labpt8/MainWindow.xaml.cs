using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace labpt8
{
    public partial class MainWindow : Window
    {
        private List<Person> people;
        private List<dynamic> linqResult; // Dodano pole do przechowywania wyniku linqResult
        private int lastID=1;
        public int GetLastID()
        {
            return lastID;
            }

        public void AddLastID()
        {
            lastID+=1;
        }
        public MainWindow()
        {
            InitializeComponent();
            people = new List<Person>();
            linqResult = new List<dynamic>();
        }

        private void GenerateData(object sender, RoutedEventArgs e)
        {
            string[] Names = new string[]
            {
                    "Adam", "Adrian", "Alan", "Aleksander", "Andrzej", "Antoni", "Arkadiusz", "Artur", "Bartłomiej", "Bartosz",
                    "Błażej", "Bogdan", "Bolesław", "Cezary", "Cyprian", "Czesław", "Damian", "Daniel", "Dariusz", "Dawid",
                    "Dominik", "Edward", "Emil", "Ernest", "Eryk", "Eugeniusz", "Fabian", "Feliks", "Filip", "Franciszek",
                    "Gabriel", "Grzegorz", "Gustaw", "Henryk", "Hubert", "Igor", "Ireneusz", "Jacek", "Jakub", "Jan",
                    "Janusz", "Jarosław", "Jerzy", "Joachim", "Józef", "Julian", "Kacper", "Kajetan", "Karol", "Kazimierz",
                    "Klaudiusz", "Konrad", "Kornel", "Krzysztof", "Lech", "Leon", "Leszek", "Lucjan", "Ludwik", "Łukasz",
                    "Maciej", "Maksymilian", "Marcel", "Marek", "Marian", "Mariusz", "Mateusz", "Michał", "Miłosz", "Norbert",
                    "Olek", "Olgierd", "Oskar", "Patryk", "Paweł", "Piotr", "Przemysław", "Radosław", "Rafał", "Remigiusz",
                    "Robert", "Roman", "Ryszard", "Sebastian", "Seweryn", "Sławomir", "Stanisław", "Stefan", "Sylwester", "Szymon",
                    "Tadeusz", "Tomasz", "Waldemar", "Wiktor", "Witold", "Władysław", "Wojciech", "Zbigniew", "Zdzisław", "Zenon"
            };
            string[] Surnames = new string[]
            {
                    "Czuba","Kulesza","Moniewski"
            };
            people.Clear();
            people.Add(new Person(-1, "Pan", "ikśiński", ZodiacSign.Scorpio, 0)); // do xpath
            string RandomName, RandomSurname;
            int RandomNumber;
            ZodiacSign RandomZS;
            Random rnd = new Random();
            for (int i = 1; i < 101; i++)
            {
                RandomNumber = rnd.Next(1, 10);
                RandomName = Names[rnd.Next(0, Names.Length)];
                RandomSurname = Surnames[rnd.Next(0, Surnames.Length)];
                RandomZS = (ZodiacSign)rnd.Next(0, Enum.GetNames(typeof(ZodiacSign)).Length);
                people.Add(new Person(RandomNumber, RandomName, RandomSurname, RandomZS, lastID));
                lastID++;
            }

            RefreshList();
        }

        private void RefreshList()
        {
            personListBox.Items.Clear();
            foreach (var person in people)
            {
                personListBox.Items.Add(person);
            }
        }

        private void AddPerson(object sender, RoutedEventArgs e)
        {
            Window1 addWindow = new Window1(people,lastID, RefreshList,this);
            addWindow.ShowDialog();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PersonSelected(object sender, SelectionChangedEventArgs e)
        {
            if (personListBox.SelectedItem is Person selected)
            {
                descriptionText.Text = $"Name: {selected.Name}\n" +
                                       $"Surname: {selected.Surname}\n" +
                                       $"Number: {selected.Number}\n" +
                                       $"Zodiac: {selected.Zs}\n" +
                                       $"ID: {selected.ID}";
            }
        }

    
        private void Q1(object sender, RoutedEventArgs e)
        {
            linqResult = people
                  .Where(p => p.ID % 2 == 1)
                  .Select(p => (dynamic)new
                  {
                      SUM_OF = p.Number + (int)p.Zs,
                      UPPERCASE = p.Name.ToUpper()
                  }).ToList();

            descriptionText.Text = ""; 
            foreach (var item in linqResult)
                descriptionText.Text += $"{item.UPPERCASE}: {item.SUM_OF}\n";
        }

        private void Q2(object sender, RoutedEventArgs e)
        {
            if (linqResult == null || linqResult.Count == 0)
            {
                descriptionText.Text = "Najpierw uruchom Q1";
                return;
            }

            var grouped = linqResult
                .GroupBy(x => x.UPPERCASE)
                .Select(g => new
                {
                    Name = g.Key,
                    Avg = g.Average(x => (int)x.SUM_OF)
                });

            descriptionText.Text = "";
            foreach (var group in grouped)
                descriptionText.Text += $"Group: {group.Name}, Avg SUM_OF: {group.Avg:F2}\n";
        }

        private void SerializeToXML(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            var filePath = item?.Tag?.ToString();
            var serializer = new XmlSerializer(typeof(List<Person>));
            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, people);
        }

        private void DeserializeFromXML(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            var filePath = item?.Tag?.ToString();
            var serializer = new XmlSerializer(typeof(List<Person>));
            using var reader = new StreamReader(filePath);
            people = (List<Person>)serializer.Deserialize(reader);

            foreach (var p in people) {
                p.ID = lastID;
                lastID++;
            }
            RefreshList();
        }

        private void XpathExp(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            var filePath = item?.Tag?.ToString();
            var doc = XDocument.Load(filePath);
            var unique = doc.Root
                .Elements("Person")
                .GroupBy(x => (int)x.Element("Number"))
                .Where(g => g.Count() == 1)
                .Select(g => g.First());

            descriptionText.Text = "";
            foreach (var el in unique)
                descriptionText.Text += (el);

        }

        private void DHTP(object sender, RoutedEventArgs e)
        {


            XNamespace xhtml = "http://www.w3.org/1999/xhtml";

            var xhtmlDoc = new XDocument(
                new XDocumentType("html", null, null, null),
                new XElement(xhtml + "html",
                    new XElement(xhtml + "head",
                        new XElement(xhtml + "meta", new XAttribute("charset", "UTF-8")),
                        new XElement(xhtml + "title", "Lista osób")
                    ),
                    new XElement(xhtml + "body",
                        new XElement(xhtml + "table",
                            new XAttribute("border", "1"),
                            new XElement(xhtml + "tr",
                                new XElement(xhtml + "th", "Imię"),
                                new XElement(xhtml + "th", "Nazwisko"),
                                new XElement(xhtml + "th", "Numer"),
                                new XElement(xhtml + "th", "Znak zodiaku")
                            ),
                            from p in people
                            select new XElement(xhtml + "tr",
                                new XElement(xhtml + "td", p.Name),
                                new XElement(xhtml + "td", p.Surname),
                                new XElement(xhtml + "td", p.Number),
                                new XElement(xhtml + "td", p.Zs.ToString())
                            )
                        )
                    )
                )
            );

            xhtmlDoc.Save("people.xhtml"); 
        }

    }
}
