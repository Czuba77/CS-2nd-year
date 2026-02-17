package org.example;

import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

//TIP To <b>Run</b> code, press <shortcut actionId="Run"/> or
// click the <icon src="AllIcons.Actions.Execute"/> icon in the gutter.
public class Main {
    public static void main(String[] args) {
        List<Runnable> runnables = new ArrayList<Runnable>();
        List<Thread> threads = new ArrayList<Thread>();
        int numOfElements = 3;

        String labelArr[] = new String[3];
        labelArr[0] = "Kebab z kurczakiem";
        labelArr[1] = "Kebab z baranina";
        labelArr[2] = "Kebab z mieso mieszane";

        Storage kebab = new Storage(numOfElements,labelArr);
        List<Producer> producers = new ArrayList<>();
        List<Consument> consumers = new ArrayList<>();
        producers.add(new Producer("Kebab z kurczakiem", kebab, 0,(int)(Math.random() * 5000)));
        producers.add(new Producer("Kebab z baranina", kebab, 1,(int)(Math.random() * 5000)));
        producers.add(new Producer("Kebab z mieso mieszane", kebab, 2,(int)(Math.random() * 5000)));
        consumers.add(new Consument("Antoni", kebab, 0,(int)(Math.random() * 5000)));
        consumers.add(new Consument("Ania", kebab, 1,(int)(Math.random() * 5000)));
        consumers.add(new Consument("Krzysztof", kebab, 2,(int)(Math.random() * 5000)));
        runnables.addAll(producers);
        runnables.addAll(consumers);

        for (Runnable r : runnables) {
            Thread t = new Thread(r);
            threads.add(t);
            t.start();
        }
        Scanner scanner = new Scanner(System.in);
        while(true){
            if (scanner.hasNext()) {
                String input = scanner.next();

                // zatrzymaj wszystkie runnable
                for (Producer p : producers) {
                    p.stopRunning();
                }
                for (Consument c : consumers) {
                    c.stopRunning();
                }

                // czekaj aż wszystkie wątki się zakończą
                for (Thread t : threads) {
                    try {
                        t.join();
                    } catch (InterruptedException e) {
                        Thread.currentThread().interrupt();
                    }
                }
            }
            break;
        }

    }
}