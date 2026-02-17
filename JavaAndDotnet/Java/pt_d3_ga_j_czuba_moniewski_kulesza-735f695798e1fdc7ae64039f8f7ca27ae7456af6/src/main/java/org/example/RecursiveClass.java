package org.example;


import java.util.Comparator;
import java.util.HashSet;
import java.util.Set;
import java.util.TreeSet;


public class RecursiveClass implements Comparable<RecursiveClass>{
    private int Number;
    private String Name;
    private String Surname;
    private Set<RecursiveClass> Children;

    public int GetNumber(){
        return this.Number;
    }
    public String GetName(){
        return this.Name;
    }
    public String GetSurname(){
        return this.Surname;
    }
    public RecursiveClass(int number, String name, String surname, String mode) {
        this.Number = number;
        this.Name = name;
        this.Surname = surname;
        switch (mode) {
            case "naturalny":
                Children = new TreeSet<>();
                break;
            case "comparator":
                Children = new TreeSet<>(new AltComparator());
                break;
            default:
                Children = new HashSet<>();
        }
    }

    @Override
    public String toString(){
        return Name + " " + Surname + "," + Number;
    }

    @Override
    public int hashCode(){
        int hash = 7;
        for (int i=0; i<Name.length(); i++) {
            hash +=Name.charAt(i);
        }
        for (int i=0; i<Surname.length(); i++) {
            hash += 13 * Surname.charAt(i);
        }
        hash += 17 * Number;
        return hash;
    }

    public Boolean equals(RecursiveClass o){
        if(Number == o.Number && Name.equals(o.Name) && Surname.equals(o.Surname)){
            return true;
        }
        return false;
    }

    @Override
    public int compareTo(RecursiveClass o) {
        if (o.Number < Number) {            //
            return 1;
        }
        else if (o.Number > Number) {       //
            return -1;
        }
        else {
            if (Name.compareTo(o.Name) == 0) {
                return Surname.compareTo(o.Surname);
            }
            else return Name.compareTo(o.Name);
        }
    }

    public void AddChild(RecursiveClass child){
        Children.add(child);
    }

    public void printRecursive(String depth){
        System.out.println(toString());
        for (RecursiveClass child : Children) {
            System.out.printf(depth);
            child.printRecursive (depth+"\t");
        }
    }

    public int CountChildren(){
        int childrenAmount=0;
        for (RecursiveClass child : Children) {
            childrenAmount++;
            childrenAmount+=child.CountChildren();
        }
        return childrenAmount;
    }
}

