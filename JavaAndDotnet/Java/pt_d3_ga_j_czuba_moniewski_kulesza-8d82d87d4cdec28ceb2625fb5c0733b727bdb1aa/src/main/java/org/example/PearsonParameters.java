package org.example;

import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.lang.Math;


public class PearsonParameters implements Runnable {
    private TaskClass task;
    private int index;
    private int idStart,idEnd;
    private double meanXAge, meanYBones, ParameterXX=0, ParameterYY=0, ParameterXY=0;
    String inputFile;
    public PearsonParameters(double meanXAge, double meanYBones, int idStart, int idEnd, String inputFile,TaskClass task,int index) {
        this.task = task;
        this.idStart = idStart;
        this.idEnd = idEnd;
        this.inputFile = inputFile;
        this.index=index;
        this.meanXAge=meanXAge;
        this.meanYBones=meanYBones;
    }

    public void run(){
        long startTime = System.currentTimeMillis();
        try (BufferedReader reader = new BufferedReader(new FileReader(inputFile))) {
            float xAge,yBones;
            String line;
            String[] values;

            line = reader.readLine();//pierwsza linia to naglowek

            do {
                line = reader.readLine();
                values = line.split(",");
            }while ( Integer.parseInt(values[0])!=idStart);
            do {
                line = reader.readLine();
                values = line.split(",");
                xAge=Float.parseFloat(values[3]);
                yBones=Float.parseFloat(values[4]);
                ParameterXX+=Math.pow((xAge-meanXAge),2);
                ParameterYY+=Math.pow((yBones-meanYBones),2);
                ParameterXY+=(yBones-meanYBones)*(xAge-meanXAge);
            }while ( Integer.parseInt(values[0])!=idEnd);
            long endTime = System.currentTimeMillis();
            this.task.addFromThread(ParameterXY,ParameterXX,ParameterYY,(endTime-startTime),this.index);
           // System.out.println(ParameterXY + " XY," + ParameterXX + " XX," + ParameterYY + " YY," + (endTime-startTime) + "," + index + " index");

        } catch (IOException e) {
            System.out.println("An error occurred: " + e.getMessage());
        }
    }
}