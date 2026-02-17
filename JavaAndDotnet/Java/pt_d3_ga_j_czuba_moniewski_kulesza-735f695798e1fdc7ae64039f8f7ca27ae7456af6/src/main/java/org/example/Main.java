package org.example;

import java.util.TreeSet;
import java.util.HashSet;
import java.util.Set;

// Press Shift twice to open the Search Everywhere dialog and type `show whitespaces`,
// then press Enter. You can now see whitespace characters in your code.
public class Main {
    public static void main(String[] args) {
        if (args.length == 0) {
            System.out.println("Podaj tryb sortowania: brak, naturalny, comparator");
            return;
        }

        String mode = args[0];
        Set<RecursiveClass> people;

        switch (mode) {
            case "naturalny":
                people = new TreeSet<>();
                break;
            case "comparator":
                people = new TreeSet<>(new AltComparator());
                break;
            default:
                people = new HashSet<>();
        }

        RecursiveClass michal = new RecursiveClass(2, "Michał", "Kulesza", mode);
        RecursiveClass antek = new RecursiveClass(1, "Antek", "Czuba", mode);
        RecursiveClass janek = new RecursiveClass(3, "Janek", "Moniewski", mode);

        antek.AddChild(new RecursiveClass(6, "Mijka", "Czuba-Wozniak", mode));
        RecursiveClass humus = new RecursiveClass(4, "Humus", "Czuba", mode);
        humus.AddChild(new RecursiveClass(2, "Bibi", "Czuba", mode));
        antek.AddChild(humus);

        janek.AddChild(new RecursiveClass(7, "Bajtek", "Moniewski", mode));
        janek.AddChild(new RecursiveClass(3, "Kasza", "Moniewski", mode));
        janek.AddChild(new RecursiveClass(2, "Cekin", "Moniewski", mode));


        people.add(michal);
        people.add(antek);
        people.add(janek);

        for (RecursiveClass child : people) {
            System.out.println("Liczba potomstwa: "+child.CountChildren());
            child.printRecursive("\t");
        }
        //System.out.println(people);
    }
}