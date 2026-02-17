package org.example;

public class Storage{
    final int MAXIMUM_CAPACITY = 20;
    int [] StorageArr;
    int numOfDiffrentTypesOfEl;
    int numElements;
    String[] labelArr;

    public Storage(int size,String[] labelArr) {
        this.numOfDiffrentTypesOfEl = size;
        StorageArr = new int[size];
        numElements = 0;
        this.labelArr = labelArr;
    }

    public int getNumOfDiffrentTypesOfEl() {
        return numOfDiffrentTypesOfEl;
    }



    synchronized public boolean Consume(int elementIndex, String name){
        if (StorageArr[elementIndex] > 0){
            StorageArr[elementIndex]--;
            numElements--;
            return true;
        }
        return false;
    }

    synchronized public boolean Produce(int elementIndex, String name){
        if (numElements < MAXIMUM_CAPACITY){
            StorageArr[elementIndex]++;
            numElements++;

            return true;
        }
        return false;
    }

    public void printArray(String colour) {
        for (int i = 0; i <  StorageArr.length; i++) {
            System.out.println(colour + this.labelArr[i] +" "+ StorageArr[i]+ "\u001B[0m");
        }
    }

    public String getLabel(int index){
            return labelArr[index];
    }

}
