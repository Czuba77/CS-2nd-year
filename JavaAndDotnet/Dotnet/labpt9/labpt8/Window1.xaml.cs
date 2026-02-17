using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace labpt8
{
    public partial class Window1 : Window
    {
        private ObservableCollection<Person> people;
        private MainWindow mainWind;
        private Action refreshCallback;
        private int lastID;
        public Window1(ObservableCollection<Person> peopleList,int lastID, MainWindow mainWind)
        {
            InitializeComponent();
            people = peopleList;
            this.lastID = lastID;
            this.mainWind = mainWind;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(numberBox.Text, out int number) &&
                number >= 1 && number <= 10 &&
                !string.IsNullOrWhiteSpace(nameBox.Text) &&
                !string.IsNullOrWhiteSpace(surnameBox.Text) &&
                zodiacBox.SelectedItem != null)
            {
                Enum.TryParse(zodiacBox.SelectedItem.ToString(), out ZodiacSign sign);

                people.Add(new Person(number, nameBox.Text, surnameBox.Text, sign, lastID));
                mainWind.AddLastID();
                refreshCallback?.Invoke();
                Close();
            }
            else
            {
                MessageBox.Show("Please fill in all fields correctly.\nNumber must be between 1 and 10.");
            }
        }
    }
}
