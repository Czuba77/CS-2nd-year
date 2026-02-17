package org.example;

import java.util.UUID;

public class Consument implements Runnable {
    private String name;
    private Storage Kebab;
    private int index;
    int offset;
    private volatile boolean running = true;

        public Consument(String name, Storage Kebab, int index, int offset) {
            this.name = name;
            this.Kebab = Kebab;
            this.index = index;
            this.offset = offset;
        }

        public String getName() {
            return name;
        }

        public Storage getKebab() {
            return Kebab;
        }
        public int getIndex() {
            return index;
        }

    @Override
    public void run() {
        while(running){
            int waitingTime = (int)(Math.random() * 5000)+5000+ offset;
            try {
                Thread.sleep(waitingTime);
            } catch (InterruptedException e) {
                throw new RuntimeException(e);
            }
            int choice= (int)(Math.random() * Kebab.getNumOfDiffrentTypesOfEl());
            if(Kebab.Consume(choice,name)){
                System.out.println("Consument "+index+"| Klientowi "+ name + " udało się zamówić " + Kebab.getLabel(choice));
            }
            else{
                System.out.println("Consument "+index+"| Klientowi "+ name + " NIE udało się zamówić " + Kebab.getLabel(choice));
            }
            Kebab.printArray("\u001B[32m");
        }
    }

    public void stopRunning() {
        running = false;
    }
}
