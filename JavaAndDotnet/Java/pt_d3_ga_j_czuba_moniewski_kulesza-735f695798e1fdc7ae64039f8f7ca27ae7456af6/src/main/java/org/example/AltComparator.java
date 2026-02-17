package org.example;
import java.util.Comparator;

class AltComparator implements Comparator<RecursiveClass> {
    @Override
    public int compare(RecursiveClass o1, RecursiveClass o2) {
        // Comparing order:
        // 1: Name
        // 2: Surname
        // 3: Number
        if (o1.GetName().compareTo(o2.GetName()) > 0) {
            return 1;                                                           // 1 - Name is lexicographically greater than o2.Name
        } else if (o1.GetName().compareTo(o2.GetName()) < 0) {
            return -1;                                                          // -1 - Name is lexicographically less than o2.Name
        } else {
            if (o1.GetSurname().compareTo(o2.GetSurname()) == 0) {              // 0 - The strings are the same
                return Integer.compare(o1.GetNumber(), o2.GetNumber());         // Return the result of comparing Number to o2.Number
            } else if (o1.GetSurname().compareTo(o2.GetSurname()) < 0) {
                return -1;                                                      // -1 - Surname is lexicographically less than o2.Surname
            } else {
                return 1;                                                       // 1 - Surname is lexicographically greater than o2.Surname
            }
        }
    }
}