package app;
import jakarta.persistence.*;
import entity.*;

import java.util.*;

public class App {
    private static final EntityManagerFactory emf = Persistence.createEntityManagerFactory("fabryka");
    private static final Scanner scanner = new Scanner(System.in);

    public static void main(String[] args) {
        putData();
        menu();
        emf.close();
    }

    static void putData() {
        EntityManager em = emf.createEntityManager();
        em.getTransaction().begin();
        Dom dep1 = new Dom("Lubczykowa 18");
        Dom dep2 = new Dom("Świstaka 9");
        Kotek e1 = new Kotek("Bajtek",15,5,dep1);
        Kotek e2 = new Kotek("Kasza",12,10,dep1);
        Kotek e3 = new Kotek("Mijka",7,3,dep2);
        Kotek e4 = new Kotek("Humus",3,1,dep2);
        dep1.setKotki(Arrays.asList(e1, e2));
        em.persist(dep1);

        dep2.setKotki(Arrays.asList(e3, e4));
        em.persist(dep2);

        em.getTransaction().commit();
        em.close();
    }

    static void menu() {
        while (true) {
            System.out.println("1. Dodaj kotka\n2. Usuń kotka\n3. Pokaż kotki\n4. Zapytania\n5. Usun dom\n6. Wyjście");
            int choice = Integer.parseInt(scanner.nextLine());

            switch (choice) {
                case 1 -> addKotek();
                case 2 -> removeKotek();
                case 3 -> showKotki(true);
                case 4 -> showQueries();
                case 5 -> removeDom();
                case 6 -> { return; }
                default -> System.out.println("Nieprawidłowa opcja.");
            }
        }
    }

    static void addKotek() {
        EntityManager em = emf.createEntityManager();

        System.out.println("dom: ");
        showDomy();
        String departament = scanner.nextLine();
        while (departament.isEmpty() || departament.equals(" ") || em.find(Dom.class, departament) == null) {
            System.out.println("Podany dom nie istnieje: ");
            departament = scanner.nextLine();
        }
        System.out.print("Imię: ");
        String name = scanner.nextLine();
        System.out.print("Wiek: ");
        int age = Integer.parseInt(scanner.nextLine());
        System.out.print("Waga: ");
        int weight = Integer.parseInt(scanner.nextLine());

        em.getTransaction().begin();

        Dom dept = em.find(Dom.class, departament);

        Kotek emp = new Kotek(name, age, weight, dept);

        em.persist(emp);
        em.getTransaction().commit();
        em.close();
    }

    static void removeKotek() {
        showKotki();
        System.out.print("ID kotka do usunięcia: ");
        long id = Long.parseLong(scanner.nextLine());

        EntityManager em = emf.createEntityManager();
        em.getTransaction().begin();

        Kotek emp = em.find(Kotek.class, id);
        if (emp != null) em.remove(emp);

        em.getTransaction().commit();
        em.close();
    }

    static void removeDom() {
        showDomy();
        System.out.print("ID domu do usunięcia: ");
        long id = Long.parseLong(scanner.nextLine());

        EntityManager em = emf.createEntityManager();
        em.getTransaction().begin();

        Dom emp = em.find(Dom.class, id);
        if (emp != null) em.remove(emp);

        em.getTransaction().commit();
        em.close();
    }


    static void showKotki() {
        EntityManager em = emf.createEntityManager();
        List<Kotek> list = em.createQuery("SELECT k FROM Kotek k", Kotek.class).getResultList();
        for (Kotek k : list) {
                System.out.println(k.getId() + ": " + k.getName() + ", wiek: " + k.getAge() + ", waga: " + k.getWeight() + ", mieszka na: " + k.getDom().getName());
        }
        em.close();
    }
    static void showKotki(Boolean select) {
        EntityManager em = emf.createEntityManager();
        List<Kotek> list = em.createQuery("SELECT k FROM Kotek k", Kotek.class).getResultList();
        if (select) {
            System.out.print("Podaj ile kotków wyświetlić: ");
            int ilosc = Integer.parseInt(scanner.nextLine());
            if( ilosc <=0 ){
                showKotki();
                em.close();
                return;
            }
            int iter = 0;
            for (Kotek k : list) {
                System.out.println(k.getId() + ": " + k.getName() + ", wiek: " + k.getAge() + ", waga: " + k.getWeight() + ", mieszka na: " + k.getDom().getName());
                iter++;
                if (iter == ilosc) {
                    break;
                }
            }
        }
        em.close();
    }

    static void showDomy() {
        EntityManager em = emf.createEntityManager();
        List<Dom> list = em.createQuery("SELECT d FROM Dom d", Dom.class).getResultList();
        for (Dom d : list) {
            System.out.println(d.getId() + ": " + d.getName());
        }
        em.close();
    }

    static void showQueries() {
        EntityManager em = emf.createEntityManager();

        System.out.println("1. Kotki starsze niż 8 lat");
        em.createQuery("SELECT k FROM Kotek k WHERE k.age > 8", Kotek.class)
                .getResultList().forEach(k -> System.out.println(k.getName()));

        System.out.println("2. Top 3 wg wagi:");
        em.createQuery("SELECT k FROM Kotek k ORDER BY k.weight DESC", Kotek.class)
                .setMaxResults(3).getResultList().forEach(k -> System.out.println(k.getName()));

        System.out.println("3. Wszyscy z domu na Lubczykowej 18:");
        em.createQuery("SELECT k FROM Kotek k WHERE k.dom.name = 'Lubczykowa 18'", Kotek.class)
                .getResultList().forEach(k -> System.out.println(k.getName()));

        System.out.println("4. Średni wiek:");
        Double avgAge = em.createQuery("SELECT AVG(k.age) FROM Kotek k", Double.class).getSingleResult();
        System.out.println(avgAge);

        System.out.println("5. Liczba kotkow:");
        Long count = em.createQuery("SELECT COUNT(k) FROM Kotek k", Long.class).getSingleResult();
        System.out.println(count);

        em.close();
    }
}
