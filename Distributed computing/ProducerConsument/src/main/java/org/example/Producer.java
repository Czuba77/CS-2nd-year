package org.example;

public class Producer  implements Runnable {
    String elementName;
    int elementIndex;
    int offset;
    private Storage Kebab;
    private volatile boolean running = true;

    public Producer(String elementName,Storage Kebab, int elementIndex, int offset) {
        this.elementName = elementName;
        this.elementIndex = elementIndex;
        this.Kebab = Kebab;
        this.offset = offset;
    }

    public String getElementName() {
        return elementName;
    }
    public int getElementIndex() {
        return elementIndex;
    }

    @Override
    public void run() {
        while(running){
            int waitingTime = (int)(Math.random() * 5000)+5000 + offset;
            try {
                Thread.sleep(waitingTime);
            } catch (InterruptedException e) {
                throw new RuntimeException(e);
            }
            if(Kebab.Produce(elementIndex,elementName)){
                System.out.println("Producer "+elementIndex+"| Kucharzowi "+ elementName + " udało się dostarczyć " + Kebab.getLabel(elementIndex));
            }
            else{
                System.out.println("Producer "+elementIndex+"| Kucharzowi "+ elementName + " NIE udało się dostarczyć " + Kebab.getLabel(elementIndex));
            }
            Kebab.printArray("\u001B[33m");
        }
    }

    public void stopRunning() {
        running = false;
    }
}
