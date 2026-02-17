package org.example;

import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;

public class MeanOfData implements Runnable {
    private TaskClass task;
    private int index;
    private int counterOfLines;
    private int idStart,idEnd;
    private double meanXAge, meanYBones;
    private String inputFile;
    public MeanOfData(TaskClass task, int index, int idStart, int idEnd, String inputFile) {
        this.task = task;
        this.index = index;
        this.idStart = idStart;
        this.idEnd = idEnd;
        this.meanXAge = 1.0f;
        this.meanYBones = 1.0f;
        this.inputFile = inputFile;
        this.counterOfLines=0;
    }

    public void run() {
        double sumXAge = 0.0f;
        double sumYBones = 0.0f;

        try (BufferedReader reader = new BufferedReader(new FileReader(inputFile))) {
            long startTime = System.currentTimeMillis();
            String line;
            String[] values;

            line = reader.readLine();//pierwsza linia to naglowek

            do {
                line = reader.readLine();
                values = line.split(",");
            } while (Integer.parseInt(values[0]) != idStart);

            do {
                if((line = reader.readLine())==null){
                    break;
                };
                values = line.split(",");
                sumXAge += Float.parseFloat(values[3]);
                sumYBones += Float.parseFloat(values[4]);
                counterOfLines++;
            } while (Integer.parseInt(values[0]) != idEnd);

            meanXAge = sumXAge / counterOfLines;
            meanYBones = sumYBones / counterOfLines;
            long endTime = System.currentTimeMillis();

            this.task.meanFromThread(meanXAge,meanYBones,(endTime-startTime),this.index);

        } catch (IOException e) {
            System.out.println("An error occurred: " + e.getMessage());
        }
    }
}
