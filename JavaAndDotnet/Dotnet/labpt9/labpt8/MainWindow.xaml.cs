using Lab8Plus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace labpt8
{
    public partial class MainWindow : Window
    {
        public SearchableSortableCollection<Person> People { get; }  // <-- public, żeby XAML widział

        private List<dynamic> linqResult;
        private int lastID = 1;
        private ICollectionView peopleView;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;              // <-- klucz do bindowania
            People = new SearchableSortableCollection<Person>();
            linqResult = new List<dynamic>();
            peopleView = CollectionViewSource.GetDefaultView(People);
        }

        private void PropertyBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                PopulatePropertyBox();
        }

        // 2) Użytkownik otworzył listę strzałką ▼
        private void PropertyBox_DropDownOpened(object sender, EventArgs e)
        {
            if (propertyBox.Items.Count == 0)        // ładujemy tylko raz!
                PopulatePropertyBox();
        }

        private void DoSearch(object sender, RoutedEventArgs e)
        {
            if (propertyBox.SelectedItem is not string propName) return;

            string raw = valueBox.Text.Trim();
            if (string.IsNullOrEmpty(raw)) return;

            // ustaw filtr w peopleView
            peopleView.Filter = obj =>
            {
                var val = obj.GetType().GetProperty(propName)?.GetValue(obj);

                if (val is string s)
                    return s.IndexOf(raw, StringComparison.OrdinalIgnoreCase) >= 0;

                if (val is int i && int.TryParse(raw, out int num))
                    return i == num;

                return false;   // inne typy ignorujemy
            };
            peopleView.Refresh();
        }

        // Enter w polu wartości = naciśnięcie "Szukaj"
        private void ValueBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) DoSearch(sender, e);
        }

        private void ClearSearch(object sender, RoutedEventArgs e)
        {
            peopleView.Filter = null;
            peopleView.Refresh();
            valueBox.Clear();
            valueBox.Focus();
        }

        private void PopulatePropertyBox()
        {
            Type itemType = null;

            // Jeśli kolekcja ma elementy – bierzemy typ pierwszego;
            // jeśli pusta, bierzemy generyczny argument T
            if (peopleGrid.Items.Count > 0 && peopleGrid.Items[0] != CollectionView.NewItemPlaceholder)
                itemType = peopleGrid.Items[0].GetType();
            else
                itemType = typeof(Person);          // <- fallback (lub People.GetType().GenericTypeArguments[0])

            var props = itemType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                .Where(p => p.PropertyType == typeof(string) ||
                                            p.PropertyType == typeof(int))
                                .Select(p => p.Name)
                                .OrderBy(n => n)
                                .ToList();

            propertyBox.ItemsSource = props;
            if (props.Count > 0) propertyBox.SelectedIndex = 0;
        }
        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            string q = searchBox.Text.Trim();
            if (int.TryParse(q, out int num))
            {
                var found = People.FindBy(nameof(Person.Number), num);
                peopleView.Filter = o => found.Contains((Person)o);
            }
            else
            {
                var foundName = People.FindBy(nameof(Person.Name), q);
                var foundSur = People.FindBy(nameof(Person.Surname), q);
                peopleView.Filter = o => foundName.Contains((Person)o) ||
                                         foundSur.Contains((Person)o);
            }
            peopleView.Refresh();
        }

        public int GetLastID()
        {
            return lastID;
            }

        public void AddLastID()
        {
            lastID+=1;
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
            People.Clear();
            People.Add(new Person(-1, "Pan", "ikśiński", ZodiacSign.Scorpio, 0)); // do xpath
            string RandomName, RandomSurname;
            int RandomNumber;
            ZodiacSign RandomZS;
            Random rnd = new Random();
            for (int i = 0; i < 100; ++i)
            {
                RandomNumber = rnd.Next(1, 10);
                RandomName = Names[rnd.Next(0, Names.Length)];
                RandomSurname = Surnames[rnd.Next(0, Surnames.Length)];
                RandomZS = (ZodiacSign)rnd.Next(0, Enum.GetNames(typeof(ZodiacSign)).Length);
                People.Add(new Person(RandomNumber, RandomName, RandomSurname, RandomZS, lastID));
                lastID++;
            }
        }
        private void SortAscending(object sender, RoutedEventArgs e)
         => People.SortBy(nameof(Person.Number));

        private void SortDescending(object sender, RoutedEventArgs e)
            => People.SortBy(nameof(Person.Number), ListSortDirection.Descending);
        private void AddPerson(object sender, RoutedEventArgs e)
        {
            var win = new Window1(People, lastID, this);
            win.ShowDialog();
        }

        private void DeletePerson(object sender, RoutedEventArgs e)
        {
            if (peopleGrid.SelectedItem is Person p) People.Remove(p);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PersonSelected(object? sender, SelectionChangedEventArgs e)
        {
            if (peopleGrid.SelectedItem is Person p)
                descriptionText.Text = $"Name: {p.Name}\nSurname: {p.Surname}\nNumber: {p.Number}\nZodiac: {p.Zs}\nID: {p.ID}";
        }


        private void Q1(object sender, RoutedEventArgs e)
        {
            linqResult = People
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
            serializer.Serialize(writer, People.ToList());
        }

        private void DeserializeFromXML(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem item || item.Tag is not string filePath) return;

            var serializer = new XmlSerializer(typeof(List<Person>));   // lub ObservableCollection<Person>
            using var reader = new StreamReader(filePath);

            // wczytujemy do listy, bo rzutowanie na ObservableCollection powodowało wyjątek
            var loaded = (List<Person>)serializer.Deserialize(reader);

            People.Clear();                   // czyścimy starą zawartość
            foreach (var p in loaded)         // kopiujemy rekordy
                People.Add(p);

            // odśwież maksymalne ID, jeśli go używasz
            lastID = People.Any() ? People.Max(x => x.ID) + 1 : 1;
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
                            from p in People
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
