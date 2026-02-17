package org.example;

import java.io.FileWriter;
import java.io.IOException;
import java.util.Random;

public class GeneratorDanych {
    public static void main(String[] args) {
        String csvFile = "data.csv";
        String[] maleNames = {
                "Adam", "Adrian", "Aleksander", "Andrzej", "Antoni", "Artur", "Bartlomiej", "Beniamin",
                "Blazej", "Cezary", "Damian", "Daniel", "Dawid", "Dominik", "Edward", "Eryk", "Fabian", "Filip",
                "Franciszek", "Gabriel", "Grzegorz", "Henryk", "Hubert", "Igor", "Jacek", "Jakub", "Jan", "Janusz",
                "Jaroslaw", "Jonasz", "Jozef", "Julian", "Kacper", "Kajetan", "Karol", "Kazimierz", "Konrad", "Krystian",
                "Ksawery", "Kuba", "Lech", "Leon", "Leonard", "Lukasz", "Maciej", "Maksymilian", "Marcel", "Marcin",
                "Mariusz", "Mateusz", "Maurycy", "Michal", "Mieczyslaw", "Miroslaw", "Natan", "Nikodem", "Norbert", "Olaf",
                "Oskar", "Pawel", "Piotr", "Przemyslaw", "Rafal", "Robert", "Roman", "Ryszard", "Sebastian", "Seweryn",
                "Slawomir", "Stanislaw", "Stefan", "Szymon", "Tadeusz", "Tomasz", "Wieslaw", "Wiktor", "Wincenty", "Witold",
                "Wladyslaw", "Zbigniew", "Zdzislaw"
        };
        int length = maleNames.length;
        try (FileWriter writer = new FileWriter(csvFile)) {
            writer.append("ID,Name,Surname,Age,BoneFractures\n");
            int indexName=0;
            Random random = new Random();
            int age=random.nextInt(81) + 20;
            int boneFractures=0;
            int i;
            for (i = 0; i < 20000000; i++) {
                age=random.nextInt(81) + 20;
                boneFractures=random.nextInt(15) + 3;
                String name=maleNames[random.nextInt(length)];
                writer.append(i + "," + name + ",Czuba," + age + "," + boneFractures + "\n");
            }
            for (i = 2000000; i < 40000000; i++) {
                age=random.nextInt(81) + 20;
                boneFractures=random.nextInt(7) + 6;
                String name=maleNames[random.nextInt(length)];
                writer.append(i + "," + name + ",Jaworski," + age + "," + boneFractures + "\n");
            }
            for (i = 40000000; i < 60000000; i++) {
                age=random.nextInt(81) + 20;
                boneFractures=random.nextInt(7) + 6;
                String name=maleNames[random.nextInt(length)];
                writer.append(i + "," + name + ",Moniewski," + age + "," + boneFractures + "\n");
            }
            for (i = 60000000; i < 80000000; i++) {
                age=random.nextInt(81) + 20;
                boneFractures=random.nextInt(7) + 6;
                String name=maleNames[random.nextInt(length)];
                writer.append(i + "," + name + ",Kulesza," + age + "," + boneFractures + "\n");
            }
            for (i = 80000000; i < 100000000; i++) {
                age=random.nextInt(81) + 20;
                boneFractures=random.nextInt(7) + 6;
                String name=maleNames[random.nextInt(length)];
                writer.append(i + "," + name + ",Hillar," + age + "," + boneFractures + "\n");
            }



        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
