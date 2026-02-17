package org.example;

import java.io.*;

public class TaskClass {
    private int numberOfThreads;
    private int counterOfLines;
    private double meanX, meanY;
    private double sumXY, sumXX, sumYY;
    private Runnable[] runnArray;
    private Thread[] thread;
    private long computingTime;
    private long[] times;
    private long startTime;
    private long endTime;
    private String inputFile;
    private ObjectOutputStream oos;


    public TaskClass(int numberOfThreads, String inputFile, ObjectOutputStream oos){

        this.oos = oos;

        this.startTime = System.currentTimeMillis();
        this.endTime=0;
        this.numberOfThreads = numberOfThreads;

        this.computingTime = 0;
        this.counterOfLines=10000000;
        this.meanX=0.0F;
        this.meanY=0.0F;
        this.sumXY=0.0F;
        this.sumXX=0.0F;
        this.sumYY=0.0F;
        this.inputFile=inputFile;

        //mean
        this.runnArray = new Runnable[numberOfThreads];
        this.thread = new Thread[numberOfThreads];
        this.times = new long[numberOfThreads];
        for (int i = 0; i < numberOfThreads; i++) {
            this.runnArray[i]=new MeanOfData(this,i,i*this.counterOfLines/numberOfThreads,(i+1)*this.counterOfLines/numberOfThreads-1,this.inputFile);
            this.thread[i]=new Thread(this.runnArray[i]);
            this.thread[i].start();
        }

        try {
            this.waitTillComputed();
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }

        for (int i = 0; i < numberOfThreads; i++) {
            this.thread[i].stop();
        }

        //sum
        for (int i = 0; i < numberOfThreads; i++) {
            this.runnArray[i]=new PearsonParameters(meanX,meanY,i*this.counterOfLines/numberOfThreads,(i+1)*this.counterOfLines/numberOfThreads-1,this.inputFile,this, i);
            this.thread[i]=new Thread(this.runnArray[i]);
            this.thread[i].start();
        }

        try {
            this.waitTillComputed();
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
        double r=sumXY/(Math.sqrt(sumXX)*Math.sqrt(sumYY));
        this.endTime = System.currentTimeMillis();
        this.computingTime = this.endTime - this.startTime;

       //System.out.println(numberOfThreads + " threads computed in " + this.computingTime + "ms");
       // System.out.println(numberOfThreads +" "+ this.computingTime);
        /*
        System.out.println("Computing time is " + computingTime + "ms");
        System.out.println("SumXY is " + sumXY);
        System.out.println("SumXX is " + sumXX);
        System.out.println("SumYY is " + sumYY);
        */
        //System.out.println("Pearson correlation is " + r);
        try {
            oos.writeObject(""+r);
        } catch (IOException e) {
            throw new RuntimeException(e);
        }

        for (int i = 0; i < numberOfThreads; i++) {
            this.thread[i].stop();
        }
    }



    synchronized  public void meanFromThread(double threadMeanXX,double threadMeanYY,long computingTime, int index) {
        if(meanX==0.0F){
            this.meanX=threadMeanXX;
            this.meanY=threadMeanYY;
        }
        else{
            this.meanX=(meanX+threadMeanXX)/2;
            this.meanY=(meanY+threadMeanYY)/2;
        }

        this.times[index]=computingTime;
    }


    synchronized  public void addFromThread(double threadSumXY,double threadSumXX,double threadSumYY,long computingTime, int index) {
        this.sumXY+=threadSumXY;
        this.sumXX+=threadSumXX;
        this.sumYY+=threadSumYY;
        this.times[index]=computingTime;
    }

    synchronized public void stopThreads(){
        for (int i = 0; i < numberOfThreads; i++) {
            if(this.thread[i].isAlive()) {
                this.thread[i].stop();
            }
        }
        System.out.println("Koniec wykonywania programu");
        System.exit(0);
    }

    private  void waitTillComputed() throws InterruptedException {
        boolean cond=true;
        while (cond) {
            for (int i = 0; i < numberOfThreads; i++) {
                cond = this.thread[i].isAlive();
                if (cond) {
                    break;
                }
            }

            //Thread.sleep(100);
        }

    }

}
