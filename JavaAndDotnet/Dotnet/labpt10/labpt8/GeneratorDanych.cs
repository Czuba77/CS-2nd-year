using System;
using System.IO;

namespace ExampleApp
{
    /// <summary>
    /// Generates a CSV file named "data.csv" with 10 000 000 rows.
    /// Format per row: ID,FirstName,Hillar,Age,BoneFractures
    /// (male first names chosen uniformly at random, age 18–65, boneFractures 6–12).
    /// </summary>
    public static class GeneratorDanych
    {
        private static readonly string[] MaleNames = new[]
        {
            "Adam", "Adrian", "Aleksander", "Andrzej", "Antoni", "Artur", "Bartlomiej", "Beniamin",
            "Blazej", "Cezary", "Damian", "Daniel", "Dawid", "Dominik", "Edward", "Eryk", "Fabian", "Filip",
            "Franciszek", "Gabriel", "Grzegorz", "Henryk", "Hubert", "Igor", "Jacek", "Jakub", "Jan", "Janusz",
            "Jaroslaw", "Jonasz", "Jozef", "Julian", "Kacper", "Kajetan", "Karol", "Kazimierz", "Konrad", "Krystian",
            "Ksawery", "Lukasz", "Maciej", "Maksymilian", "Marcel", "Marcin", "Marek", "Mateusz", "Michal", "Mikolaj",
            "Miroslaw", "Mariusz", "Norbert", "Oskar", "Patryk", "Piotr", "Przemyslaw", "Radoslaw", "Robert", "Sebastian",
            "Stanislaw", "Szymon", "Tadeusz", "Tomasz", "Witold", "Wojciech", "Zbigniew", "Zdzislaw", "Zygmunt"
        };

        public static void GenerateData()
        {
            const string csvFile = "data.csv";
            const int totalRows = 100_000;
            var rand = new Random();

            using (var writer = new StreamWriter(csvFile))
            {
                // Write header (optional; Java version did not write a header)
                // writer.WriteLine("ID,FirstName,LastName,Age,BoneFractures");

                for (int i = 0; i < totalRows/2; i++)
                {
                    // Age: 18–65
                    int age = rand.Next(18, 66);
                    // Bone fractures: 6–12
                    int boneFractures = rand.Next(6, 13);
                    // Random male name from array
                    string firstName = MaleNames[rand.Next(MaleNames.Length)];
                    // Last name is always "Hillar"
                    writer.WriteLine($"{i},{firstName},Hillar,{age},{boneFractures}");
                }
                for (int i = totalRows / 2; i < totalRows; i++)
                {
                    // Age: 18–76
                    int age = rand.Next(18, 76);
                    // Bone fractures: 2-6
                    int boneFractures = rand.Next(2, 6);
                    // Random male name from array
                    string firstName = MaleNames[rand.Next(MaleNames.Length)];
                    // Last name is always "Moniewski"
                    writer.WriteLine($"{i},{firstName},Moniewski,{age},{boneFractures}");
                }
            }
        }
    }
}
